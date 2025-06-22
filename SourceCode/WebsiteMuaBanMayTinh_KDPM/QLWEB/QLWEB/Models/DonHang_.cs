using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace QLWEB.Models
{

    public class DonHang_
    {
        private int _MaDH;

        public int MaDH
        {
            get { return _MaDH; }
            set { _MaDH = value; }
        }

        private int _IDKhachHang;

        public int IDKhachHang
        {
            get { return _IDKhachHang; }
            set { _IDKhachHang = value; }
        }
        private string _NgayDat = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

        public string NgayDat
        {
            get { return _NgayDat; }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    _NgayDat = value;
                }
                else
                {
                    throw new ArgumentException("Ngày đặt không hợp lệ.");
                }
            }
        }

        private decimal _TongTien;

        public decimal TongTien
        {
            get { return _TongTien; }
            set { _TongTien = value; }
        }

        private string _TrangThai = "Pending";
        public string TrangThai
        {
            get { return _TrangThai; }
            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                    _TrangThai = value;
                else
                    throw new ArgumentException("Trạng thái không được để trống.");
            }
        }

        private int _PhuongThucThanhToan;

        public int PhuongThucThanhToan
        {
            get { return _PhuongThucThanhToan; }
            set { _PhuongThucThanhToan = value; }
        }

        [Required(ErrorMessage = "Không được bỏ trống")]
        [StringLength(150, MinimumLength = 20, ErrorMessage = "Vui lòng nhập đúng địa chỉ")]
        public string DiaChi { get; set; }

        public string LayPTTT()
        {
            if (PhuongThucThanhToan == 1) return "Thanh toán khi nhận hàng";

            return "Chưa làm các phương thức thanh toán còn lại";
        }
    }

}