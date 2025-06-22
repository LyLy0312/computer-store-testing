using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Threading;

namespace QLWebTests
{
    [TestFixture]
    public class PublicPageTests
    {
        private IWebDriver driver;
        private const string BaseUrl = "http://localhost:54551";

        [SetUp]
        public void Setup()
        {
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
        }

        private void AssertUrlLoads(string relativeUrl)
        {
            driver.Navigate().GoToUrl($"{BaseUrl}{relativeUrl}");
            Thread.Sleep(1000);
            Assert.IsFalse(string.IsNullOrEmpty(driver.Title) && driver.PageSource.Length < 100, $"Trang {relativeUrl} không tải đúng.");
        }

        private void AssertTextPresent(string text)
        {
            Assert.IsTrue(driver.PageSource.Contains(text), $"Không tìm thấy từ khóa '{text}' trong trang.");
        }

        [Test]
        public void HomePage_ShouldLoad() => AssertUrlLoads("/MayTinh/Index");

        [Test]
        public void ProductDetail_ShouldLoad() => AssertUrlLoads("/MayTinh/Details/1");

        [Test]
        public void ContactPage_ShouldLoad() => AssertUrlLoads("/MayTinh/LienHe");

        [Test]
        public void AboutPage_ShouldLoad() => AssertUrlLoads("/MayTinh/GioiThieu");

        [Test]
        public void MapPage_ShouldLoad() => AssertUrlLoads("/MayTinh/Maps");

        [Test]
        public void Cart_WhenEmpty_ShouldRedirect() => AssertUrlLoads("/GioHang/GioHang");

        [Test]
        public void RegisterForm_ShouldShowInputs()
        {
            driver.Navigate().GoToUrl("http://localhost:54551/MayTinh/Register");
            AssertTextPresent("TenKhachHang");
        }

        [Test]
        public void ForgotForm_ShouldHaveEmail()
        {
            driver.Navigate().GoToUrl("http://localhost:54551/MayTinh/Forgot");
            AssertTextPresent("Email");
        }

        [Test]
        public void LoginPage_ShouldShowButton()
        {
            driver.Navigate().GoToUrl("http://localhost:54551/MayTinh/Login");
            AssertTextPresent("Đăng nhập");
        }

        [TearDown]
        public void Teardown()
        {
            driver.Quit();
        }
    }
}
