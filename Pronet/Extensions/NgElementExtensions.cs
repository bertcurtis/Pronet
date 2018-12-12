// <copyright file="SearchContextExtensions.cs" company="Objectivity Bespoke Software Specialists">
// Copyright (c) Objectivity Bespoke Software Specialists. All rights reserved.
// </copyright>
// <license>
//     The MIT License (MIT)
//     Permission is hereby granted, free of charge, to any person obtaining a copy
//     of this software and associated documentation files (the "Software"), to deal
//     in the Software without restriction, including without limitation the rights
//     to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//     copies of the Software, and to permit persons to whom the Software is
//     furnished to do so, subject to the following conditions:
//     The above copyright notice and this permission notice shall be included in all
//     copies or substantial portions of the Software.
//     THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//     IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//     FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//     AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//     LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//     OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//     SOFTWARE.
// </license>

namespace Pronet.Extensions
{
    using Pronet.WebElements;
    using OpenQA.Selenium;
    using OpenQA.Selenium.Interactions;
    using OpenQA.Selenium.Support.UI;
    using Pronet.Protractor;
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Globalization;
    using System.Threading;

    /// <summary>
    /// Extensions methods for NgWebElement
    /// </summary>
    public static class WebElementExtensionsNg
    {
        public static void LocationClick(this NgWebElement webElement)
        {
            new Actions(webElement.NgDriver).MoveToElement(webElement, webElement.Location.X, webElement.Location.Y).Click().Perform();
        }

        public static void DoubleClick(this NgWebElement webElement)
        {
            webElement.NgDriver.WaitForAngular();
            new Actions(webElement.NgDriver).DoubleClick().Build().Perform();
        }

        public static bool IsElementTextEqualsToExpected(this NgWebElement webElement, string text)
        {
            return webElement.Text.Equals(text);
        }

        public static void JavaScriptClick(this NgWebElement webElement)
        {
            var javascript = webElement.NgDriver as IJavaScriptExecutor;
            if (javascript == null)
            {
                throw new ArgumentException("Element must wrap a web driver that supports javascript execution");
            }
            webElement.NgDriver.WaitForAngular();
            javascript.ExecuteScript("arguments[0].click();", webElement);
        }

        public static string GetTextContent(this NgWebElement webElement)
        {
            var javascript = webElement.NgDriver as IJavaScriptExecutor;
            if (javascript == null)
            {
                throw new ArgumentException("Element must wrap a web driver that supports javascript execution");
            }
            webElement.NgDriver.WaitForAngular();
            var textContent = (string)javascript.ExecuteScript("return arguments[0].textContent", webElement);
            return textContent;
        }

        public static void SetAttribute(this NgWebElement webElement, string attribute, string attributeValue)
        {
            var javascript = webElement.NgDriver as IJavaScriptExecutor;
            if (javascript == null)
            {
                throw new ArgumentException("Element must wrap a web driver that supports javascript execution");
            }
            webElement.NgDriver.WaitForAngular();
            javascript.ExecuteScript(
                "arguments[0].setAttribute(arguments[1], arguments[2])",
                webElement,
                attribute,
                attributeValue);
        }

        public static void ScrollToElement(this NgWebElement webElement)
        {
            var js = (IJavaScriptExecutor)webElement.NgDriver;
            if (webElement.NgDriver != null)
            {
                int height = webElement.NgDriver.Manage().Window.Size.Height;

                var hoverItem = (ILocatable)webElement;
                var locationY = hoverItem.LocationOnScreenOnceScrolledIntoView.Y;
                js.ExecuteScript(string.Format(CultureInfo.InvariantCulture, "javascript:window.scrollBy(0,{0})", locationY - (height / 2)));
            }
        }

        public static bool ElementIsVisible(this NgWebElement webElement)
        {
            try
            {
                var element = webElement.NgDriver.FindElement(webElement.By);
                if (!element.Displayed || !element.Enabled || element.Location.IsEmpty)
                {
                    return false;
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool ElementsAreVisible(this IList<NgWebElement> webElements)
        {
            try
            {
                foreach (var element in webElements)
                {
                    var webElement = element.NgDriver.FindElement(element.By);
                    if (!webElement.Displayed || !webElement.Enabled || webElement.Location.IsEmpty)
                    {
                        return false;
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static string WaitUntilElementIsInvisible(this NgWebElement webElement, double seconds = 120)
        {
            try
            {
                new WebDriverWait(webElement.NgDriver, TimeSpan.FromSeconds(seconds)).Until(ExpectedConditions.InvisibilityOfElementLocated(webElement.By));
                return "Element is now gone.";
            }
            catch(Exception e)
            {
                if (e is NoSuchElementException || e is NullReferenceException || e is ArgumentNullException)
                {
                    return e.Message + ": Element is already gone";
                }
                else
                {
                    throw e;
                }
            }
        }

        public static string WaitUntilElementsAreInvisible(this IList<NgWebElement> webElements, double seconds = 120)
        {
            foreach (var webElement in webElements)
            {
                try
                {
                    new WebDriverWait(webElement.NgDriver, TimeSpan.FromSeconds(seconds)).Until(ExpectedConditions.InvisibilityOfElementLocated(webElement.By));
                }
                catch (NoSuchElementException e)
                {
                    return e.Message + ": Elements were already gone.";
                }
            }
            return "Elements are now gone.";
        }

        public static void ClickBottomAreaOfElement(this NgWebElement element)
        {
            new Actions(element.NgDriver).MoveToElement(element).MoveByOffset(0, 20).Click().Perform();
        }

        public static Select Dropdown(this NgWebElement element)
        {
            return new Select(element, element.NgDriver);
        }

        public static void ClickAndDragToTargetLocation(this NgWebElement element, NgWebElement targetElement)
        {
            new Actions(element.NgDriver).ClickAndHold(element).MoveToElement(targetElement).Release().Perform();
        }

        public static NgWebElement HoldDownShiftKey(this NgWebElement element)
        {
            new Actions(element.NgDriver).KeyDown(Keys.Shift.ToString()).Perform();
            return element;
        }

        public static NgWebElement LetGoShiftKey(this NgWebElement element)
        {
            new Actions(element.NgDriver).KeyUp(Keys.Shift.ToString()).Perform();
            return element;
        }
        private static bool RetryTimeout(int retryCount, int timeoutInSeconds)
        {
            Thread.Sleep(1000);
            if (retryCount > timeoutInSeconds)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public static NgWebElement WaitForElementToAppear(this NgWebElement element, int timeoutInSeconds = 9)
        {
            int retryCount = 0;
            while (!element.ElementIsVisible())
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
            return element;
        }
        public static IList<NgWebElement> WaitForElementsToAppear(this IList<NgWebElement> elements, int timeoutInSeconds = 9)
        {
            int retryCount = 0;
            while (!elements.ElementsAreVisible())
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
            return elements;
        }

    }
}
