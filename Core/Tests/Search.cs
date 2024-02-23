using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace AutomationHulkApps.Core.Tests
{
    [TestFixture]
    internal class SearchTests
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
        
        // Test method to verify search results
        [Test]
        public void VerifySearchResults()
        {
            // Login with password
            EnterPasswordAndLogin("brauff");

            // Navigate to the homepage
            _driver.Navigate().GoToUrl("https://qa-practical-test.myshopify.com/");

            // Find and click on the search icon
            IWebElement searchIcon = _driver.FindElement(By.CssSelector(".header__icon--search"));
            searchIcon.Click();

            // Find the search input field after clicking the search icon and enter text
            IWebElement searchField = _driver.FindElement(By.CssSelector("#Search-In-Modal"));
            searchField.SendKeys("gold");

            // Find the search button based on class and click
            IWebElement searchButton = _driver.FindElement(By.CssSelector(".search__button"));
            searchButton.Click();

            // Check for the presence of at least one <li class="grid__item">
            IReadOnlyCollection<IWebElement> gridItems = _driver.FindElements(By.CssSelector("li.grid__item"));
            Assert.That(gridItems.Count > 0, Is.True, "No grid items found");

            // Print the number of grid items found
            Console.WriteLine($"Number of grid items found: {gridItems.Count}");
        }

        // Test method to verify search results with empty string
        [Test]
        public void VerifySearchResultsEmptyString()
        {
            // Login with password
            EnterPasswordAndLogin("brauff");

            // Save the initial URL before performing the search
            string initialUrl = _driver.Url;

            // Navigate to the homepage
            _driver.Navigate().GoToUrl("https://qa-practical-test.myshopify.com/");

            // Find and click on the search icon
            IWebElement searchIcon = _driver.FindElement(By.CssSelector(".header__icon--search"));
            searchIcon.Click();

            // Find the search input field after clicking the search icon and enter an empty string
            IWebElement searchField = _driver.FindElement(By.CssSelector("#Search-In-Modal"));
            searchField.SendKeys("");

            // Find the search button based on class and click
            IWebElement searchButton = _driver.FindElement(By.CssSelector(".search__button"));
            searchButton.Click();

            // Check if we remained on the same page
            Assert.That(_driver.Url, Is.EqualTo(initialUrl), "URLs do not match after empty search");
        }
        
        // Method to clean up after each test
        [TearDown]
        public void Cleanup()
        {
            _driver.Quit();
        }
    }
}

