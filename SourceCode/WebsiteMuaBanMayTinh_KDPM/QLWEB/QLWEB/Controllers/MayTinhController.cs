using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.Entity;
using System.Data.EntityClient;
using System.Data.EntityModel;
using System.Data.SqlClient;
using QLWEB.Models;
using System.IO;
using System.Net.Mail;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Net.Http;
using Newtonsoft.Json;
using System.Web.Security;

namespace QLWEB.Controllers
{
    public class MayTinhController : Controller
    {
        //
        // GET: /MayTinh/
        CuaHangMayTinhEntities9 db = new CuaHangMayTinhEntities9();
        public DbSet<DanhGia> DanhGias { get; set; }

        //cho khach hang : (xem)
        public ActionResult Index(string DanhMucID, string txtSearch)
        {
            ViewBag.DanhMucs = db.DanhMucs.ToList();
            var products = db.MTs.Include("DanhMuc").AsQueryable();

            if (!string.IsNullOrEmpty(DanhMucID))
            {
                int danhMucId = int.Parse(DanhMucID);
                products = products.Where(m => m.DanhMucID == danhMucId);
            }

            if (!string.IsNullOrEmpty(txtSearch))
            {
                products = products.Where(m => m.TenSP.Contains(txtSearch));
            }

            var tam = db.MTs.Take(3).ToList();

            if (tam != null && tam.Any())
            {
                ViewData["gioithieu"] = tam;
            }
            else
            {
                ViewData["gioithieu"] = new List<MT>();
            }
            return View(products.ToList());
        }

        public ActionResult test(string DanhMucID, string txtSearch)
        {
            ViewBag.DanhMucs = db.DanhMucs.ToList();
            var products = db.MTs.Include("DanhMuc").AsQueryable();

            if (!string.IsNullOrEmpty(DanhMucID))
            {
                int danhMucId = int.Parse(DanhMucID);
                products = products.Where(m => m.DanhMucID == danhMucId);
            }

            if (!string.IsNullOrEmpty(txtSearch))
            {
                products = products.Where(m => m.TenSP.Contains(txtSearch));
            }

            return View(products.ToList());
        }

        public ActionResult LocDanhMuc(int danhMucID)
        {
            var locsp = db.MTs.Where(m => m.DanhMucID == danhMucID).Include("DanhMuc").ToList();
            return View("Index", locsp);
        }

        public ActionResult Search(string searchTerm)
        {
            var searchResults = db.MTs.Where(m => m.TenSP.Contains(searchTerm)).ToList();
            return View("Index", searchResults);
        }

        [HttpGet]
        public JsonResult GetSuggestions(string term)
        {
            var suggestions = db.MTs
                .Where(m => m.TenSP.Contains(term))
                .Select(m => new
                {
                    ID = m.MTID,
                    Name = m.TenSP,
                    Img = m.DuongDan
                })
                .Take(10)
                .ToList();

            return Json(suggestions, JsonRequestBehavior.AllowGet);
        }


        private List<object> GetAprioriSuggestions(string keyword)
        {
            var matchedIds = db.MTs
                .Where(m => m.TenSP.Contains(keyword))
                .Select(m => m.MTID)
                .ToList();

            var totalOrders = db.CTDonHangs
                .Select(c => c.DonHangID)
                .Distinct()
                .Count();

            if (totalOrders == 0) return new List<object>();

            var relatedProducts = db.CTDonHangs
                .Where(ct => matchedIds.Contains(ct.MTID))
                .Join(db.CTDonHangs, a => a.DonHangID, b => b.DonHangID, (a, b) => new { a, b })
                .Where(joined => !matchedIds.Contains(joined.b.MTID))
                .GroupBy(j => j.b.MTID)
                .Select(g => new
                {
                    ProductID = g.Key,
                    Count = g.Select(x => x.a.DonHangID).Distinct().Count()
                })
                .Where(x => (double)x.Count / totalOrders >= 0.5)
                .Join(db.MTs, x => x.ProductID, mt => mt.MTID, (x, mt) => new
                {
                    ID = mt.MTID,
                    Name = mt.TenSP
                })
                .ToList<object>();

            return relatedProducts;
        }



        public ActionResult Details(int id)
        {
            var product = db.MTs.Include("DanhMuc").FirstOrDefault(m => m.MTID == id);
            if (product == null)
            {
                return HttpNotFound();
            }

            // Lấy đánh giá cho sản phẩm
            var danhGias = db.DanhGias
                .Where(d => d.MTID == id)
                .Include(d => d.KhachHang)
                .OrderByDescending(d => d.NgayDanhGia)
                .ToList();

            ViewBag.DanhGias = danhGias;

            // Lấy danh sách sản phẩm gợi ý
            var associatedProductIDs = GetAprioriAssociatedProducts(id);
            List<MT> aprioriSuggestedProducts = new List<MT>();
            if (associatedProductIDs != null && associatedProductIDs.Any())
            {
                aprioriSuggestedProducts = db.MTs.Where(m => associatedProductIDs.Contains(m.MTID)).ToList();
            }
            else
            {
                Random rand = new Random();
                int criteria = rand.Next(2);
                IQueryable<MT> suggestedProducts = db.MTs.Where(m => m.MTID != id);

                // Filter products with ratings of 4 or higher
                var productsWithHighRatings = suggestedProducts
                    .Where(m => db.DanhGias
                        .Where(d => d.MTID == m.MTID)
                        .Average(d => d.DiemDanhGia) >= 4);

                // Apply the logic based on the random criteria (brand or category)
                if (criteria == 0)
                {
                    productsWithHighRatings = productsWithHighRatings.Where(m => m.ThuongHieu == product.ThuongHieu);
                }
                else
                {
                    productsWithHighRatings = productsWithHighRatings.Where(m => m.DanhMucID == product.DanhMucID);
                }

                // If there are no products with high ratings, fallback to random suggestion
                aprioriSuggestedProducts = productsWithHighRatings.OrderBy(x => Guid.NewGuid()).Take(20).ToList();

                // If no high-rated products are found, suggest randomly
                if (!aprioriSuggestedProducts.Any())
                {
                    aprioriSuggestedProducts = suggestedProducts
                        .OrderBy(x => Guid.NewGuid()) // Randomize order
                        .Take(20)
                        .ToList();
                }
            }

            ViewBag.SuggestedProducts = aprioriSuggestedProducts;
            return View(product);
        }



        //review
        [HttpPost]
        public ActionResult GuiDanhGia(int MTID, int DiemDanhGia, string NoiDungDanhGia)
        {
            if (Session["KhachHangID"] == null)
            {
                return RedirectToAction("Login", "MayTinh");
            }

            int khachHangID = Convert.ToInt32(Session["KhachHangID"]);

            DanhGia danhGia = new DanhGia
            {
                MTID = MTID,
                KhachHangID = khachHangID,
                DiemDanhGia = DiemDanhGia,
                NoiDungDanhGia = NoiDungDanhGia,
                NgayDanhGia = DateTime.Now
            };

            db.DanhGias.Add(danhGia);
            db.SaveChanges();

            return RedirectToAction("Details", new { id = MTID });
        }



        // Hàm lấy các sản phẩm liên quan dựa trên thuật toán Apriori
        private List<int> GetAprioriAssociatedProducts(int productId)
        {
            // Lấy danh sách giao dịch dưới dạng danh sách các bộ sản phẩm (list of product IDs)
            var transactions = db.CTDonHangs
                                 .GroupBy(ct => ct.DonHangID)
                                 .Select(g => g.Select(ct => ct.MTID).ToList())
                                 .ToList();

            // Đặt các ngưỡng hỗ trợ (support) và tin cậy (confidence)
            int minSupportCount = 3; // Ví dụ: candidate phải xuất hiện cùng sản phẩm đang xem ít nhất 3 lần.
            double minConfidence = 0.5; // Ví dụ: độ tin cậy tối thiểu là 50%

            // Lấy tất cả các giao dịch chứa sản phẩm hiện tại
            var transactionsContainingProduct = transactions.Where(t => t.Contains(productId)).ToList();
            int totalTransactionsWithProduct = transactionsContainingProduct.Count;

            // Nếu sản phẩm không được mua trong bất kì giao dịch nào, trả về danh sách rỗng
            if (totalTransactionsWithProduct == 0)
                return new List<int>();

            // Đếm số lần xuất hiện của từng sản phẩm khác trong các giao dịch chứa sản phẩm hiện tại
            var candidateCounts = new Dictionary<int, int>();
            foreach (var transaction in transactionsContainingProduct)
            {
                foreach (var pid in transaction)
                {
                    if (pid == productId) continue;
                    if (candidateCounts.ContainsKey(pid))
                        candidateCounts[pid]++;
                    else
                        candidateCounts[pid] = 1;
                }
            }
            var resultProducts = new List<int>();
            foreach (var candidate in candidateCounts)
            {
                // Tính confidence = (số giao dịch chứa cả hai sản phẩm) / (số giao dịch chứa sản phẩm hiện tại)
                double confidence = (double)candidate.Value / totalTransactionsWithProduct;
                if (candidate.Value >= minSupportCount && confidence >= minConfidence)
                {
                    resultProducts.Add(candidate.Key);
                }
            }

            return resultProducts;
        }

        //cho admin : (thêm, xoá, sửa)
        [HttpGet]
        public ActionResult Add()
        {
            ViewBag.Categories = new SelectList(db.DanhMucs, "DanhMucID", "TenDanhMuc");
            return View();
        }

        [HttpPost]
        public ActionResult Add(MT product, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                if(file != null && file.ContentLength > 0)
                {
                    string filePath = Path.Combine(Server.MapPath("~/Images"), Path.GetFileName(file.FileName));

                    file.SaveAs(filePath);

                    product.DuongDan = Path.GetFileName(file.FileName);

                    db.MTs.Add(product);
                    db.SaveChanges();
                    return RedirectToAction("test");
                }                
            }
            ViewBag.Categories = new SelectList(db.DanhMucs, "DanhMucID", "TenDanhMuc", product.DanhMucID);
            return View(product);
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            var product = db.MTs.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        //public ActionResult XacNhanDelete()
        //{
        //    return View();
        //}

        [HttpPost]
        public ActionResult XacNhanDelete(int id)
        {
            var product = db.MTs.Find(id);
            try
            {
                if (product != null)
                {
                    db.MTs.Remove(product);
                    db.SaveChanges();
                }
                return RedirectToAction("test");
            }
            catch
            {

            }
            return RedirectToAction("test");
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var product = db.MTs.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.Categories = new SelectList(db.DanhMucs, "DanhMucID", "TenDanhMuc", product.DanhMucID);
            return View(product);
        }

        [HttpPost]
        public ActionResult Edit(MT product, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                if (file != null && file.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    var path = Path.Combine(Server.MapPath("~/Images"), fileName);
                    file.SaveAs(path);
                    product.DuongDan = fileName;
                }

                db.Entry(product).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("test");
            }
            ViewBag.Categories = new SelectList(db.DanhMucs, "DanhMucID", "TenDanhMuc", product.DanhMucID);
            return View(product);
        }

        //đn, dk
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(KhachHang khachHang)
        {
            // Kiểm tra nếu model không hợp lệ
            if (ModelState.IsValid)
            {
                // Kiểm tra email có tồn tại trong cơ sở dữ liệu hay không
                var existingUser = db.KhachHangs.FirstOrDefault(k => k.Email == khachHang.Email);
                if (existingUser != null)
                {
                    ModelState.AddModelError("Email", "Email đã được dùng cho một tài khoản khác.");
                    return View(khachHang);
                }

                if (string.IsNullOrEmpty(khachHang.TenKhachHang) || string.IsNullOrEmpty(khachHang.SoDienThoai) ||
                    string.IsNullOrEmpty(khachHang.DiaChi) || string.IsNullOrEmpty(khachHang.Email) || string.IsNullOrEmpty(khachHang.MatKhau))
                {
                    ModelState.AddModelError("", "Vui lòng điền đủ thông tin!");
                    return View(khachHang);
                }

                int count = db.KhachHangs.Select(m => m.KhachHangID).OrderByDescending(m => m).FirstOrDefault();
                db.KhachHangs.Add(new KhachHang
                {
                    KhachHangID = count + 1,
                    TenKhachHang = khachHang.TenKhachHang,
                    Email = khachHang.Email,
                    MatKhau = khachHang.MatKhau,
                    DiaChi = khachHang.DiaChi,
                    SoDienThoai = khachHang.SoDienThoai
                });
                db.SaveChanges();
                return RedirectToAction("Login");
            }

            return View(khachHang);
        }
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string email, string password, string captchaInput, string captchaValue, bool rememberMe = false)
        {
            // Kiểm tra CAPTCHA
            if (string.IsNullOrEmpty(captchaInput) || captchaInput != captchaValue)
            {
                ViewBag.ErrorMessage = "Vui lòng xác minh CAPTCHA chính xác.";
                return View();
            }

            // Kiểm tra tài khoản admin
            if (email == "admin@gmail.com" && password == "123")
            {
                Session["IsAdmin"] = true;
                Session["Username"] = email;

                // Thiết lập cookie xác thực cho admin
                FormsAuthentication.SetAuthCookie(email, rememberMe);
                if (rememberMe)
                {
                    var authTicket = new FormsAuthenticationTicket(
                        1, // Phiên bản
                        email, // Tên người dùng
                        DateTime.Now, // Thời gian tạo
                        DateTime.Now.AddDays(30), // Hết hạn sau 30 ngày
                        true, // Lưu lâu dài
                        "Admin" // Dữ liệu người dùng (vai trò)
                    );
                    string encryptedTicket = FormsAuthentication.Encrypt(authTicket);
                    var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket)
                    {
                        Expires = authTicket.Expiration
                    };
                    Response.Cookies.Add(authCookie);
                }

                return RedirectToAction("test", "MayTinh");
            }

            // Kiểm tra tài khoản khách hàng
            var user = db.KhachHangs.FirstOrDefault(k => k.Email == email && k.MatKhau == password);
            if (user != null)
            {
                if (TaiKhoanVoHieu.Contains(user.KhachHangID))
                {
                    ViewBag.ThongBao = "Tài khoản của bạn đã bị vô hiệu vì đã vi phạm nguyên tắc cộng đồng!!!";
                    return View();
                }

                // Lưu thông tin vào Session
                Session["KhachHangID"] = user.KhachHangID;
                Session["TenKhachHang"] = user.TenKhachHang;
                Session["SoDienThoai"] = user.SoDienThoai;
                Session["DiaChi"] = user.DiaChi;

                // Thiết lập cookie xác thực cho khách hàng
                FormsAuthentication.SetAuthCookie(user.Email, rememberMe);
                if (rememberMe)
                {
                    var authTicket = new FormsAuthenticationTicket(
                        1, // Phiên bản
                        user.Email, // Tên người dùng
                        DateTime.Now, // Thời gian tạo
                        DateTime.Now.AddDays(30), // Hết hạn sau 30 ngày
                        true, // Lưu lâu dài
                        "User" // Dữ liệu người dùng (vai trò)
                    );
                    string encryptedTicket = FormsAuthentication.Encrypt(authTicket);
                    var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket)
                    {
                        Expires = authTicket.Expiration
                    };
                    Response.Cookies.Add(authCookie);
                }

                return RedirectToAction("Index", "MayTinh");
            }
            else
            {
                ViewBag.ErrorMessage = "THÔNG TIN ĐĂNG NHẬP SAI";
                return View();
            }
        }


        private async Task<bool> VerifyRecaptcha(string gRecaptchaResponse)
        {
            var secretKey = "6LeIxAcTAAAAAGG-vFI1TnRWxMZNFuojJ4WifJWe";
            using (var client = new HttpClient())
            {
                var values = new Dictionary<string, string>
        {
            { "secret", secretKey },
            { "response", gRecaptchaResponse }
        };

                var content = new FormUrlEncodedContent(values);
                var response = await client.PostAsync("https://www.google.com/recaptcha/api/siteverify", content);
                var responseString = await response.Content.ReadAsStringAsync();

                var obj = JsonConvert.DeserializeObject<RecaptchaResponse>(responseString);

                return obj.Success;
            }
        }

        public class RecaptchaResponse
        {
            [JsonProperty("success")]
            public bool Success { get; set; }
        }



        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Login");
        }

        public ActionResult LienHe()
        {
            return View();
        }

        [HttpPost]
        public ActionResult LienHe(string noidung)
        {            
            if(Session["KhachHangID"] != null)
            {              
                int ID_KH = (Session["KhachHangID"] != null)?(int)(Session["KhachHangID"]):(-1);

                int count = db.LIENHEs.Where(x => x.KhachHangID == ID_KH).Count() + 1;

                LIENHE tam = new LIENHE { IDLienHe = count, KhachHangID = ID_KH, NoiDung = noidung };

                db.LIENHEs.Add(tam);

                db.SaveChanges();

                ViewBag.MesageLienHe = "Phản hồi của bạn đã được ghi nhận";
            }
            else
            {
                return RedirectToAction("Login", "MayTinh");
            }
           
            return View();
        }

        public ActionResult GioiThieu()
        {
            var tam = db.MTs.Take(3).ToList();

            ViewData["gioithieu"] = tam;
            return View();
        }

        public ActionResult Forgot()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Forgot(Forgot model)
        {
            if (ModelState.IsValid)
            {
                var tk = db.KhachHangs.Where(t => t.Email == model.Email).FirstOrDefault();

                if (tk == null)
                {
                    Session["message"] = "Email không tồn tại, vui lòng kiểm tra lại!";
                    return View();
                }

                try
                {
                    var message = new MailMessage("longhoang192004@gmail.com", tk.Email)
                    {
                        Subject = "Yêu cầu đặt lại mật khẩu",
                        Body = string.Format("Mật khẩu của bạn là: {0}", tk.MatKhau),
                        IsBodyHtml = true
                    };

                    using (var smtp = new SmtpClient("smtp.gmail.com", 587))
                    {
                        smtp.Credentials = new NetworkCredential("longhoang192004@gmail.com", "hdnt udxf vpqz bzym");
                        smtp.EnableSsl = true;

                        smtp.Send(message);
                    }

                    Session["message"] = "Một email đã được gửi đến bạn để hướng dẫn đặt lại mật khẩu.";
                }
                catch (SmtpException ex)
                {
                    Session["message"] = "Đã có lỗi xảy ra trong quá trình gửi email. Vui lòng thử lại sau!";

                }
            }

            return View();
        }


        public ActionResult Maps()
        {
            return View();
        }

        public ActionResult DSPhanhoi()
        {
            var lienHes = db.LIENHEs.Include("KhachHang").ToList();
            return View(lienHes);
        }

        //ds don hang
        public ActionResult DSDonHang()
        {
            var donHangs = db.DonHangs.ToList();
            return View(donHangs);
        }

        public ActionResult DSDonHangDetails(int id)
        {
            var donHang = db.DonHangs.Find(id);
            if (donHang == null)
            {
                return HttpNotFound();
            }
            return View(donHang);
        }

        public ActionResult CapNhatTrangThai(int madh, string trangthai)
        {
            var donHang = db.DonHangs.Find(madh);
            if (donHang != null && (trangthai == "Hoàn thành" || trangthai == "Bị huỷ"))
            {
                donHang.TrangThaiDonHang = trangthai;
                db.SaveChanges();
            }
            return RedirectToAction("DSDonHang"); // quay lại danh sách đơn hàng
        }


        //ds taikhoan
        public ActionResult DSTaiKhoan()
        {
            var khachhangs = db.KhachHangs.ToList();
            return View(khachhangs);
        }

        // Xem chi tiết tài khoản
        public ActionResult ChiTietTaiKhoan(int id)
        {
            var kh = db.KhachHangs.FirstOrDefault(x => x.KhachHangID == id);
            if (kh == null)
            {
                return HttpNotFound();
            }
            return View(kh);
        }

        public static List<int> TaiKhoanVoHieu = new List<int>();

        public ActionResult VoHieuTaiKhoan(int id)
        {
            if (TaiKhoanVoHieu.Contains(id))
            {
                // Nếu đã bị vô hiệu => mở khoá
                TaiKhoanVoHieu.Remove(id);
            }
            else
            {
                // Nếu đang hoạt động => vô hiệu
                TaiKhoanVoHieu.Add(id);
            }

            return RedirectToAction("DSTaiKhoan");
        }



        //chat bot

        private readonly string openAiApiKey = "YOUR_OPENAI_API_KEY";

        [HttpPost]
        public async Task<ActionResult> Ask(string message)
        {
            if (string.IsNullOrEmpty(message))
            {
                return Json(new { reply = "Bạn cần hỏi gì?" });
            }

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", openAiApiKey);

                var requestBody = new
                {
                    model = "gpt-3.5o",
                    messages = new[] {
                    new { role = "system", content = "Bạn là trợ lý giúp trả lời các câu hỏi của người dùng." },
                    new { role = "user", content = message }
                },
                    max_tokens = 150,
                    temperature = 0.7
                };

                var jsonRequest = JsonConvert.SerializeObject(requestBody);
                var content = new StringContent(jsonRequest, System.Text.Encoding.UTF8, "application/json");

                var response = await client.PostAsync("https://api.openai.com/v1/chat/completions", content);
                var jsonResponse = await response.Content.ReadAsStringAsync();

                // Parse phản hồi từ API
                dynamic result = JsonConvert.DeserializeObject(jsonResponse);
                string reply = result.choices[0].message.content;

                return Json(new { reply });
            }
        }

        public ActionResult TKDoanhThu(DateTime? tuNgay, DateTime? denNgay)
        {
            using (var db = new CuaHangMayTinhEntities9())
            {
                var query = db.DonHangs
                    .Where(d => d.TrangThaiDonHang == "Đã giao");

                if (tuNgay.HasValue)
                    query = query.Where(d => d.NgayDatHang >= tuNgay.Value);

                if (denNgay.HasValue)
                    query = query.Where(d => d.NgayDatHang <= denNgay.Value);

                var thongKe = query
                    .ToList() // thực hiện truy vấn SQL
                    .GroupBy(d => d.NgayDatHang.Value.Date)
                    .Select(g => new
                    {
                        Ngay = g.Key,
                        TongDoanhThu = g.Sum(x => x.TongTien)
                    })
                    .OrderBy(x => x.Ngay)
                    .ToList();

                ViewBag.TuNgay = tuNgay.HasValue ? tuNgay.Value.ToString("yyyy-MM-dd") : "";
                ViewBag.DenNgay = denNgay.HasValue ? denNgay.Value.ToString("yyyy-MM-dd") : "";

                return View(thongKe);
            }
        }


    }
}
