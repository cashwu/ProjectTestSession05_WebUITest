using System;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace Lab.WebApplicationUITests
{
    [TestClass]
    public class TestCase03
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
                driver.Quit();
            }
            catch (Exception)
            {
                // Ignore errors if unable to close the browser
            }
            Assert.AreEqual("", verificationErrors.ToString());
        }

        [TestMethod]
        public void TheCase03Test()
        {
            driver.Navigate().GoToUrl(baseURL);
            new SelectElement(driver.FindElement(By.Id("Companys"))).SelectByText("磐石寬頻");
            driver.FindElement(By.Id("ButtonSubmit")).Click();
            Assert.AreEqual("水金九-九份(商圈辦公室)", driver.FindElement(By.CssSelector("td")).Text);
        }
        private bool IsElementPresent(By by)
        {
            try
            {
                driver.FindElement(by);
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
                driver.SwitchTo().Alert();
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
                IAlert alert = driver.SwitchTo().Alert();
                string alertText = alert.Text;
                if (acceptNextAlert)
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
                acceptNextAlert = true;
            }
        }
    }
}
