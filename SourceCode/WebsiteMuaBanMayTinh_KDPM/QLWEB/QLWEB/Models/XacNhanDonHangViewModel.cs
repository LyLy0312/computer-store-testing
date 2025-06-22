using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLWEB.Models
{
    public class XacNhanDonHangViewModel
    {
        private KhachHang _KhachHang;

        public KhachHang KhachHang
        {
            get { return _KhachHang; }
            set { _KhachHang = value; }
        }
        private DonHang _DonHang;

        public DonHang DonHang
        {
            get { return _DonHang; }
            set { _DonHang = value; }
        }
        private List<CTDonHang> _ChiTietDonHang;

        public List<CTDonHang> ChiTietDonHang
        {
            get { return _ChiTietDonHang; }
            set { _ChiTietDonHang = value; }
        }
    }
}