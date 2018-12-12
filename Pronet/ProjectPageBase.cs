using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Pronet.Protractor;
using System;
using System.Globalization;
using System.Linq;

using NLog;

using Pronet.Extensions;
using OpenQA.Selenium.Interactions;
using Pronet.WebElements;
using Pronet.Helpers;
using Pronet.Mongo;
using Pronet.Fiddler;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading;

namespace Pronet.Driver
{
    public class ProjectPageBase : ProjectTestBase
    {
        public ProjectPageBase()
        {
            NgDriver = DriverContext.NgDriver;
            NgDriver.Manage().Timeouts().AsynchronousJavaScript = TimeSpan.FromSeconds(15);
            Mongo = DriverContext.Mongo;
            //Fiddler = DriverContext.Fiddler;
        }
        public NgWebDriver NgDriver { get; set; }
        public MongoDriver Mongo { get; set; }
        //public FiddlerProxy Fiddler { get; set; }

        private static readonly ILogger Logger = LogManager.GetLogger("DRIVER");

        #region
        //Driver logic


        public void WaitUntilClickableBy(By finder, bool isWrappedDriver = false)
        {
            if (isWrappedDriver)
            {
                var wait = new WebDriverWait(NgDriver, TimeSpan.FromSeconds(BaseConfiguration.MediumTimeout));
                wait.IgnoreExceptionTypes(typeof(StaleElementReferenceException), typeof(NoSuchElementException));
                wait.Until(ExpectedConditions.ElementToBeClickable(finder));
            }
            else
            {
                var wait = new WebDriverWait(NgDriver.WrappedDriver, TimeSpan.FromSeconds(BaseConfiguration.MediumTimeout));
                wait.IgnoreExceptionTypes(typeof(StaleElementReferenceException), typeof(NoSuchElementException));
                wait.Until(ExpectedConditions.ElementToBeClickable(finder));
            }
        }
        public IWebDriver SwitchToLastWindowHandle()
        {
            return NgDriver.SwitchTo().Window(NgDriver.WindowHandles.Last());
        }

        public void SwitchToActiveElement()
        {
            NgDriver.SwitchTo().ActiveElement();
        }

        public void BrowserAccept(bool isWrapped = false)
        {
            if (isWrapped)
            {
                NgDriver.WrappedDriver.SwitchTo().ActiveElement().SendKeys(Keys.Return);
            }
            else
            {
                NgDriver.SwitchTo().ActiveElement().SendKeys(Keys.Return);
            }

        }

        public void BrowserRefresh()
        {
            NgDriver.Navigate().Refresh();
        }

        public bool CheckIfElementExists(By finder)
        {
            try
            {
                NgDriver.FindElement(finder);
                return true;
            }
            catch
            {
                return false;
            }
        }


        /// <summary>
        /// Determines whether [is page title] equals [the specified page title].
        /// </summary>
        /// <example>Sample code to check page title: <code>
        /// this.Driver.IsPageTitle(expectedPageTitle, BaseConfiguration.MediumTimeout);
        /// </code></example>
        /// <param name="pageTitle">The page title.</param>
        /// <returns>
        /// Returns title of page
        /// </returns>
        public bool IsPageTitle(string pageTitle)
        {
            var wait = new WebDriverWait(NgDriver, TimeSpan.FromSeconds(BaseConfiguration.ShortTimeout));

            try
            {
                wait.Until(ExpectedConditions.TitleIs(pageTitle));
            }
            catch (WebDriverTimeoutException)
            {
                Logger.Error(CultureInfo.CurrentCulture, "Actual page title is {0};", NgDriver.Title);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Navigates to.
        /// </summary>
        /// <param name="url">The URL.</param>
        public void NavigateTo(Uri url)
        {
            NgDriver.Navigate().GoToUrl(url);
            ApproveCertificateForInternetExplorer();
        }

        /// <summary>
        /// Navigates to.
        /// </summary>
        /// <param name="url">The URL.</param>
        public void NavigateTo(string url)
        {
            NgDriver.Navigate().GoToUrl(url);
            ApproveCertificateForInternetExplorer();
        }

        /// <summary>
        /// Approves the cert for IE if that's the browser that's being used
        /// </summary>
        private void ApproveCertificateForInternetExplorer()
        {
            if (BaseConfiguration.TestBrowser.Equals(BrowserType.InternetExplorer) && NgDriver.Title.Contains("Certificate"))
            {
                By by = By.Id("overridelink");
                var element = new NgWebElement(NgDriver, NgDriver.FindElement(by), by);
                element.JavaScriptClick();
            }
        }

        /// <summary>
        /// Enable a secondary synchronization with angular.
        /// </summary>
        /// <param name="enable">Enable or disable synchronization.</param>
        public void AdditionalSynchronizationWithAngular(bool enable)
        {
            DriversCustomSettings.SetAngularSynchronizationForDriver(NgDriver, enable);
        }

        /// <summary>
        /// A secondary wait for all angular actions to be completed.
        /// </summary>
        public void AdditionalWaitForAngular()
        {
            AdditionalWaitForAngular(BaseConfiguration.MediumTimeout);
        }

        /// <summary>
        /// A secondary wait for all angular actions to be completed.
        /// </summary>
        /// /// <param name="timeout">The timeout.</param>
        public void AdditionalWaitForAngular(double timeout)
        {
            try
            {
                new WebDriverWait(NgDriver, TimeSpan.FromSeconds(timeout)).Until(
                    driver =>
                    {
                        var javaScriptExecutor = driver as IJavaScriptExecutor;
                        return javaScriptExecutor != null
                               &&
                               (bool)javaScriptExecutor.ExecuteScript(
                                   "return window.angular != undefined && window.angular.element(document.body).injector().get('$http').pendingRequests.length == 0");
                    });
            }
            catch (InvalidOperationException)
            {
                Logger.Info("Wait for angular invalid operation exception.");
            }
        }

        /// <summary>Easy use for java scripts.</summary>
        /// <example>Sample use of java scripts: <code>
        /// ExecuteCustomJavaScript("return document.getElementById("demo").innerHTML");
        /// </code></example>
        /// <returns>An IJavaScriptExecutor Handle.</returns>
        public IJavaScriptExecutor ExecuteCustomJavaScript(string script)
        {
            return (IJavaScriptExecutor)NgDriver.ExecuteScript(script);
        }

        /// <summary>Checks that page source contains text for a specified wait time.</summary>
        /// <param name="text">The text.</param>
        /// <returns>true if it succeeds, false if it fails.</returns>
        public bool CheckIfPageSourceContainsText(string text)
        {
            Func<IWebDriver, bool> condition;
            condition = drv => drv.PageSource.ToUpperInvariant().Contains(text.ToUpperInvariant());

            var wait = new WebDriverWait(NgDriver, TimeSpan.FromSeconds(BaseConfiguration.MediumTimeout));
            wait.Until(condition);

            return condition.Invoke(NgDriver);
        }

        /// <summary>
        /// Selenium Actions.
        /// </summary>
        /// <example>Simple use of Actions: <code>
        /// DriverActionToPerform().SendKeys(Keys.Return).Perform();
        /// </code></example>
        /// <returns>Return new Action handle</returns>
        public Actions DriverActionToPerform()
        {
            return new Actions(NgDriver);
        }

        /// <summary>
        /// Switch to existing window using url.
        /// </summary>
        /// <param name="url">The url.</param>
        /// <param name="usingWrappedDriver">Optional parameter to use the wrapped instance of the IWebDriver.</param>
        public void SwitchToWindowUsingUrl(Uri url, bool usingWrappedDriver = false)
        {
            if (usingWrappedDriver)
            {
                SwitchToWindowUsingUrl(url, NgDriver.WrappedDriver);
            }
            else
            {
                SwitchToWindowUsingUrl(url, NgDriver);
            }
        }

        /// <summary>
        /// Private method used locally.
        /// </summary>
        /// <param name="url">The url.</param>
        /// <param name="webDriver">The IWebDriver.</param>
        private void SwitchToWindowUsingUrl(Uri url, IWebDriver webDriver)
        {
            var wait = new WebDriverWait(webDriver, TimeSpan.FromSeconds(BaseConfiguration.MediumTimeout));
            wait.Until(
                driver =>
                {
                    foreach (var handle in webDriver.WindowHandles)
                    {
                        webDriver.SwitchTo().Window(handle);
                        if (driver.Url.Equals(url.ToString()))
                        {
                            return true;
                        }
                    }

                    return false;
                });
        }

        /// <summary>
        /// Handler for simple use JavaScriptAlert.
        /// </summary>
        /// <example>Sample confirmation for java script alert: <code>
        /// JavaScriptAlert().ConfirmJavaScriptAlert();
        /// </code></example>
        /// <returns>JavaScriptAlert Handle</returns>
        public JavaScriptAlert JavaScriptAlert()
        {
            return new JavaScriptAlert(NgDriver);
        }

        /// <summary>
        /// Navigates to given url and measure time for this action including or not Ajax.
        /// </summary>
        /// <example>Sample confirmation for java script alert: <code>
        /// this.Driver.NavigateToAndMeasureTime("http://objectivity.co.uk", waitForAjax: true);
        /// </code></example>
        /// <param name="url">The URL.</param>
        /// <param name="waitForAjax">Wait or not for Ajax</param>
        public long NavigateToAndMeasureTime(Uri url, bool waitForAjax = false)
        {
            PerformanceHelper.Instance.StartMeasure();
            NgDriver.Navigate().GoToUrl(url);
            if (waitForAjax)
            {
                WaitForAjax();
            }

            PerformanceHelper.Instance.StopMeasure();
            return PerformanceHelper.Instance.GetLoadTimeList[PerformanceHelper.Instance.GetLoadTimeList.Count - 1].Duration;
        }

        /// <summary>
        /// Waits for all ajax actions to be completed.
        /// </summary>
        public void WaitForAjax()
        {
            WaitForAjax(BaseConfiguration.MediumTimeout);
        }

        /// <summary>
        /// Waits for all ajax actions to be completed.
        /// </summary>
        /// <param name="timeout">The timeout.</param>
        public void WaitForAjax(double timeout)
        {
            try
            {
                new WebDriverWait(NgDriver, TimeSpan.FromSeconds(timeout)).Until(
                    driver =>
                    {
                        var javaScriptExecutor = driver as IJavaScriptExecutor;
                        return javaScriptExecutor != null
                               && (bool)javaScriptExecutor.ExecuteScript("return jQuery.active == 0");
                    });
            }
            catch (InvalidOperationException)
            {
            }
        }
        private NgWebElement NgElement { get; set; }
        public bool SetElement(By by)
        {
            try
            {
                NgElement = new NgWebElement(NgDriver, NgDriver.FindElement(by), by);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public NgWebElement GetElement(By by)
        {
            int count = 0;
            while (!SetElement(by))
            {
                Thread.Sleep(1000);
                count++;
                if (count > 60)
                {
                    throw new ElementNotVisibleException("Could not locate element in 60 seconds");
                }
            }
            return NgElement;
        }

        private IList<NgWebElement> NgElements { get; set; }
        public bool SetElements(By by)
        {
            try
            {
                NgElements = new List<NgWebElement>();
                foreach (var element in NgDriver.FindElements(by))
                {
                    NgElements.Add(new NgWebElement(NgDriver, element, by));
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        public IList<NgWebElement> GetElements(By by)
        {
            int count = 0;
            while (!SetElements(by))
            {
                Thread.Sleep(1000);
                count++;
                if (count > 60)
                {
                    throw new ElementNotVisibleException("Could not locate element in 60 seconds");
                }
            }
            return NgElements;
        }
        public void WaitForElementToDie(By by, int timeoutInSeconds = 9)
        {
            int retryCount = 0;
            while (SetElement(by))
            {
                if (RetryTimeout(retryCount, timeoutInSeconds))
                {
                    retryCount++;
                }
                else
                {
                    break;
                }
            }
        }
        public bool RetryTimeout(int retryCount, int timeoutInSeconds = 9)
        {
            Thread.Sleep(1000);
            if (retryCount > timeoutInSeconds)
            {
                Logger.Error("Evaluation of method never resolved in the designated timeout time" +
                    " of " + timeoutInSeconds.ToString());
                return false;
            }
            else
            {
                return true;
            }
        }
        #endregion

    }
   
}
