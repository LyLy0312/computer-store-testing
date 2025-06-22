using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Threading;

namespace QLWebTests
{
    [TestFixture]
    public class ProductTests
    {
        private IWebDriver driver;
        private const string BaseUrl = "http://localhost:54551";

        [SetUp]
        public void Setup()
        {
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
        }

        private void Navigate(string path)
        {
            driver.Navigate().GoToUrl($"{BaseUrl}{path}");
            Thread.Sleep(1000);
        }

        [Test]
        public void ProductList_ShouldLoadWithoutError()
        {
            Navigate("/MayTinh/Index");
            Assert.IsFalse(driver.PageSource.Contains("Lỗi") || driver.PageSource.Contains("500"));
        }

        [Test]
        public void ProductDetail_Page_ShouldLoadCorrectly()
        {
            Navigate("/MayTinh/Details/1");
            Assert.IsTrue(driver.PageSource.Contains("Mô tả") || driver.PageSource.Contains("Tên sản phẩm"));
        }

        [Test]
        public void ProductDetail_ShouldHaveImage()
        {
            Navigate("/MayTinh/Details/1");
            Assert.IsTrue(driver.FindElements(By.TagName("img")).Count > 0);
        }

        [Test]
        public void Cart_AddButton_ShouldExistOnProductPage()
        {
            Navigate("/MayTinh/Index");
            Assert.IsTrue(driver.PageSource.Contains("Thêm vào giỏ") || driver.PageSource.Contains("ThemGioHang"));
        }


        [Test]
        public void SearchBox_ShouldExistOnHomePage()
        {
            Navigate("/MayTinh/Index");
            Assert.IsTrue(driver.FindElements(By.Name("txtSearch")).Count > 0);
        }

        [Test]
        public void CategoryFilter_ShouldNotCrash()
        {
            Navigate("/MayTinh/Index?DanhMucID=1");
            Assert.IsFalse(driver.PageSource.Contains("Lỗi") || driver.PageSource.Contains("500"));
        }
        [Test]
        public void PageNotExist_ShouldFail()
        {
            driver.Navigate().GoToUrl(BaseUrl + "/MayTinh/KhongTonTai");
            Thread.Sleep(500);
            Assert.IsFalse(driver.PageSource.Contains("404") || driver.PageSource.Contains("Server Error"),
                "Trang không tồn tại nhưng không phát hiện lỗi.");
        }
        [Test]
        public void InvalidProductDetails_ShouldFail()
        {
            driver.Navigate().GoToUrl(BaseUrl + "/MayTinh/Details/9999");
            Thread.Sleep(500);
            Assert.IsFalse(driver.PageSource.Contains("404") || driver.PageSource.Contains("Server Error"),
                "Chi tiết sản phẩm không hợp lệ nhưng không phát hiện lỗi.");
        }

        [TearDown]
        public void Teardown()
        {
            driver.Quit();
        }
    }
}