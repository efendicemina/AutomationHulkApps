using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Threading;

namespace AutomationHulkApps.Core.Tests
{
    internal class FormTests
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
            IWebElement submitButton = _driver.FindElement(By.CssSelector("button[type='submit']"));
            submitButton.Click();
        }
        
        [Test]
        public void FillAndSubmitForm()
        {
            EnterPasswordAndLogin("brauff");
            _driver.Navigate().GoToUrl("https://qa-practical-test.myshopify.com/pages/form-builder");

            // Switch to the iframe
            _driver.SwitchTo().Frame("frame_8Q1zrNOWpYyVd3BKCe2hAA");

            WebDriverWait wait = new(_driver, TimeSpan.FromSeconds(10));

            // Wait for the first name input field
            IWebElement firstName = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("form_input_0")));
            firstName.SendKeys("Emina");

            // Wait for the last name input field
            IWebElement lastName = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("form_input_1")));
            lastName.SendKeys("Doe");

            // Wait for the email input field
            IWebElement email = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("form_input_2")));
            email.SendKeys("emina.doe@example.com");

            // Wait for the date of birth input field
            IWebElement dateOfBirth = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("form_input_3")));
            dateOfBirth.SendKeys("02/23/1990");

            // Wait for the website input field
            IWebElement website = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("form_input_5")));
            website.SendKeys("www.example.com");

            // Wait for the ratings input field
            IWebElement ratings = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("form_input_6")));
            ratings.SendKeys("9");

            // Submit the form
            _driver.FindElement(By.CssSelector(".btn")).Click();

            // Wait for success message
            IWebElement successMessage = wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("#thank-you-alert p")));
            Assert.That(successMessage.Text, Does.Contain("Thank you! The form was submitted successfully."),
                "Form submission failed. Success message not displayed.");

            // Switch back to default content
            _driver.SwitchTo().DefaultContent();
        }
        
        [Test]
        public void FillAndSubmitForm_RatingGreaterThan10()
        {
            EnterPasswordAndLogin("brauff");
            _driver.Navigate().GoToUrl("https://qa-practical-test.myshopify.com/pages/form-builder");

            // Switch to the iframe
            _driver.SwitchTo().Frame("frame_8Q1zrNOWpYyVd3BKCe2hAA");

            WebDriverWait wait = new(_driver, TimeSpan.FromSeconds(10));

            // Wait for the first name input field
            IWebElement firstName = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("form_input_0")));
            firstName.SendKeys("Emina");

            // Wait for the last name input field
            IWebElement lastName = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("form_input_1")));
            lastName.SendKeys("Doe");

            // Wait for the email input field
            IWebElement email = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("form_input_2")));
            email.SendKeys("emina.doe@example.com");

            // Wait for the date of birth input field
            IWebElement dateOfBirth = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("form_input_3")));
            dateOfBirth.SendKeys("02/23/1990");

            // Wait for the website input field
            IWebElement website = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("form_input_5")));
            website.SendKeys("www.example.com");

            // Wait for the ratings input field
            IWebElement ratings = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("form_input_6")));
            ratings.SendKeys("11"); // Rating greater than 10

            // Submit the form
            _driver.FindElement(By.CssSelector(".btn")).Click();

            // Wait for error messages
            IWebElement errorMessages = wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector(".alert-danger")));
            Assert.That(errorMessages.Text, Does.Contain("Give Ratings must be less than 10"), "Rating error message not displayed.");

            // Switch back to default content
            _driver.SwitchTo().DefaultContent();
        }
        
        [Test]
        public void FillAndSubmitForm_MissingRequiredFields()
        {
            EnterPasswordAndLogin("brauff");
            _driver.Navigate().GoToUrl("https://qa-practical-test.myshopify.com/pages/form-builder");

            // Switch to the iframe
            _driver.SwitchTo().Frame("frame_8Q1zrNOWpYyVd3BKCe2hAA");

            WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));

            // Wait for the first name input field
            IWebElement firstName = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("form_input_0")));
            firstName.SendKeys("Emina");

            // Wait for the last name input field
            IWebElement lastName = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("form_input_1")));
            lastName.SendKeys("");

            // Wait for the email input field
            IWebElement email = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("form_input_2")));
            email.SendKeys("");

            // Wait for the date of birth input field
            IWebElement dateOfBirth = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("form_input_3")));
            dateOfBirth.SendKeys("02/23/1990");

            // Wait for the website input field
            IWebElement website = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("form_input_5")));
            website.SendKeys("www.example.com");

            // Wait for the ratings input field
            IWebElement ratings = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("form_input_6")));
            ratings.SendKeys(""); 

            // Submit the form
            _driver.FindElement(By.CssSelector(".btn")).Click();

            // Wait for error messages
            IWebElement errorMessages = wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector(".alert-danger")));
            Assert.That(errorMessages.Text, Does.Contain("Last Name is required"), "Last Name error message not displayed.");
            Assert.That(errorMessages.Text, Does.Contain("Email is required"), "Email error message not displayed.");
            Assert.That(errorMessages.Text, Does.Contain("Give Ratings is required"), "Rating error message not displayed.");

            // Switch back to default content
            _driver.SwitchTo().DefaultContent();
        }
        
        [Test]
        public void FillAndSubmitForm_OptionalFieldsEmpty()
        {
            EnterPasswordAndLogin("brauff");
            _driver.Navigate().GoToUrl("https://qa-practical-test.myshopify.com/pages/form-builder");

            // Switch to the iframe
            _driver.SwitchTo().Frame("frame_8Q1zrNOWpYyVd3BKCe2hAA");

            WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));

            // Wait for the first name input field
            IWebElement firstName = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("form_input_0")));
            firstName.SendKeys("");

            // Wait for the last name input field
            IWebElement lastName = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("form_input_1")));
            lastName.SendKeys("Efendic");

            // Wait for the email input field
            IWebElement email = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("form_input_2")));
            email.SendKeys("example@example.com");

            // Wait for the date of birth input field
            IWebElement dateOfBirth = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("form_input_3")));
            dateOfBirth.SendKeys("");

            // Wait for the website input field
            IWebElement website = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("form_input_5")));
            website.SendKeys("");

            // Wait for the ratings input field
            IWebElement ratings = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("form_input_6")));
            ratings.SendKeys("9"); 

            // Submit the form
            _driver.FindElement(By.CssSelector(".btn")).Click();

            // Wait for success message
            IWebElement successMessage = wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("#thank-you-alert p")));
            Assert.That(successMessage.Text, Does.Contain("Thank you! The form was submitted successfully."),
                "Form submission failed. Success message not displayed.");

            // Switch back to default content
            _driver.SwitchTo().DefaultContent();
        }

        [Test]
        public void FillAndSubmitForm_FutureDateOfBirth()
        {
            EnterPasswordAndLogin("brauff");
            _driver.Navigate().GoToUrl("https://qa-practical-test.myshopify.com/pages/form-builder");

            // Switch to the iframe
            _driver.SwitchTo().Frame("frame_8Q1zrNOWpYyVd3BKCe2hAA");

            WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));

            // Wait for the first name input field
            IWebElement firstName = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("form_input_0")));
            firstName.SendKeys("Emina");

            // Wait for the last name input field
            IWebElement lastName = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("form_input_1")));
            lastName.SendKeys("Doe");

            // Wait for the email input field
            IWebElement email = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("form_input_2")));
            email.SendKeys("emina.doe@example.com");

            // Wait for the date of birth input field
            IWebElement dateOfBirth = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("form_input_3")));
            // Select future date
            dateOfBirth.SendKeys(DateTime.Now.AddDays(1).ToString("MM/dd/yyyy"));

            // Wait for the website input field
            IWebElement website = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("form_input_5")));
            website.SendKeys("www.example.com");

            // Wait for the ratings input field
            IWebElement ratings = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("form_input_6")));
            ratings.SendKeys("9");

            // Submit the form
            _driver.FindElement(By.CssSelector(".btn")).Click();

            IWebElement errorMessages = wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector(".alert-danger")));
            Assert.That(errorMessages.Text, Does.Contain("Date cannot be in the future"), "Date error message not displayed.");

            // Switch back to default content
            _driver.SwitchTo().DefaultContent();
        }

        [TearDown]
        public void Cleanup()
        {
            _driver.Quit();
        }
    }
}
