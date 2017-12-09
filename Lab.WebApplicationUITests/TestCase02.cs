using System;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace Lab.WebApplicationUITests
{
    [TestClass]
    [DeploymentItem(@"chromedriver.exe")]
    public class TestCase02
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
            this.baseURL = "http://172.18.51.237/";
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
        public void TheCase02NewTaipeiTest()
        {
            driver.Navigate().GoToUrl(baseURL);

            new SelectElement(driver.FindElement(By.Id("Districts"))).SelectByText("新店區");
            new SelectElement(driver.FindElement(By.Id("Types"))).SelectByText("NewTaipei");
            new SelectElement(driver.FindElement(By.Id("Companys"))).SelectByText("中華電信");

            driver.FindElement(By.Id("ButtonSubmit")).Click();

            Assert.AreEqual("新北市立圖書館新店三民圖書閱覽室-1_A", driver.FindElement(By.CssSelector("td")).Text);
            Assert.IsTrue(IsElementPresent(By.LinkText("5")));

            driver.FindElement(By.LinkText("»»")).Click();
            Assert.IsTrue(this.driver.Url.Contains("page=5&districts=%E6%96%B0%E5%BA%97%E5%8D%80&types=NewTaipei&companys=%E4%B8%AD%E8%8F%AF%E9%9B%BB%E4%BF%A1"));

            driver.FindElement(By.LinkText("««")).Click();
            Assert.IsTrue(this.driver.Url.Contains("page=1&districts=%E6%96%B0%E5%BA%97%E5%8D%80&types=NewTaipei&companys=%E4%B8%AD%E8%8F%AF%E9%9B%BB%E4%BF%A1"));
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