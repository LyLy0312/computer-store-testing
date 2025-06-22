using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Assert = NUnit.Framework.Assert;
using System.Threading;

namespace QLWebTests
{
    [TestFixture]
    public class AdminProductTests
    {
        private IWebDriver driver;
        private const string BaseUrl = "http://localhost:54551";

        [SetUp]
        public void Setup()
        {
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
        }

        private void LoginAsAdmin()
        {
            driver.Navigate().GoToUrl($"{BaseUrl}/MayTinh/Login");
            driver.FindElement(By.Id("email")).SendKeys("admin@gmail.com");
            driver.FindElement(By.Name("password")).SendKeys("admin");

            var captchaCheckbox = driver.FindElement(By.Id("captchaCheckbox"));
            if (!captchaCheckbox.Selected)
                captchaCheckbox.Click();

            Thread.Sleep(1000);
            string captcha = driver.FindElement(By.Id("captchaCode")).Text;
            driver.FindElement(By.Name("captchaInput")).SendKeys(captcha);
            driver.FindElement(By.CssSelector("button[type='submit']")).Click();
        }

        [Test]
        public void Test_AddProduct()
        {
            LoginAsAdmin();

            driver.Navigate().GoToUrl($"{BaseUrl}/MayTinh/Add");

            driver.FindElement(By.Id("MTID")).SendKeys("SPTEST1");
            driver.FindElement(By.Id("TenSP")).SendKeys("Sản phẩm test");
            driver.FindElement(By.Id("ThuongHieu")).SendKeys("TestBrand");
            driver.FindElement(By.Id("DanhMucID")).SendKeys("1");
            driver.FindElement(By.Id("MoTa")).SendKeys("Mô tả sản phẩm");
            driver.FindElement(By.Id("Gia")).SendKeys("100000");
            driver.FindElement(By.Id("SoLuongKho")).SendKeys("10");
            driver.FindElement(By.Id("RAM")).SendKeys("8GB");
            driver.FindElement(By.Id("ViXuLy")).SendKeys("i5");
            driver.FindElement(By.Id("LuuTru")).SendKeys("512GB SSD");
            driver.FindElement(By.Id("KichThuocManHinh")).SendKeys("15.6\"");
            driver.FindElement(By.Id("MauSac")).SendKeys("Đen");
            driver.FindElement(By.Id("DungLuongPin")).SendKeys("4000mAh");

            // Scroll đến nút và click bằng JavaScript để tránh lỗi bị che
            var submitButton = driver.FindElement(By.CssSelector("input[type='submit']"));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", submitButton);
            Thread.Sleep(500);
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", submitButton);

            // Kiểm tra đã chuyển trang hoặc có thông báo thành công
            Assert.IsTrue(
                driver.PageSource.Contains("Thêm sản phẩm thành công") ||
                driver.PageSource.Contains("Quay lại") ||
                driver.Url.Contains("Index")
            );
        }

        [TearDown]
        public void Teardown()
        {
            driver.Quit();
        }
    }
}
