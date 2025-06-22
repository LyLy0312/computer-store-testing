using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Threading;

namespace QLWebTests
{
    [TestFixture]
    public class UserFlowTests
    {
        private IWebDriver driver;
        private const string BaseUrl = "http://localhost:54551";

        [SetUp]
        public void Setup() => driver = new ChromeDriver();

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

        [Test]
        public void HomePage_ShouldDisplayProductList()
        {
            driver.Navigate().GoToUrl(BaseUrl + "/MayTinh/Index");
            Thread.Sleep(1000);
            var heading = driver.FindElement(By.XPath("//*[contains(text(),'Danh sách sản phẩm')]"));
            Assert.IsTrue(heading.Displayed);
            var products = driver.FindElements(By.CssSelector(".card"));
            Assert.IsTrue(products.Count > 0);
        }

        [Test]
        public void ProfilePage_UpdateInfo_ShouldSucceed()
        {
            LoginAsCustomer();
            driver.Navigate().GoToUrl(BaseUrl + "/KhachHang/ThongTin");
            var diachiInput = driver.FindElement(By.Name("DiaChi"));
            diachiInput.Clear();
            diachiInput.SendKeys("Hanoi - updated");
            driver.FindElement(By.CssSelector("button[type='submit']")).Click();
            Thread.Sleep(1000);
            Assert.IsTrue(driver.Url.Contains("ThongTin"));
        }

        [Test]
        public void Chatbot_ShouldOpenAndSendMessage()
        {
            driver.Navigate().GoToUrl(BaseUrl + "/MayTinh/Index");
            Thread.Sleep(500);
            driver.FindElement(By.Id("chatbot-btn")).Click();
            var chatbox = driver.FindElement(By.Id("chatbot-box"));
            Assert.IsTrue(chatbox.Displayed);
            driver.FindElement(By.CssSelector("#chatbot-input input")).SendKeys("Xin chào");
            driver.FindElement(By.CssSelector("#chatbot-input button")).Click();
            Thread.Sleep(1000);
            Assert.IsTrue(driver.PageSource.Contains("Xin chào"));
        }

        [Test]
        public void Footer_ShouldContainLinksAndIcons()
        {
            driver.Navigate().GoToUrl(BaseUrl + "/MayTinh/Index");
            Thread.Sleep(500);
            Assert.IsTrue(driver.PageSource.Contains("Trang Chủ") && driver.PageSource.Contains("Giới Thiệu"));
            Assert.IsTrue(driver.FindElements(By.CssSelector("footer img")).Count >= 2);
        }

        [TearDown]
        public void TearDown() => driver.Quit();
    }
}
