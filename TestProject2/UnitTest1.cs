using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;

namespace TrabalhoPratico.Tests
{
    public class LoginUITests
    {
        private IWebDriver driver;
        private WebDriverWait wait;

        [SetUp]
        public void Setup()
        {
            var options = new ChromeOptions();

            new DriverManager().SetUpDriver(new ChromeConfig());
            driver = new ChromeDriver(options);

            driver.Manage().Window.Size = new System.Drawing.Size(1280, 720);
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        }

        [Test]
        public void LoginComSucesso_DeveRedirecionar()
        {
            driver.Navigate().GoToUrl("http://localhost:5055");

            wait.Until(d => d.FindElement(By.TagName("body")));

            var emailInput = wait.Until(d => d.FindElement(By.Id("email")));
            emailInput.Clear();
            emailInput.SendKeys("alfredomandmaguiar@gmail.com23");

            var passwordInput = driver.FindElement(By.Id("password"));
            passwordInput.Clear();
            passwordInput.SendKeys("123456");

            var entrarButton = driver.FindElement(By.CssSelector("button.login-button"));
            entrarButton.Click();

            
            wait.Until(d => d.Url.Contains("/main"));
            Assert.IsTrue(driver.Url.Contains("/main"), "Deveria redirecionar para o painel após login.");
        }

        [Test]
        public void LoginComErro_DeveMostrarMensagemErro()
        {
            driver.Navigate().GoToUrl("http://localhost:5055");

            wait.Until(d => d.FindElement(By.TagName("body")));

            var emailInput = wait.Until(d => d.FindElement(By.Id("email")));
            emailInput.Clear();
            emailInput.SendKeys("email_invalido@example.com");

            var passwordInput = driver.FindElement(By.Id("password"));
            passwordInput.Clear();
            passwordInput.SendKeys("senhaErrada");

            var entrarButton = driver.FindElement(By.CssSelector("button.login-button"));
            entrarButton.Click();

            wait.Until(d => d.FindElement(By.CssSelector(".alert-error")));

            var errorMessage = driver.FindElement(By.CssSelector(".alert-error span:nth-child(2)"));

            Assert.AreEqual("Email ou senha incorretos.", errorMessage.Text);
        }


        [TearDown]
        public void TearDown()
        {
            if (driver != null)
            {
                driver.Quit();
                driver.Dispose();
                driver = null;
            }
        }
    }
}
