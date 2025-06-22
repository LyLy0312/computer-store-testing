using NUnit.Framework;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using System.Threading;

[TestFixture]
public class AdminFeedbackTests
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
    public void Test_ViewFeedbackList()
    {
        LoginAsAdmin();
        driver.Navigate().GoToUrl($"{BaseUrl}/MayTinh/DSPhanhoi");
        Assert.IsTrue(driver.PageSource.Contains("Phản Hồi") || driver.PageSource.Contains("Nội dung"));
    }

    [TearDown]
    public void Teardown() => driver.Quit();
}
