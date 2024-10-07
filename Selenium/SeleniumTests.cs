using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using Xunit;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Selenium{
    public class BasePage {
        protected readonly IWebDriver _driver;
        protected readonly WebDriverWait _wait;

        public BasePage(IWebDriver driver) {
            _driver = driver;
            _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));  // Wait for up to 10 seconds
        }
    }

    public class SeleniumTests {
        [Fact]
        public void TestCompanyOverviewWorkflow() {
            var options = new ChromeOptions();
            options.AddArgument("start-maximized");
            using (IWebDriver driver = new ChromeDriver(options)) {
                //Create references
                driver.Navigate().GoToUrl("https://www.agdata.com");
                HomePage homePage = new HomePage(driver);
                CompanyPage companyPage = new CompanyPage(driver);
                ContactPage contactPage = new ContactPage(driver);

                //Start with navigating to "Company" page
                homePage.NavigateToOverview();

                //Fetch headings from "Our Values" section
                var valuesHeadings = companyPage.GetOurValuesHeadings();
                Assert.NotEmpty(valuesHeadings); // Ensure headings are found
                foreach (var heading in valuesHeadings) {
                    Console.WriteLine(heading);
                }

                //Click "Let's Get Started" button
                companyPage.ClickLetsGetStarted();

                //Assert Contact page is loaded
                Assert.True(contactPage.IsContactPageDisplayed());
            }
        }
    }

    public class HomePage : BasePage {
        public HomePage(IWebDriver driver) : base(driver) { }

        //Hover over "Company" and click "Overview"
        public void NavigateToOverview() {
            var companyMenu = _wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//nav//a[text()='Company']")));
            companyMenu.Click();

            var overviewLink = _wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//nav//a[text()='Overview']")));
            overviewLink.Click();

            //Verify Debug out
            Console.WriteLine("Completed HomePage");
        }
    }

    public class CompanyPage : BasePage {
        public CompanyPage(IWebDriver driver) : base(driver) { }

        //Get the headings of "Our Values" section and return the headings
        public List<string> GetOurValuesHeadings() {
            var ourValuesHeadings = By.CssSelector("h3");

            var headings = _driver.FindElements(ourValuesHeadings).Select(e => e.Text).ToList();

            //Dirty way to verify info, could be put into a log
            Console.WriteLine("Got Headings " + headings.ToString());

            return headings;
        }

        //Click on the Let's get Started button
        public void ClickLetsGetStarted() {
            var letsGetStartedButton = _wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//a[text()=\"Let's Get Started\"]")));
            letsGetStartedButton.Click();
            Console.WriteLine("Clicked Button");
        }
    }

    public class ContactPage : BasePage {
        public ContactPage(IWebDriver driver) : base(driver) { }

        public bool IsContactPageDisplayed() {
            //Verify that the Contact page is displayed by checking if the GET IN TOUCH WITH US text is visible AND the Gform is loaded ref by ID
            var getInTouch = _wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//h1[text()='GET IN TOUCH WITH US']")));
            var gform = _wait.Until(ExpectedConditions.ElementIsVisible(By.Id("gform_1")));

            //Verify 2 sections of the site
            bool bothLoaded = getInTouch.Displayed && gform.Displayed; 

            Console.WriteLine("Contact form is = " + bothLoaded);

            return bothLoaded;
        }
    }
}
