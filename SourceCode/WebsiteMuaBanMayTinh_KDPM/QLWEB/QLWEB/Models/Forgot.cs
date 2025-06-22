using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace QLWEB.Models
{
    public class Forgot
    {
        [Required(ErrorMessage = "Email là bắt buộc.")]
        [EmailAddress(ErrorMessage = "Địa chỉ email không hợp lệ.")]
        public string _Email;

        public string Email
        {
            get { return _Email; }
            set { _Email = value; }
        }
    }
}