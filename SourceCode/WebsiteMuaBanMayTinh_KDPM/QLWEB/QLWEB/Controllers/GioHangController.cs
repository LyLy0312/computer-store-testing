using QLWEB.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Permissions;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Diagnostics;


namespace QLWEB.Controllers
{
    public class GioHangController : Controller
    {
        //
        // GET: /GioHang/
        private readonly CuaHangMayTinhEntities9 db = new CuaHangMayTinhEntities9();

        // Lấy giỏ hàng
        public List<GioHang> LayGioHang()
        {
            List<GioHang> lstGioHang = Session["GioHang"] as List<GioHang>;
            if (lstGioHang == null)
            {
                lstGioHang = new List<GioHang>();
                Session["GioHang"] = lstGioHang;
            }
            return lstGioHang;
        }

        // Tính tổng số lượng
        //private int TongSoLuong()
        //{
        //    List<GioHang> lstGioHang = LayGioHang();
        //    return lstGioHang.Sum(sp => sp.iSoLuong);
        //}

        // Tính tổng thành tiền
        private double TongThanhTien()
        {
            List<GioHang> lstGioHang = LayGioHang();
            return lstGioHang.Where(sp => sp.bXacNhan == true).Sum(sp => sp.ThanhTien);
        }

        // Trang giỏ hàng
        public ActionResult GioHang()
        {
            List<GioHang> lstGioHang = LayGioHang();
            if (lstGioHang.Count == 0)
            {
                return RedirectToAction("Index", "MayTinh");
            }

            //ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongThanhTien = TongThanhTien();
            return View();
        }

        // Giỏ hàng Partial View
        //public ActionResult GioHangPartial()
        //{
        //    ViewBag.TongSoLuong = TongSoLuong();
        //    ViewBag.TongThanhTien = TongThanhTien();
        //    return PartialView();
        //}

        // Thêm sản phẩm vào giỏ hàng
        
        public ActionResult ThemGioHang(int MTID, string returnUrl)
        {
            List<GioHang> lstGioHang = LayGioHang();
            GioHang sp = lstGioHang.FirstOrDefault(s => s.iMaMT == MTID);

            if (sp == null)
            {

                sp = new GioHang(MTID);
                lstGioHang.Add(sp);
                Session["soluongsanpham"] = lstGioHang.Count();

            }
            else
            {
                sp.iSoLuong++;
            }
            TempData["ThongBao"] = "Sản phẩm đã được thêm vào giỏ hàng!";
            return Redirect(returnUrl);
        }

        // Cập nhật số lượng sản phẩm trong giỏ hàng
        //public ActionResult CapNhatGioHang(int MTID, int SoLuong)
        //{
        //    if (MTID <= 0 || SoLuong <= 0) return RedirectToAction("GioHang");

        //    List<GioHang> lstGioHang = LayGioHang();
        //    var sp = lstGioHang.SingleOrDefault(s => s.iMaMT == MTID);

        //    if (sp != null)
        //    {
        //        sp.iSoLuong = SoLuong;
        //    }
        //    return RedirectToAction("GioHang");
        //}

        // Xóa sản phẩm khỏi giỏ hàng
        public ActionResult XoaGioHang(int MTID)
        {
            List<GioHang> lstGioHang = LayGioHang();
            GioHang sp = lstGioHang.SingleOrDefault(s => s.iMaMT == MTID);

            if (sp != null)
            {
                lstGioHang.Remove(sp);
                Session["soluongsanpham"] = lstGioHang.Count();
            }

            return RedirectToAction("GioHang");
        }

        //// Đặt hàng
        //[HttpPost]
        //public ActionResult DatHang(DonHang_ model)
        //{
        //    List<GioHang> lstGioHang = LayGioHang();

        //    if (model.DiaChi != null && model.PhuongThucThanhToan != null)
        //    {
        //        if (Session["KhachHangID"] != null)
        //        {
        //            int makh = int.Parse(Session["KhachHangID"].ToString());

        //            // Tạo mã đơn hàng mới
        //            int newDonHangID = db.DonHangs.Any()
        //                ? db.DonHangs.Max(m => m.DonHangID) + 1
        //                : 1;

        //            DonHang donHang = new DonHang
        //            {
        //                DonHangID = newDonHangID,
        //                KhachHangID = makh,
        //                NgayDatHang = DateTime.Now,
        //                DiaChiGiaoHang = model.DiaChi,
        //                PhuongThucThanhToan = model.LayPTTT(),
        //                TrangThaiDonHang = "Đang giao",
        //                TongTien = (decimal)TongThanhTien()
        //            };
        //            db.DonHangs.Add(donHang);
        //            db.SaveChanges();

        //            // Thêm chi tiết đơn hàng
        //            for (int i = lstGioHang.Count - 1; i >= 0; i--)
        //            {
        //                if (lstGioHang[i].bXacNhan == true)
        //                {
        //                    CTDonHang ctDonHang = new CTDonHang
        //                    {
        //                        DonHangID = donHang.DonHangID,
        //                        MTID = lstGioHang[i].iMaMT,
        //                        SoLuong = lstGioHang[i].iSoLuong,
        //                        Gia = (decimal)lstGioHang[i].dDonGia
        //                    };
        //                    db.CTDonHangs.Add(ctDonHang);
        //                    db.SaveChanges();
        //                    lstGioHang.RemoveAt(i);
        //                }
        //            }

        //            // Cập nhật kho
        //            db.CapNhatKhoDuaTrenDonHang(donHang.DonHangID);

        //            // Cập nhật lại Session giỏ hàng
        //            Session["GioHang"] = lstGioHang;
        //            Session["soluongsanpham"] = lstGioHang.Count;

        //            ViewBag.thongbao = "Đặt hàng thành công";
        //        }
        //        else
        //        {
        //            ViewBag.thongbao = "Bạn cần đăng nhập để đặt hàng.";
        //        }
        //    }
        //    else
        //    {
        //        ViewBag.thongbao = "Vui lòng nhập đầy đủ địa chỉ và phương thức thanh toán.";
        //    }

        //    return View(model);
        //}


        // Đặt hàng fix
        [HttpPost]
        public ActionResult DatHang(DonHang_ model)
        {
            List<GioHang> lstGioHang = LayGioHang();
            Debug.WriteLine("Giỏ hàng: " + lstGioHang.Count); // Kiểm tra số lượng sản phẩm trong giỏ hàng

            if (model.DiaChi != null && model.PhuongThucThanhToan != null)
            {
                if (Session["KhachHangID"] != null)
                {
                    int makh = int.Parse(Session["KhachHangID"].ToString());
                    Debug.WriteLine("Khách hàng ID: " + makh);

                    // Kiểm tra địa chỉ và phương thức thanh toán
                    Debug.WriteLine("Địa chỉ giao hàng: " + model.DiaChi);
                    Debug.WriteLine("Phương thức thanh toán: " + model.LayPTTT());

                    // Tạo mới ID đơn hàng
                    int newDonHangID = db.DonHangs.Any() ? db.DonHangs.Max(m => m.DonHangID) + 1 : 1;
                    Debug.WriteLine("Mã đơn hàng mới: " + newDonHangID);

                    DonHang donHang = new DonHang
                    {
                        DonHangID = newDonHangID,
                        KhachHangID = makh,
                        NgayDatHang = DateTime.Now,
                        DiaChiGiaoHang = model.DiaChi,
                        PhuongThucThanhToan = model.LayPTTT(),
                        TrangThaiDonHang = "Đang giao",
                        TongTien = (decimal)TongThanhTien()
                    };

                    db.DonHangs.Add(donHang);
                    db.SaveChanges();
                    Debug.WriteLine("Đơn hàng đã được lưu");

                    // Xử lý chi tiết đơn hàng
                    foreach (var item in lstGioHang.Where(sp => sp.bXacNhan == true).ToList())
                    {
                        CTDonHang ctDonHang = new CTDonHang
                        {
                            DonHangID = donHang.DonHangID,
                            MTID = item.iMaMT,
                            SoLuong = item.iSoLuong,
                            Gia = (decimal)item.dDonGia
                        };
                        db.CTDonHangs.Add(ctDonHang);
                        db.SaveChanges();
                        lstGioHang.Remove(item);
                        Debug.WriteLine("Chi tiết đơn hàng đã được thêm: " + item.iMaMT);
                    }

                    // Cập nhật kho dựa trên đơn hàng
                    db.CapNhatKhoDuaTrenDonHang(donHang.DonHangID);
                    Debug.WriteLine("Kho đã được cập nhật");

                    // Cập nhật giỏ hàng trong session
                    Session["GioHang"] = lstGioHang;
                    Session["soluongsanpham"] = lstGioHang.Count;

                    TempData["thongbao"] = "Đặt hàng thành công";
                    return RedirectToAction("DatHangThanhCong");
                }
                else
                {
                    ViewBag.thongbao = "Bạn cần đăng nhập để đặt hàng.";
                }
            }
            else
            {
                ViewBag.thongbao = "Vui lòng nhập đầy đủ địa chỉ và phương thức thanh toán.";
            }

            return RedirectToAction("DatHangThanhCong");
        }



        public ActionResult DatHangThanhCong()
        {
            return View();
        }


        // Trang xác nhận đơn hàng
        public ActionResult XacNhanDonHang()
        {
            if (Session["KhachHangID"] == null)
            {
                return RedirectToAction("DangNhap", "Auth");
            }

            int makh = int.Parse(Session["KhachHangID"].ToString());
            var khachHang = db.KhachHangs.SingleOrDefault(k => k.KhachHangID == makh);
            if (khachHang == null)
            {
                return HttpNotFound("Không tìm thấy thông tin khách hàng.");
            }
            var donHang = db.DonHangs.Where(dh => dh.KhachHangID == makh)
                                     .OrderByDescending(dh => dh.NgayDatHang)
                                     .FirstOrDefault();
            if (donHang == null)
            {
                return HttpNotFound("Không tìm thấy đơn hàng.");
            }

            var chiTietDonHang = db.CTDonHangs.Where(ct => ct.DonHangID == donHang.DonHangID).ToList();
            var xacNhanDonHangViewModel = new XacNhanDonHangViewModel
            {
                KhachHang = khachHang,
                DonHang = donHang,
                ChiTietDonHang = chiTietDonHang
            };

            return View(xacNhanDonHangViewModel);
        }

        public ActionResult CongSP(int id)
        {
            List<GioHang> lstGioHang = LayGioHang();
            if(lstGioHang != null)
            {
                foreach(var item in lstGioHang)
                {
                    if(item.iMaMT == id)
                    {
                        item.iSoLuong++;
                    }
                }
            }
            return RedirectToAction("GioHang");
        }

        public ActionResult TruSP(int id)
        {
            List<GioHang> lstGioHang = LayGioHang();
            if (lstGioHang != null)
            {
                foreach (var item in lstGioHang)
                {
                    if (item.iMaMT == id && item.iSoLuong > 1)
                    {
                        item.iSoLuong--;
                    }
                }
            }
            return RedirectToAction("GioHang");
        }

        public ActionResult ChonSanPham(int id)
        {
            List<GioHang> lst = LayGioHang();
            foreach(var item in lst)
            {
                if(item.iMaMT == id)
                {
                    item.bXacNhan = item.bXacNhan == false ? true : false;
                }
            }

            ViewBag.TongThanhTien = TongThanhTien();

            return RedirectToAction("GioHang");
        }

    }
}
