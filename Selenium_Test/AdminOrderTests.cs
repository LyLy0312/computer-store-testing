using NUnit.Framework;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using System.Threading;

[TestFixture]
public class AdminOrderTests
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
    public void Test_ViewOrdersAndDetails()
    {
        LoginAsAdmin();
        driver.Navigate().GoToUrl($"{BaseUrl}/MayTinh/DSDonHang");
        Assert.IsTrue(driver.PageSource.Contains("Đơn Hàng"));

        var viewLinks = driver.FindElements(By.LinkText("Xem"));
        Assert.IsTrue(viewLinks.Count > 0);
        viewLinks[0].Click();
        Assert.IsTrue(driver.PageSource.Contains("Chi Tiết Đơn Hàng"));
    }

    [TearDown]
    public void Teardown() => driver.Quit();
}
