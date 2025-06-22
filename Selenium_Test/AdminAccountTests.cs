using NUnit.Framework;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using System.Threading;

[TestFixture]
public class AdminAccountTests
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
    public void Test_ViewUserAccounts()
    {
        LoginAsAdmin();
        driver.Navigate().GoToUrl($"{BaseUrl}/MayTinh/DSTaiKhoan");
        Assert.IsTrue(driver.PageSource.Contains("Danh sách tài khoản khách hàng"));
        Assert.IsTrue(driver.PageSource.Contains("Email"));
    }

    [TearDown]
    public void Teardown() => driver.Quit();
}
