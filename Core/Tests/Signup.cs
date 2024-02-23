using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;

namespace AutomationHulkApps.Core.Tests
{
    internal class SignupTests
    {
        private IWebDriver _driver;

        [SetUp]
        public void Initialize()
        {
            _driver = new ChromeDriver();
        }

        // Method to enter password and login
        private void EnterPasswordAndLogin(string password)
        {
            _driver.Navigate().GoToUrl("https://qa-practical-test.myshopify.com/password");

            // Find the password input field and enter the password
            IWebElement passwordField = _driver.FindElement(By.Id("password"));
            passwordField.SendKeys(password);

            // Find the submit button and click to login
            IWebElement submitButton = _driver.FindElement(By.CssSelector("button[type='submit']"));
            submitButton.Click();
        }
        
        [Test]
        public void InvalidEmailSignup()
        {
            EnterPasswordAndLogin("brauff");
            _driver.Navigate().GoToUrl("https://qa-practical-test.myshopify.com/account/register");

            // Find the email input field and enter an invalid email address
            IWebElement emailField = _driver.FindElement(By.Id("RegisterForm-email"));
            emailField.SendKeys("invalidemail.com");

            // Wait for the "Create" button to load and become clickable
            WebDriverWait wait = new(_driver, TimeSpan.FromSeconds(3));
            IWebElement createButton = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id='create_customer']/button")));

            // Click the "Create" button
            createButton.Click();

            // Find the element for displaying the error message
            IWebElement errorMessage = wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector(".customer.register .errors li")));
            Assert.That(errorMessage.Text, Is.EqualTo("Email is invalid"));
        }

        [Test]
        public void InvalidPasswordSignup()
        {
            EnterPasswordAndLogin("brauff");
            _driver.Navigate().GoToUrl("https://qa-practical-test.myshopify.com/account/register");

            // Find the email input field
            IWebElement emailField = _driver.FindElement(By.Id("RegisterForm-email"));
            emailField.SendKeys("novikorisnik@example.com");

            // Find the password input field and enter an invalid (too short) password
            IWebElement passwordField = _driver.FindElement(By.Id("RegisterForm-password"));
            passwordField.SendKeys("abc");

            // Wait for the "Create" button to load and become clickable
            WebDriverWait wait = new(_driver, TimeSpan.FromSeconds(3));
            IWebElement createButton = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id='create_customer']/button")));

            // Click the "Create" button
            createButton.Click();

            // Find the element for displaying the error message
            IWebElement errorMessage = wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector(".customer.register .errors li")));
            Assert.That(errorMessage.Text, Is.EqualTo("Password is too short (minimum is 5 characters)"));
        }
        
        [TearDown]
        public void Cleanup()
        {
            _driver.Quit();
        }
    }
}
