using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Threading;

namespace QLWebTests
{
    [TestFixture]
    public class UserGUITests_All
    {
        private IWebDriver driver;
        private const string BaseUrl = "http://localhost:54551";

        [SetUp] public void Setup() => driver = new ChromeDriver();

        [Test]
        public void Banner_Contains_SALE_Test()
        {
            driver.Navigate().GoToUrl(BaseUrl + "/MayTinh/Index");
            Thread.Sleep(700);
            var bannerText = driver.FindElement(By.XPath("//*[contains(text(),'SALE')]"));
            Assert.IsTrue(bannerText.Displayed, "Không tìm thấy chữ SALE trong banner.");
        }

        [Test]
        public void CustomerName_ShownInProfile_Test()
        {
            LoginAsCustomer();
            driver.Navigate().GoToUrl(BaseUrl + "/KhachHang/ThongTin");
            Thread.Sleep(700);
            var nameInput = driver.FindElement(By.Name("HoTen"));
            Assert.IsTrue(nameInput.GetAttribute("value").Contains("Nguyen Van B"));
        }

        [Test]
        public void Has_TogglePasswordCheckbox_Test()
        {
            LoginAsCustomer();
            driver.Navigate().GoToUrl(BaseUrl + "/KhachHang/ThongTin");
            Thread.Sleep(700);
            var checkboxLabel = driver.FindElement(By.XPath("//*[contains(text(),'Thay đổi mật khẩu')]"));
            Assert.IsTrue(checkboxLabel.Displayed);
        }


        [Test]
        public void Navbar_Has_DangXuat_Test()
        {
            LoginAsCustomer();
            driver.Navigate().GoToUrl(BaseUrl + "/MayTinh/Index");
            Thread.Sleep(700);
            Assert.IsTrue(driver.PageSource.Contains("Đăng xuất"));
        }

        [Test]
        public void ProfileInfo_HasEmail_Test()
        {
            LoginAsCustomer();
            driver.Navigate().GoToUrl(BaseUrl + "/KhachHang/ThongTin");
            Thread.Sleep(700);
            var emailInput = driver.FindElement(By.Name("Email"));
            Assert.AreEqual("nguyenvana@gmail.com", emailInput.GetAttribute("value"));
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

        [TearDown] public void TearDown() => driver.Quit();
    }
}
