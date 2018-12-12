using System;
using Pronet.Protractor;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System.Linq;

namespace Pronet.Extensions
{
    public static class DriverExtensions
    {
        public static void WaitUntilClickableBy(this NgWebDriver driver, By finder, bool isWrappedDriver = false)
        {
            if (isWrappedDriver)
            {
                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(BaseConfiguration.MediumTimeout));
                wait.IgnoreExceptionTypes(typeof(StaleElementReferenceException), typeof(NoSuchElementException));
                wait.Until(ExpectedConditions.ElementToBeClickable(finder));
            }
            else
            {
                var wait = new WebDriverWait(driver.WrappedDriver, TimeSpan.FromSeconds(BaseConfiguration.MediumTimeout));
                wait.IgnoreExceptionTypes(typeof(StaleElementReferenceException), typeof(NoSuchElementException));
                wait.Until(ExpectedConditions.ElementToBeClickable(finder));
            }
        }
        public static IWebDriver SwitchToLastWindowHandle(this NgWebDriver driver)
        {
            return driver.SwitchTo().Window(driver.WindowHandles.Last());
        }

        public static void SwitchToActiveElement(this NgWebDriver driver)
        {
            driver.SwitchTo().ActiveElement();
        }

        public static void BrowserAccept(this NgWebDriver driver, bool isWrapped = false)
        {
            if (isWrapped)
            {
                driver.WrappedDriver.SwitchTo().ActiveElement().SendKeys(Keys.Return);
            }
            else
            {
                driver.SwitchTo().ActiveElement().SendKeys(Keys.Return);
            }

        }

        public static void BrowserRefresh(this NgWebDriver driver)
        {
            driver.Navigate().Refresh();
        }

        public static bool CheckIfElementExists(this NgWebDriver driver, By finder)
        {
            try
            {
                driver.FindElement(finder);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
