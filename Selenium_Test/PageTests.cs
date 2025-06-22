using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Threading;

namespace QLWebTests
{
    [TestFixture]
    public class SimpleSmokeTests
    {
        private IWebDriver driver;
        private const string BaseUrl = "http://localhost:54551";

        [SetUp]
        public void Setup()
        {
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
        }

        private void AssertPageLoads(string url)
        {
            driver.Navigate().GoToUrl(url);
            Thread.Sleep(500);
            TestContext.WriteLine(driver.PageSource);
            Assert.IsTrue(driver.PageSource.Length > 100, $"Trang {url} có thể không load đúng.");
        }

        [Test]
        public void HomePage_ShouldLoad() => AssertPageLoads($"{BaseUrl}/MayTinh/Index");

        [Test]
        public void LoginPage_ShouldLoad() => AssertPageLoads($"{BaseUrl}/MayTinh/Login");

        [Test]
        public void RegisterPage_ShouldLoad() => AssertPageLoads($"{BaseUrl}/MayTinh/Register");

        [Test]
        public void ForgotPasswordPage_ShouldLoad() => AssertPageLoads($"{BaseUrl}/MayTinh/Forgot");

        [Test]
        public void ContactPage_ShouldLoad()
        {
            driver.Navigate().GoToUrl($"{BaseUrl}/MayTinh/Login");
            driver.FindElement(By.Id("email")).SendKeys("nguyenvana@gmail.com");
            driver.FindElement(By.Name("password")).SendKeys("1");
            driver.FindElement(By.Id("captchaCheckbox")).Click();
            Thread.Sleep(500);
            driver.FindElement(By.Name("captchaInput")).SendKeys(driver.FindElement(By.Id("captchaCode")).Text);
            driver.FindElement(By.CssSelector("button[type='submit']")).Click();
            Thread.Sleep(1000);
            AssertPageLoads($"{BaseUrl}/MayTinh/LienHe");
        }

        [Test]
        public void AboutPage_ShouldLoad()
        {
            driver.Navigate().GoToUrl($"{BaseUrl}/MayTinh/GioiThieu");
            Thread.Sleep(500);
            Assert.IsTrue(driver.PageSource.Length > 100);
        }

        [Test]
        public void MapPage_ShouldLoad()
        {
            driver.Navigate().GoToUrl($"{BaseUrl}/MayTinh/Maps");
            Thread.Sleep(500);
            Assert.IsTrue(driver.PageSource.Contains("iframe") || driver.PageSource.Length > 100);
        }

        [Test]
        public void CartPage_ShouldLoad()
        {
            driver.Navigate().GoToUrl($"{BaseUrl}/MayTinh/Index");
            Thread.Sleep(500);
            var links = driver.FindElements(By.TagName("a"));
            foreach (var link in links)
            {
                if (link.GetAttribute("href") != null && link.GetAttribute("href").Contains("ThemGioHang"))
                {
                    link.Click();
                    break;
                }
            }
            Thread.Sleep(1000);
            driver.Navigate().GoToUrl($"{BaseUrl}/GioHang/GioHang");
            Assert.IsTrue(driver.PageSource.Length > 100);
        }

        [Test]
        public void OrderListPage_ShouldLoad_WhenLoggedIn()
        {
            driver.Navigate().GoToUrl($"{BaseUrl}/MayTinh/Login");
            driver.FindElement(By.Id("email")).SendKeys("nguyenvana@gmail.com");
            driver.FindElement(By.Name("password")).SendKeys("1");
            driver.FindElement(By.Id("captchaCheckbox")).Click();
            Thread.Sleep(500);
            driver.FindElement(By.Name("captchaInput")).SendKeys(driver.FindElement(By.Id("captchaCode")).Text);
            driver.FindElement(By.CssSelector("button[type='submit']")).Click();
            Thread.Sleep(1000);
            AssertPageLoads($"{BaseUrl}/KhachHang/DanhSachDonHang");
        }

        [Test]
        public void AdminProductPage_ShouldLoad_WhenLoggedIn()
        {
            driver.Navigate().GoToUrl($"{BaseUrl}/MayTinh/Login");
            driver.FindElement(By.Id("email")).SendKeys("admin@gmail.com");
            driver.FindElement(By.Name("password")).SendKeys("123");
            driver.FindElement(By.Id("captchaCheckbox")).Click();
            Thread.Sleep(500);
            driver.FindElement(By.Name("captchaInput")).SendKeys(driver.FindElement(By.Id("captchaCode")).Text);
            driver.FindElement(By.CssSelector("button[type='submit']")).Click();
            Thread.Sleep(1000);
            AssertPageLoads($"{BaseUrl}/MayTinh/test");
        }

        [TearDown]
        public void TearDown()
        {
            driver.Quit();
        }
    }
}
