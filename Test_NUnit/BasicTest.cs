using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Threading;

namespace QLWebTests
{
    [TestFixture]
    public class HomePage_ShouldLoad
    {
        private IWebDriver driver;
        private const string BaseUrl = "http://localhost:54551";

        [SetUp]
        public void Setup() => driver = new ChromeDriver();

        [Test]
        public void HomePage_ShouldLoad_Test()
        {
            driver.Navigate().GoToUrl(BaseUrl + "/MayTinh/Index");
            Thread.Sleep(500);
            Assert.IsTrue(driver.PageSource.Length > 100);
        }

        [TearDown]
        public void TearDown() => driver.Quit();
        [TestFixture]
        public class LoginPage_ShouldLoad
        {
            private IWebDriver driver;
            private const string BaseUrl = "http://localhost:54551";

            [SetUp]
            public void Setup() => driver = new ChromeDriver();

            [Test]
            public void LoginPage_ShouldLoad_Test()
            {
                driver.Navigate().GoToUrl(BaseUrl + "/MayTinh/Login");
                Thread.Sleep(500);
                Assert.IsTrue(driver.PageSource.Contains("Đăng nhập"));
            }

            [TearDown]
            public void TearDown() => driver.Quit();
        }
        [TestFixture]
        public class RegisterPage_ShouldLoad
        {
            private IWebDriver driver;
            private const string BaseUrl = "http://localhost:54551";

            [SetUp]
            public void Setup() => driver = new ChromeDriver();

            [Test]
            public void RegisterPage_ShouldLoad_Test()
            {
                driver.Navigate().GoToUrl(BaseUrl + "/MayTinh/Register");
                Thread.Sleep(500);
                Assert.IsTrue(driver.PageSource.Contains("Đăng ký"));
            }

            [TearDown]
            public void TearDown() => driver.Quit();
        }
        [TestFixture]
        public class ForgotPage_ShouldLoad
        {
            private IWebDriver driver;
            private const string BaseUrl = "http://localhost:54551";

            [SetUp]
            public void Setup() => driver = new ChromeDriver();

            [Test]
            public void ForgotPage_ShouldLoad_Test()
            {
                driver.Navigate().GoToUrl(BaseUrl + "/MayTinh/Forgot");
                Thread.Sleep(500);
                Assert.IsTrue(driver.PageSource.Contains("email"));
            }

            [TearDown]
            public void TearDown() => driver.Quit();
        }
        [TestFixture]
        public class AboutPage_ShouldLoad
        {
            private IWebDriver driver;
            private const string BaseUrl = "http://localhost:54551";

            [SetUp]
            public void Setup() => driver = new ChromeDriver();

            [Test]
            public void AboutPage_ShouldLoad_Test()
            {
                driver.Navigate().GoToUrl(BaseUrl + "/MayTinh/GioiThieu");
                Thread.Sleep(500);
                Assert.IsTrue(driver.PageSource.Length > 100);
            }

            [TearDown]
            public void TearDown() => driver.Quit();
        }
        [TestFixture]
        public class ContactPage_ShouldLoad
        {
            private IWebDriver driver;
            private const string BaseUrl = "http://localhost:54551";

            [SetUp]
            public void Setup() => driver = new ChromeDriver();

            [Test]
            public void ContactPage_ShouldLoad_Test()
            {
                driver.Navigate().GoToUrl(BaseUrl + "/MayTinh/LienHe");
                Thread.Sleep(500);
                Assert.IsTrue(driver.PageSource.Length > 100);
            }

            [TearDown]
            public void TearDown() => driver.Quit();
        }
        [TestFixture]
        public class CartPage_ShouldLoad
        {
            private IWebDriver driver;
            private const string BaseUrl = "http://localhost:54551";

            [SetUp]
            public void Setup() => driver = new ChromeDriver();

            [Test]
            public void CartPage_ShouldLoad_Test()
            {
                driver.Navigate().GoToUrl(BaseUrl + "/GioHang/GioHang");
                Thread.Sleep(500);
                Assert.IsTrue(driver.PageSource.Length > 100);
            }

            [TearDown]
            public void TearDown() => driver.Quit();
        }
        [TestFixture]
        public class ProfilePage_ShouldLoad
        {
            private IWebDriver driver;
            private const string BaseUrl = "http://localhost:54551";

            [SetUp]
            public void Setup() => driver = new ChromeDriver();

            [Test]
            public void ProfilePage_ShouldLoad_Test()
            {
                driver.Navigate().GoToUrl(BaseUrl + "/KhachHang/ThongTin");
                Thread.Sleep(500);
                Assert.IsTrue(driver.PageSource.Length > 100);
            }

            [TearDown]
            public void TearDown() => driver.Quit();
        }
        [TestFixture]
        public class LogoutPage_ShouldLoad
        {
            private IWebDriver driver;
            private const string BaseUrl = "http://localhost:54551";

            [SetUp]
            public void Setup() => driver = new ChromeDriver();

            [Test]
            public void LogoutPage_ShouldLoad_Test()
            {
                driver.Navigate().GoToUrl(BaseUrl + "/MayTinh/Logout");
                Thread.Sleep(500);
                Assert.IsTrue(driver.PageSource.Length > 50);
            }

            [TearDown]
            public void TearDown() => driver.Quit();
        }
        [TestFixture]
        public class AddProductPage_ShouldLoad
        {
            private IWebDriver driver;
            private const string BaseUrl = "http://localhost:54551";

            [SetUp]
            public void Setup() => driver = new ChromeDriver();

            [Test]
            public void AddProductPage_ShouldLoad_Test()
            {
                driver.Navigate().GoToUrl(BaseUrl + "/MayTinh/Add");
                Thread.Sleep(500);
                Assert.IsTrue(driver.PageSource.Length > 100);
            }

            [TearDown]
            public void TearDown() => driver.Quit();
        }


    }
}
