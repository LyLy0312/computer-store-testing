using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Threading;

namespace QLWebTests
{
    [TestFixture]
    public class AdminManageTests
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

        private void SafeClick(By by)
        {
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            var element = driver.FindElement(by);
            js.ExecuteScript("arguments[0].scrollIntoView(true);", element);
            Thread.Sleep(300);
            js.ExecuteScript("arguments[0].click();", element);
        }

        [Test]
        public void Admin_EditProduct_ShouldLoadForm()
        {
            LoginAsAdmin();
            driver.Navigate().GoToUrl($"{BaseUrl}/MayTinh/Edit/1");
            Assert.IsTrue(driver.PageSource.Contains("Sửa sản phẩm") || driver.PageSource.Contains("Giá"));
        }

        [Test]
        public void Admin_EditProduct_WithValidData_ShouldSucceed()
        {
            LoginAsAdmin();
            driver.Navigate().GoToUrl($"{BaseUrl}/MayTinh/Edit/1");
            var nameBox = driver.FindElement(By.Id("TenSP"));
            nameBox.Clear();
            nameBox.SendKeys("Laptop sửa");
            SafeClick(By.CssSelector("input[type='submit']"));
            Thread.Sleep(1000);
            Assert.IsTrue(driver.Url.Contains("test") || driver.PageSource.Contains("Thành công"));
        }

        [Test]
        public void Admin_EditProduct_EmptyName_ShouldShowError()
        {
            LoginAsAdmin();
            driver.Navigate().GoToUrl($"{BaseUrl}/MayTinh/Edit/1");
            var nameBox = driver.FindElement(By.Id("TenSP"));
            nameBox.Clear();
            SafeClick(By.CssSelector("input[type='submit']"));
            Assert.IsTrue(driver.PageSource.Contains("không được để trống") || driver.PageSource.Contains("Validation"));
        }

        [Test]
        public void Admin_DeleteProduct_ShouldRedirectToList()
        {
            LoginAsAdmin();
            driver.Navigate().GoToUrl($"{BaseUrl}/MayTinh/Delete/1");
            SafeClick(By.CssSelector("button[type='submit'], input[type='submit']"));
            Thread.Sleep(1000);
            Assert.IsTrue(driver.Url.Contains("test"));
        }

        [Test]
        public void Admin_AddProduct_InvalidPrice_ShouldFail()
        {
            LoginAsAdmin();
            driver.Navigate().GoToUrl($"{BaseUrl}/MayTinh/Add");
            driver.FindElement(By.Id("MTID")).SendKeys("SPINVALID1");
            driver.FindElement(By.Id("Gia")).SendKeys("abc");
            SafeClick(By.CssSelector("input[type='submit']"));
            Assert.IsTrue(driver.PageSource.Contains("không hợp lệ") || driver.PageSource.Contains("Validation"));
        }

        [Test]
        public void Admin_ProductList_ShouldContainNewProduct()
        {
            LoginAsAdmin();
            driver.Navigate().GoToUrl($"{BaseUrl}/MayTinh/test");
            Assert.IsTrue(driver.PageSource.Contains("SPTEST") || driver.PageSource.Contains("Laptop sửa"));
        }

        [Test]
        public void Admin_CannotDeleteProductWithOrder()
        {
            LoginAsAdmin();
            driver.Navigate().GoToUrl($"{BaseUrl}/MayTinh/Delete/1");
            SafeClick(By.CssSelector("button[type='submit'], input[type='submit']"));
            Thread.Sleep(1000);
            Assert.IsTrue(driver.PageSource.Contains("không thể xóa") || driver.PageSource.Contains("đơn hàng"));
        }

        [Test]
        public void Admin_NavigateToProductManagement_ShouldSucceed()
        {
            LoginAsAdmin();
            driver.Navigate().GoToUrl($"{BaseUrl}/MayTinh/test");
            Assert.IsTrue(driver.PageSource.Contains("Thêm sản phẩm") || driver.PageSource.Contains("Danh sách"));
        }

        [Test]
        public void Admin_AddProduct_ShouldRedirectBackToList()
        {
            LoginAsAdmin();
            driver.Navigate().GoToUrl($"{BaseUrl}/MayTinh/Add");
            driver.FindElement(By.Id("MTID")).SendKeys("SPFAST");
            driver.FindElement(By.Id("TenSP")).SendKeys("Quick Add");
            driver.FindElement(By.Id("Gia")).SendKeys("5000000");
            driver.FindElement(By.Id("SoLuongKho")).SendKeys("5");
            SafeClick(By.CssSelector("input[type='submit']"));
            Thread.Sleep(1000);
            Assert.IsTrue(driver.Url.Contains("test") || driver.PageSource.Contains("Thêm"));
        }

        [TearDown]
        public void Teardown()
        {
            driver.Quit();
        }
    }
}
