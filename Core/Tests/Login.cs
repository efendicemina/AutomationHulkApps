using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Threading;

namespace AutomationHulkApps.Core.Tests
{
    internal class LoginTests
    {
        private IWebDriver _driver;

        [SetUp]
        public void Initialize()
        {
            _driver = new ChromeDriver();
        }

        private void EnterPasswordAndLogin(string password)
        {
            _driver.Navigate().GoToUrl("https://qa-practical-test.myshopify.com/password");

            // Find the password input field and enter the password
            IWebElement passwordField = _driver.FindElement(By.Id("password"));
            passwordField.SendKeys(password);

            // Find the submit button and click to log in
            _ = _driver.FindElement(By.CssSelector("button[type='submit']"));
        }

            [Test]
            public void VerifySuccessfulLogin()
            {
                EnterPasswordAndLogin("brauff");
                _driver.Navigate().GoToUrl("https://qa-practical-test.myshopify.com/account/login");

                // Find the email input field and enter a valid email address
                IWebElement emailField = _driver.FindElement(By.Id("CustomerEmail"));
                emailField.SendKeys("eefendic1@etf.unsa.ba");

                // Find the password input field and enter a valid password
                IWebElement passwordField = _driver.FindElement(By.Id("CustomerPassword"));
                passwordField.SendKeys("nekipass");

                // Find the Sign in button and click
                IWebElement signInButton = _driver.FindElement(By.XPath("//button[contains(text(), 'Sign in')]"));
                signInButton.Click();

                // Wait for the redirection to the main page (or another page after successful login)
                Thread.Sleep(3000);

                // Check if the URL changed after successful login
                Assert.That(_driver.Url, Is.Not.EqualTo("https://qa-practical-test.myshopify.com/account/login"));
            }

            [Test]
            public void VerifyInvalidLogin()
            {
                EnterPasswordAndLogin("brauff");
                _driver.Navigate().GoToUrl("https://qa-practical-test.myshopify.com/account/login");

                // Find the email input field and enter an invalid email
                IWebElement emailField = _driver.FindElement(By.Id("CustomerEmail"));
                emailField.SendKeys("invalid@email.com");

                // Find the password input field and enter an invalid password
                IWebElement passwordField = _driver.FindElement(By.Id("CustomerPassword"));
                passwordField.SendKeys("invalidpassword");

                // Find the Sign in button and click
                IWebElement signInButton = _driver.FindElement(By.XPath("//button[contains(text(), 'Sign in')]"));
                signInButton.Click();

                // Wait a bit for any potential error message to appear (should not be displayed)
                Thread.Sleep(2000);

                // Check that we are still on the same page after unsuccessful login
                Assert.That(_driver.Url, Is.EqualTo("https://qa-practical-test.myshopify.com/account/login"));

                // Check that the error message is displayed
                IWebElement errorMessage = _driver.FindElement(By.CssSelector(".errors"));
                Assert.That(errorMessage, Is.Not.Null, "Error message not displayed");
            }

            [Test]
            public void VerifyForgotPasswordLink()
            {
                EnterPasswordAndLogin("brauff");
                _driver.Navigate().GoToUrl("https://qa-practical-test.myshopify.com/account/login");

                // Find the "Forgot your password?" link and click
                IWebElement forgotPasswordLink = _driver.FindElement(By.CssSelector("a[href='#recover']"));
                forgotPasswordLink.Click();

                // Wait a bit for the redirection to the password reset page
                Thread.Sleep(2000);

                // Check if we are on the password recovery page
                Assert.That(_driver.Url, Does.Contain("/account/login#recover"), "Not on password recovery page");
            }

            [Test]
            public void VerifyCreateAccountLink()
            {
                EnterPasswordAndLogin("brauff");
                _driver.Navigate().GoToUrl("https://qa-practical-test.myshopify.com/account/login");

                // Find the "Create account" link and click
                IWebElement createAccountLink = _driver.FindElement(By.CssSelector("a[href='/account/register']"));
                createAccountLink.Click();

                // Wait a bit for the redirection to the account registration page
                Thread.Sleep(2000);

                // Check if we are on the account registration page
                Assert.That(_driver.Url, Does.Contain("/account/register"), "Not on account registration page");
            }
           
            [TearDown]
            public void Cleanup()
            {
                _driver.Quit();
            }
        }
    }
