using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLWEB.Models
{
    public class GioHang
    {
        CuaHangMayTinhEntities9 db = new CuaHangMayTinhEntities9();
        private int _iMaMT;
        private string _sTenMT;
        private string _sHinhAnh;
        private double _dDonGia;
        private int _iSoLuong = 1;
        private bool _bXacNhan = false;

        public int iMaMT
        {
            get { return _iMaMT; }
            set
            {
                if (value <= 0)
                    throw new ArgumentException("Mã sản phẩm phải là số dương.");
                _iMaMT = value;
            }
        }

        public string sTenMT
        {
            get { return _sTenMT; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Tên sản phẩm không được để trống.");
                _sTenMT = value;
            }
        }

        public string sHinhAnh
        {
            get { return _sHinhAnh; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Hình ảnh sản phẩm không được để trống.");
                _sHinhAnh = value;
            }
        }

        public double dDonGia
        {
            get { return _dDonGia; }
            set
            {
                if (value < 0)
                    throw new ArgumentException("Đơn giá phải là số không âm.");
                _dDonGia = value;
            }
        }

        public int iSoLuong
        {
            get { return _iSoLuong; }
            set
            {
                if (value <= 0)
                    throw new ArgumentException("Số lượng phải lớn hơn 0.");
                _iSoLuong = value;
            }
        }

        public bool bXacNhan
        {
            get { return _bXacNhan; }
            set { _bXacNhan = value; }
        }

        public double ThanhTien
        {
            get { return iSoLuong * dDonGia; }
        }

        public GioHang() { }

        // Constructor để lấy thông tin sản phẩm từ database
        public GioHang(int MTID)
        {
            var sanPham = db.MTs.Single(t => t.MTID == MTID);
            iMaMT = sanPham.MTID;
            sTenMT = sanPham.TenSP;
            sHinhAnh = "~/Images/" + sanPham.DuongDan;
            dDonGia = (double)(sanPham.Gia ?? 0);
            //iSoLuong = 1;
        }
    }
    public static class LayDuLieu
    {
        public static CuaHangMayTinhEntities9 db = new CuaHangMayTinhEntities9();
        public static string LayTenSanPham(int id)
        {
            var tensp = db.MTs.Where(t => t.MTID == id).Select(t => t.TenSP).FirstOrDefault();

            return tensp;
        }


        public static decimal LayTongTien(int iddonhang)
        {
            var tongtien = db.DonHangs.Where(t => t.DonHangID == iddonhang).Select(t => t.TongTien).FirstOrDefault();

            return tongtien ?? 0;
        }
    }
}