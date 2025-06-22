using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Assert = NUnit.Framework.Assert;
using System.Threading;

namespace QLWebTests
{
    [TestFixture]
    public class UserTests
    {
        private IWebDriver driver;
        private const string BaseUrl = "http://localhost:54551";

        [SetUp]
        public void Setup()
        {
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
        }

        private void LoginAsCustomer()
        {
            driver.Navigate().GoToUrl($"{BaseUrl}/MayTinh/Login");

            driver.FindElement(By.Id("email")).SendKeys("nguyenvana@gmail.com");
            driver.FindElement(By.Name("password")).SendKeys("1");

            var captchaCheckbox = driver.FindElement(By.Id("captchaCheckbox"));
            if (!captchaCheckbox.Selected)
                captchaCheckbox.Click();

            Thread.Sleep(1000);
            var captchaCode = driver.FindElement(By.Id("captchaCode")).Text;
            driver.FindElement(By.Name("captchaInput")).SendKeys(captchaCode);
            driver.FindElement(By.CssSelector("button[type='submit']")).Click();
        }

        [Test]
        public void Test_ViewProfile()
        {
            LoginAsCustomer();
            driver.Navigate().GoToUrl($"{BaseUrl}/KhachHang/ThongTin");

            Assert.IsTrue(driver.PageSource.Contains("Thông Tin Tài Khoản"));
            Assert.IsTrue(driver.PageSource.Contains("Email"));
        }

        [Test]
        public void Test_ViewOrderList()
        {
            LoginAsCustomer();
            driver.Navigate().GoToUrl($"{BaseUrl}/KhachHang/DanhSachDonHang");

            Assert.IsTrue(driver.PageSource.Contains("Đơn hàng của bạn"));
            Assert.IsTrue(driver.PageSource.Contains("Hành động"));
        }

        [Test]
        public void Test_ViewOrderDetail()
        {
            LoginAsCustomer();
            driver.Navigate().GoToUrl($"{BaseUrl}/KhachHang/DanhSachDonHang");

            Thread.Sleep(1000);

            // Click vào link "Xem" đầu tiên
            var viewLinks = driver.FindElements(By.XPath("//a[contains(@href, 'XemDonHang')]"));
            Assert.IsTrue(viewLinks.Count > 0, "Không tìm thấy link 'Xem đơn hàng'");

            viewLinks[0].Click();
            Thread.Sleep(1000);

            Assert.IsTrue(driver.PageSource.Contains("Chi Tiết Hóa Đơn") || driver.PageSource.Contains("Tổng Tiền"));
        }

        [TearDown]
        public void Teardown()
        {
            driver.Quit();
        }
    }
}
