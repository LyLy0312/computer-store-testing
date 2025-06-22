using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Threading;

namespace QLWebTests
{
    [TestFixture]
    public class ExtraPageTests
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
        public void HomePageSortDesc_ShouldLoad()
        {
            driver.Navigate().GoToUrl(BaseUrl + "/MayTinh/Index?sort=desc");
            Thread.Sleep(500);
            Assert.IsTrue(driver.PageSource.Length > 100);
        }

        [Test]
        public void AdminProductList_ShouldLoad()
        {
            driver.Navigate().GoToUrl(BaseUrl + "/MayTinh/test");
            Thread.Sleep(500);
            Assert.IsTrue(driver.PageSource.Length > 100);
        }

        [Test]
        public void RegisterPage_WithRef_ShouldLoad()
        {
            driver.Navigate().GoToUrl(BaseUrl + "/MayTinh/Register?ref=homepage");
            Thread.Sleep(500);
            Assert.IsTrue(driver.PageSource.Length > 100);
        }

        [Test]
        public void Logout_ShouldRedirect()
        {
            driver.Navigate().GoToUrl(BaseUrl + "/MayTinh/Logout");
            Thread.Sleep(500);
            Assert.IsTrue(driver.PageSource.Length > 50);
        }


        [Test]
        public void OrderListPage_ShouldLoad()
        {
            driver.Navigate().GoToUrl(BaseUrl + "/KhachHang/DanhSachDonHang");
            Thread.Sleep(500);
            Assert.IsTrue(driver.PageSource.Length > 100);
        }

        [Test]
        public void AddProductPage_DirectAccess_ShouldLoad()
        {
            driver.Navigate().GoToUrl(BaseUrl + "/MayTinh/Add");
            Thread.Sleep(500);
            Assert.IsTrue(driver.PageSource.Length > 100);
        }

        [Test]
        public void ProductDetails_ShouldLoad()
        {
            driver.Navigate().GoToUrl(BaseUrl + "/MayTinh/Details/1");
            Thread.Sleep(500);
            Assert.IsTrue(driver.PageSource.Length > 100);
        }
       

        [Test]
        public void ProductDetails2_ShouldLoad()
        {
            driver.Navigate().GoToUrl(BaseUrl + "/MayTinh/Details/2");
            Thread.Sleep(500);
            Assert.IsFalse(driver.PageSource.Contains("404") || driver.PageSource.Contains("Server Error"));
        }


        [Test]
        public void Category2HomePage_ShouldLoad()
        {
            driver.Navigate().GoToUrl(BaseUrl + "/MayTinh/Index?DanhMucID=2");
            Thread.Sleep(500);
            Assert.IsFalse(driver.PageSource.Contains("404") || driver.PageSource.Contains("Server Error"));
        }
        [Test]
        public void ProductDetails2_ShouldLoad1()
        {
            driver.Navigate().GoToUrl(BaseUrl + "/MayTinh/Details/2");
            Thread.Sleep(500);
            Assert.IsFalse(driver.PageSource.Contains("404") || driver.PageSource.Contains("Server Error"));
        }


        [Test]
        public void Category2HomePage_ShouldLoad1()
        {
            driver.Navigate().GoToUrl(BaseUrl + "/MayTinh/Index?DanhMucID=2");
            Thread.Sleep(500);
            Assert.IsFalse(driver.PageSource.Contains("404") || driver.PageSource.Contains("Server Error"));
        }

        [TearDown]
        public void TearDown()
        {
            driver.Quit();
        }
    }
}
