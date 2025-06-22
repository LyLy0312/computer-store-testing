using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Assert = NUnit.Framework.Assert;
using System.Threading;

namespace QLWebTests
{
    [TestFixture]
    public class CartTests
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
        public void Cart_AddProductAndCheckout_ShouldSucceed()
        {
            LoginAsCustomer(); 
            driver.Navigate().GoToUrl($"{BaseUrl}/MayTinh");
            Thread.Sleep(1000);

            var addLink = driver.FindElement(By.XPath("//a[contains(@href, 'ThemGioHang')]"));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", addLink);
            Thread.Sleep(500);
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", addLink);

            driver.Navigate().GoToUrl($"{BaseUrl}/GioHang/GioHang");
            Thread.Sleep(1000);

            var checkbox = driver.FindElement(By.CssSelector("input[type='checkbox']"));
            if (!checkbox.Selected)
                checkbox.Click();

            var addressInput = driver.FindElement(By.Name("DiaChi"));
            addressInput.Clear();
            addressInput.SendKeys("123 ABC HCM");

            var confirmBtn = driver.FindElement(By.CssSelector("button.checkout"));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", confirmBtn);
            Thread.Sleep(300);
            confirmBtn.Click();

            Thread.Sleep(1000);

            var orderBtn = driver.FindElement(By.CssSelector("form button[type='submit']"));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", orderBtn);
            Thread.Sleep(300);
            orderBtn.Click();

            System.Threading.Thread.Sleep(2000); 
            var confirmation = driver.FindElements(By.XPath("//*[contains(text(),'Đặt hàng thành công')]"));
            Assert.IsTrue(confirmation.Count > 0, "Không thấy thông báo đặt hàng thành công.");
        }

        [TearDown]
        public void Teardown()
        {
            driver.Quit();
        }
    }
}
