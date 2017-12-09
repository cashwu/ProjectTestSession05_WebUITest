using System;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace Lab.WebApplicationUITests
{
    [TestClass]
    [DeploymentItem(@"chromedriver.exe")]
    public class TestCase01
    {
        private IWebDriver driver;
        private StringBuilder verificationErrors;
        private string baseURL;
        private bool acceptNextAlert = true;

        [TestInitialize]
        public void SetupTest()
        {
            this.driver = new ChromeDriver();

            this.driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromMilliseconds(500));

            this.baseURL = "http://localhost:36747";
            this.verificationErrors = new StringBuilder();
        }

        [TestCleanup]
        public void TeardownTest()
        {
            try
            {
                this.driver.Quit();
            }
            catch (Exception)
            {
                // Ignore errors if unable to close the browser
            }
            Assert.AreEqual("", this.verificationErrors.ToString());
        }

        [TestMethod]
        public void TheCase01DefaultTest()
        {
            this.driver.Navigate().GoToUrl(this.baseURL);

            Assert.IsTrue(this.IsElementPresent(By.Id("Districts")));
            Assert.IsTrue(this.IsElementPresent(By.Id("Types")));
            Assert.IsTrue(this.IsElementPresent(By.Id("Companys")));

            Assert.IsTrue(this.IsElementPresent(By.LinkText("1")));
            Assert.IsTrue(this.IsElementPresent(By.LinkText("10")));
            
            this.driver.FindElement(By.LinkText("»»")).Click();
            Assert.IsTrue(this.IsElementPresent(By.LinkText("138")));
            Assert.AreEqual("147", this.driver.FindElement(By.LinkText("147")).Text);
            this.driver.FindElement(By.LinkText("««")).Click();
        }

        private bool IsElementPresent(By by)
        {
            try
            {
                this.driver.FindElement(by);
                return true;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }

        private bool IsAlertPresent()
        {
            try
            {
                this.driver.SwitchTo().Alert();
                return true;
            }
            catch (NoAlertPresentException)
            {
                return false;
            }
        }

        private string CloseAlertAndGetItsText()
        {
            try
            {
                IAlert alert = this.driver.SwitchTo().Alert();
                string alertText = alert.Text;
                if (this.acceptNextAlert)
                {
                    alert.Accept();
                }
                else
                {
                    alert.Dismiss();
                }
                return alertText;
            }
            finally
            {
                this.acceptNextAlert = true;
            }
        }
    }
}