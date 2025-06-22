using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;

namespace QLWebTests
{
    [TestFixture]
    public class StaticPagesTests
    {
        private IWebDriver driver;
        private const string BaseUrl = "http://localhost:54551"; // thay đổi đúng theo port bạn đang chạy

        [SetUp]
        public void Setup()
        {
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
        }

        [Test]
        public void Test_LienHeFormSubmission()
        {
            driver.Navigate().GoToUrl($"{BaseUrl}/MayTinh/Login");
            driver.FindElement(By.Id("email")).SendKeys("nguyenvana@gmail.com");
            driver.FindElement(By.Name("password")).SendKeys("1");

            var captchaCheckbox = driver.FindElement(By.Id("captchaCheckbox"));
            if (!captchaCheckbox.Selected)
                captchaCheckbox.Click();

            System.Threading.Thread.Sleep(1000);
            var captchaCode = driver.FindElement(By.Id("captchaCode")).Text;
            driver.FindElement(By.Name("captchaInput")).SendKeys(captchaCode);
            driver.FindElement(By.CssSelector("button[type='submit']")).Click();

            driver.Navigate().GoToUrl($"{BaseUrl}/MayTinh/LienHe");

            driver.FindElement(By.Name("noidung")).SendKeys("Đây là phản hồi test tự động từ Selenium.");
            driver.FindElement(By.CssSelector("button[type='submit']")).Click();

            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            bool popupAppeared = wait.Until(d =>
            {
                try
                {
                    var popup = d.FindElement(By.Id("popupOverlay"));
                    return popup.Displayed;
                }
                catch (NoSuchElementException)
                {
                    return false;
                }
            });

            // B5: Kiểm tra popup đã hiện
            Assert.IsTrue(popupAppeared, "Không thấy popup phản hồi sau khi gửi form.");
        }

        [TearDown]
        public void TearDown()
        {
            driver.Quit();
        }
    }
}
