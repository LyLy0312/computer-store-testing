using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLWEB.Models
{
    public class KhachHangViewModel
    {
        private int _KhachHangID;

        public int KhachHangID
        {
            get { return _KhachHangID; }
            set { _KhachHangID = value; }
        }
        private string _TenKhachHang;

        public string TenKhachHang
        {
            get { return _TenKhachHang; }
            set { _TenKhachHang = value; }
        }
        private string _SoDienThoai;

        public string SoDienThoai
        {
            get { return _SoDienThoai; }
            set { _SoDienThoai = value; }
        }
        private string _Email;

        public string Email
        {
            get { return _Email; }
            set { _Email = value; }
        }
        private string _DiaChi;

        public string DiaChi
        {
            get { return _DiaChi; }
            set { _DiaChi = value; }
        }
    }
}