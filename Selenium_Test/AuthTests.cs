using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Linq;
using System.Threading;
using Assert = NUnit.Framework.Assert;

namespace QLWebTests
{
    [TestFixture]
    public class LoginTests
    {
        private IWebDriver driver;
        private const string BaseUrl = "http://localhost:54551";

        [SetUp]
        public void Setup()
        {
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
        }

        [Test]
        public void Login_WithValidCredentials_ShouldSucceed()
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

            var logoutLink = driver.FindElements(By.Id("dangxuat"));
            Assert.IsTrue(logoutLink.Count > 0, "Không thấy nút Đăng xuất => đăng nhập không thành công.");
        }

        [Test]
        public void Register_WithValidData_ShouldSucceed()
        {
            driver.Navigate().GoToUrl($"{BaseUrl}/MayTinh/Register");

            driver.FindElement(By.Name("TenKhachHang")).SendKeys("Nguyen Van Test");
            driver.FindElement(By.Name("SoDienThoai")).SendKeys("0912345678");
            driver.FindElement(By.Name("DiaChi")).SendKeys("123 ABC Street");
            driver.FindElement(By.Name("Email")).SendKeys($"test{Guid.NewGuid().ToString().Substring(0, 5)}@gmail.com");
            driver.FindElement(By.Name("MatKhau")).SendKeys("1");

            driver.FindElement(By.CssSelector("button[type='submit']")).Click();

            Thread.Sleep(2000);

            Assert.IsFalse(driver.PageSource.Contains("Email đã tồn tại") || driver.PageSource.Contains("lỗi"),
                "Có thông báo lỗi hoặc email trùng lặp => đăng ký thất bại.");
        }


        [Test]
        public void ForgotPassword_WithValidEmail_ShouldShowPopup()
        {
            driver.Navigate().GoToUrl($"{BaseUrl}/MayTinh/Forgot");

            driver.FindElement(By.Name("Email")).SendKeys("nguyenvana@gmail.com");
            driver.FindElement(By.CssSelector("button[type='submit']")).Click();

            Thread.Sleep(2000);

            var hasPopup = driver.FindElements(By.ClassName("popup")).Count > 0;
            var hasSuccessText = driver.PageSource.Contains("thành công") || driver.PageSource.Contains("gửi thành công");

            Assert.IsTrue(hasPopup || hasSuccessText, "Không thấy popup hoặc thông báo thành công.");
        }

        [Test]
        public void ForgotPassword_WithEmptyEmail_ShouldShowValidation()
        {
            driver.Navigate().GoToUrl($"{BaseUrl}/MayTinh/Forgot");

            driver.FindElement(By.CssSelector("button[type='submit']")).Click();
            Thread.Sleep(1000);

            var isStillOnPage = driver.Url.Contains("/Forgot");
            var emailFieldStillExists = driver.FindElements(By.Name("Email")).Count > 0;

            Console.WriteLine("Vẫn còn ở trang Forgot: " + isStillOnPage);
            Console.WriteLine("Field Email vẫn có mặt: " + emailFieldStillExists);

            Assert.IsTrue(isStillOnPage && emailFieldStillExists,
                "Không ở lại trang Forgot hoặc mất field Email sau khi submit trống.");
        }


        [TearDown]
        public void Teardown()
        {
            driver.Quit();
        }
    }
}
