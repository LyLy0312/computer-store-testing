using QLWEB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLWEB.Controllers
{
    public class KhachHangController : Controller
    {
        //
        // GET: /KhachHang/

        private readonly CuaHangMayTinhEntities9 db = new CuaHangMayTinhEntities9();
        // GET: KhachHang
        public ActionResult ThongTin()
        {            
            if (Session["KhachHangID"] == null)
            {
                return RedirectToAction("MayTinh", "Index");
            }

            int khachHangID = Convert.ToInt32(Session["KhachHangID"]);
            var khachHang = db.KhachHangs
                              .Where(k => k.KhachHangID == khachHangID)
                              .Select(k => new KhachHangViewModel
                              {
                                  KhachHangID = k.KhachHangID,
                                  TenKhachHang = k.TenKhachHang,
                                  Email = k.Email,
                                  SoDienThoai = k.SoDienThoai,
                                  DiaChi = k.DiaChi
                              })
                              .FirstOrDefault();
            if (khachHang == null)
            {
                return HttpNotFound("Không tìm thấy thông tin khách hàng.");
            }

            return View(khachHang);
        }

        [HttpPost]
        public ActionResult ThongTin(KhachHangViewModel model, string OldPassword, string NewPassword)
        {
            if (ModelState.IsValid)
            {
                var khachHang = db.KhachHangs.Find(Session["KhachHangID"]);

                if (khachHang != null)
                {
                    // Cập nhật thông tin cá nhân
                    khachHang.TenKhachHang = model.TenKhachHang;
                    khachHang.Email = model.Email;
                    khachHang.SoDienThoai = model.SoDienThoai;
                    khachHang.DiaChi = model.DiaChi;

                    // Kiểm tra và thay đổi mật khẩu nếu có
                    if (!string.IsNullOrEmpty(OldPassword) && !string.IsNullOrEmpty(NewPassword))
                    {
                        if (khachHang.MatKhau.Trim() == OldPassword.Trim())
                        {
                            khachHang.MatKhau = NewPassword.Trim();
                        }
                        else
                        {
                            ModelState.AddModelError("oldPassword", "Mật khẩu cũ không chính xác.");
                        }
                    }
                    try
                    {
                        db.SaveChanges();
                        ViewBag.matkhau = khachHang.MatKhau;
                        ViewBag.Success = "Cập nhật thông tin thành công.";
                    }
                    catch (Exception ex)
                    {
                        ViewBag.Error = "Có lỗi khi lưu thông tin: " + ex.Message;
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Không tìm thấy khách hàng.");
                }
            }

            return View(model);
        }

        public ActionResult DanhSachDonHang()
        {
            int makh = Session["KhachHangID"] == null ? 0 : int.Parse(Session["KhachHangID"].ToString());

            var lst = db.DonHangs.Where(m => m.KhachHangID == makh).ToList();

            return View(lst);
        }

        public ActionResult XemDonHang(int madh)
        {
            var dsChiTiet = db.CTDonHangs.Where(ct => ct.DonHangID == madh).ToList();
            return View(dsChiTiet);
        }

        public ActionResult CapNhatTrangThai(int madh, string trangthai)
        {
            var donHang = db.DonHangs.Find(madh);
            if (donHang != null && (trangthai == "Hoàn thành" || trangthai == "Bị huỷ"))
            {
                donHang.TrangThaiDonHang = trangthai;
                db.SaveChanges();
            }
            return RedirectToAction("DanhSachDonHang");
        }

    }
}
