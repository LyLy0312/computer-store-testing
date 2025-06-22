using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Threading;

namespace QLWebTests
{
    [TestFixture]
    public class StatsTests
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
            driver.FindElement(By.Name("password")).SendKeys("123");
            driver.FindElement(By.Id("captchaCheckbox")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.Name("captchaInput")).SendKeys(driver.FindElement(By.Id("captchaCode")).Text);
            driver.FindElement(By.CssSelector("button[type='submit']")).Click();
        }

        [Test]
        public void Test_ThongKeDoanhThu()
        {
            LoginAsAdmin();
            driver.Navigate().GoToUrl($"{BaseUrl}/MayTinh/TKDoanhThu");

            driver.FindElement(By.Name("tuNgay")).SendKeys("2024-01-01");
            driver.FindElement(By.Name("denNgay")).SendKeys("2024-12-31");
            driver.FindElement(By.CssSelector("button[type='submit']")).Click();

            Thread.Sleep(1000);

            var content = driver.PageSource;
            Assert.IsTrue(content.Contains("Tổng doanh thu") || content.Contains("Không có dữ liệu"));
        }

        [TearDown]
        public void Teardown()
        {
            driver.Quit();
        }
    }
}
