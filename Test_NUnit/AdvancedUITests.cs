using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Threading;

namespace QLWebTests
{
    [TestFixture]
    public class AdvancedUITests
    {
        private IWebDriver driver;
        private const string BaseUrl = "http://localhost:54551";

        [SetUp] public void Setup() => driver = new ChromeDriver();

        [TearDown] public void TearDown() => driver.Quit();

        [Test]
        public void RatingComponent_ShouldRenderStars()
        {
            driver.Navigate().GoToUrl(BaseUrl + "/MayTinh/Details/1");
            Thread.Sleep(500);
            Assert.IsTrue(driver.PageSource.Contains("fa-star") || driver.PageSource.Contains("Đánh giá"));
        }

        [Test]
        public void GiayBaoMat_VisibleOnFooter()
        {
            driver.Navigate().GoToUrl(BaseUrl + "/MayTinh/Index");
            Thread.Sleep(500);
            Assert.IsTrue(driver.PageSource.Contains("Chính sách bảo vệ thông tin"));
        }

        [Test]
        public void QuantitySelector_IncreaseDecrease_ShouldChangeValue()
        {
            driver.Navigate().GoToUrl(BaseUrl + "/GioHang/GioHang");
            var quantity = driver.FindElement(By.Name("SoLuong"));
            string initial = quantity.GetAttribute("value");
            quantity.Clear();
            quantity.SendKeys("3");
            Thread.Sleep(500);
            Assert.AreEqual("3", quantity.GetAttribute("value"));
        }

        [Test]
        public void AfterLogin_ShouldShowUsername()
        {
            Login();
            Assert.IsTrue(driver.PageSource.Contains("Nguyen Van A") || driver.FindElement(By.ClassName("nav-link")).Text.Contains("Nguyen"));
        }

 
        [Test]
        public void Search_WithLongKeyword_ShouldHandleGracefully()
        {
            driver.Navigate().GoToUrl(BaseUrl + "/MayTinh/Index");
            driver.FindElement(By.Name("txtSearch")).SendKeys(new string('a', 300));
            driver.FindElement(By.CssSelector("button[type='submit']")).Click();
            Thread.Sleep(500);
            Assert.IsTrue(driver.PageSource.Length > 0);
        }

        [Test]
        public void SocialMediaIcon_Zalo_ShouldOpenNewTab()
        {
            driver.Navigate().GoToUrl(BaseUrl + "/MayTinh/Index");
            var zalo = driver.FindElement(By.CssSelector("a[href*='zalo']"));
            string target = zalo.GetAttribute("target");
            Assert.AreEqual("_blank", target);
        }

        [Test]
        public void Captcha_InvalidCode_ShouldBlockLogin()
        {
            driver.Navigate().GoToUrl(BaseUrl + "/MayTinh/Login");
            driver.FindElement(By.Id("email")).SendKeys("nguyenvana@gmail.com");
            driver.FindElement(By.Name("password")).SendKeys("1");
            driver.FindElement(By.Id("captchaCheckbox")).Click();
            Thread.Sleep(500);
            driver.FindElement(By.Name("captchaInput")).SendKeys("sai");
            driver.FindElement(By.CssSelector("button[type='submit']")).Click();
            Thread.Sleep(1000);
            Assert.IsTrue(driver.PageSource.Contains("mã captcha không đúng") || driver.Url.Contains("Login"));
        }

        [Test]
        public void Navbar_Has_DangXuat_Test()
        {
            LoginAsCustomer();
            driver.Navigate().GoToUrl(BaseUrl + "/MayTinh/Index");
            Thread.Sleep(700);
            Assert.IsTrue(driver.PageSource.Contains("Đăng xuất"), "Không tìm thấy nút Đăng xuất trên navbar.");
        }

        [Test]
        public void SupportEmail_Visible_Test()
        {
            driver.Navigate().GoToUrl(BaseUrl + "/MayTinh/LienHe");
            Thread.Sleep(700);
            Assert.IsTrue(driver.PageSource.Contains("support@lapworld.com"), "Không thấy email hỗ trợ trên trang liên hệ.");
        }

        [Test]
        public void Navbar_Has_TrangChu_Link_Test()
        {
            driver.Navigate().GoToUrl(BaseUrl);
            Thread.Sleep(700);
            var link = driver.FindElement(By.LinkText("Trang Chủ"));
            Assert.IsTrue(link.Displayed, "Không thấy link Trang Chủ trên navbar.");
        }


        [Test]
        public void CategoryDropdown_ShouldContainLaptop()
        {
            driver.Navigate().GoToUrl(BaseUrl + "/MayTinh/Index");
            Thread.Sleep(700);
            var dropdown = driver.FindElement(By.Name("DanhMucID"));
            Assert.IsTrue(dropdown.Text.Contains("Laptop"), "Dropdown danh mục không chứa Laptop.");
        }

        [Test]
        public void Newsletter_Input_ShouldBePresent()
        {
            driver.Navigate().GoToUrl(BaseUrl + "/MayTinh/Index");
            Thread.Sleep(700);
            Assert.IsTrue(driver.PageSource.Contains("Email address"), "Không tìm thấy ô đăng ký nhận bản tin.");
        }

        [Test]
        public void RegisterPage_ShouldHavePhoneInput()
        {
            driver.Navigate().GoToUrl(BaseUrl + "/MayTinh/Register");
            Thread.Sleep(700);
            Assert.IsTrue(driver.PageSource.Contains("SoDienThoai"), "Không tìm thấy ô nhập số điện thoại trên form đăng ký.");
        }

        [Test]
        public void DetailPage_ShouldHaveRelatedProductSection()
        {
            driver.Navigate().GoToUrl(BaseUrl + "/MayTinh/Details/1");
            Thread.Sleep(700);
            Assert.IsTrue(driver.PageSource.Contains("Sản phẩm liên quan"), "Không tìm thấy khu vực Sản phẩm liên quan.");
        }

        [Test]
        public void GioiThieuPage_ShouldHaveCompanyMission()
        {
            driver.Navigate().GoToUrl(BaseUrl + "/MayTinh/GioiThieu");
            Thread.Sleep(700);
            Assert.IsTrue(driver.PageSource.ToLower().Contains("sứ mệnh"), "Không tìm thấy nội dung sứ mệnh công ty.");
        }

        [Test]
        public void LienHePage_ShouldHaveMapIframe()
        {
            driver.Navigate().GoToUrl(BaseUrl + "/MayTinh/LienHe");
            Thread.Sleep(700);
            Assert.IsTrue(driver.PageSource.Contains("iframe"), "Không tìm thấy iframe bản đồ Google Map trên trang liên hệ.");
        }

        [Test]
        public void CartPage_Empty_ShouldShowMessage()
        {
            driver.Navigate().GoToUrl(BaseUrl + "/GioHang/GioHang");
            Thread.Sleep(700);
            Assert.IsTrue(driver.PageSource.Contains("giỏ hàng trống") || driver.PageSource.Contains("chưa có sản phẩm"), "Không thấy thông báo giỏ hàng trống.");
        }

        [Test]
        public void LoginForm_ShouldContainCaptchaImage()
        {
            driver.Navigate().GoToUrl(BaseUrl + "/MayTinh/Login");
            Thread.Sleep(700);
            Assert.IsTrue(driver.FindElement(By.Id("captchaCode")).Displayed, "Không hiển thị captcha hình ảnh trong form đăng nhập.");
        }

        [Test]
        public void DangKyPage_ShouldHavePasswordPolicyNote()
        {
            driver.Navigate().GoToUrl(BaseUrl + "/MayTinh/Register");
            Thread.Sleep(700);
            Assert.IsTrue(driver.PageSource.ToLower().Contains("mật khẩu phải") || driver.PageSource.ToLower().Contains("password must"), "Không thấy hướng dẫn độ mạnh mật khẩu.");
        }

        [Test]
        public void Index_ShouldHaveCarouselWith3Slides()
        {
            driver.Navigate().GoToUrl(BaseUrl + "/MayTinh/Index");
            Thread.Sleep(700);
            var slides = driver.FindElements(By.CssSelector(".carousel-item"));
            Assert.AreEqual(3, slides.Count, "Số lượng slide trong carousel không đúng.");
        }
        [Test]
        public void IndexPage_ShouldContainLiveChatWidget()
        {
            driver.Navigate().GoToUrl(BaseUrl + "/MayTinh/Index");
            Thread.Sleep(700);
            Assert.IsTrue(driver.PageSource.ToLower().Contains("chatbot") || driver.PageSource.Contains("chatbot-container"), "Không thấy chatbot/live chat widget hiển thị.");
        }

        private void LoginAsCustomer()
        {
            driver.Navigate().GoToUrl(BaseUrl + "/MayTinh/Login");
            driver.FindElement(By.Id("email")).SendKeys("nguyenvana@gmail.com");
            driver.FindElement(By.Name("password")).SendKeys("1");
            driver.FindElement(By.Id("captchaCheckbox")).Click();
            Thread.Sleep(500);
            driver.FindElement(By.Name("captchaInput")).SendKeys(driver.FindElement(By.Id("captchaCode")).Text);
            driver.FindElement(By.CssSelector("button[type='submit']")).Click();
            Thread.Sleep(1000);
        }

        private void Login()
        {
            driver.Navigate().GoToUrl(BaseUrl + "/MayTinh/Login");
            driver.FindElement(By.Id("email")).SendKeys("nguyenvana@gmail.com");
            driver.FindElement(By.Name("password")).SendKeys("1");
            driver.FindElement(By.Id("captchaCheckbox")).Click();
            Thread.Sleep(500);
            driver.FindElement(By.Name("captchaInput")).SendKeys(driver.FindElement(By.Id("captchaCode")).Text);
            driver.FindElement(By.CssSelector("button[type='submit']")).Click();
            Thread.Sleep(1000);
        }

        private void LoginAsAdmin()
        {
            driver.Navigate().GoToUrl(BaseUrl + "/MayTinh/Login");
            driver.FindElement(By.Id("email")).SendKeys("admin@gmail.com");
            driver.FindElement(By.Name("password")).SendKeys("1");
            driver.FindElement(By.Id("captchaCheckbox")).Click();
            Thread.Sleep(500);
            driver.FindElement(By.Name("captchaInput")).SendKeys(driver.FindElement(By.Id("captchaCode")).Text);
            driver.FindElement(By.CssSelector("button[type='submit']")).Click();
            Thread.Sleep(1000);
        }

    }
}
