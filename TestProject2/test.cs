using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;

namespace TrabalhoPratico.Tests
{
    public class TestSelenium
    {
        private IWebDriver driver;

        [SetUp]
        public void Setup()
        {
            new DriverManager().SetUpDriver(new ChromeConfig());
            driver = new ChromeDriver();
        }

        [Test]
        public void TestGoogleOpen()
        {
            driver.Navigate().GoToUrl("https://www.google.com");
            Assert.IsTrue(driver.Title.Contains("Google"));
        }

        [TearDown]
        public void TearDown()
        {
            driver?.Quit();
            driver?.Dispose();
        }
    }
}