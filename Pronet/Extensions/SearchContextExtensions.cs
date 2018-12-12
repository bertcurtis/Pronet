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
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Globalization;
    using System.Threading;
    using OpenQA.Selenium;

    /// <summary>
    /// Extensions methods for both IWebDriver and IWebElement
    /// </summary>
    public static class SearchContextExtensions
    {
        /// <summary>
        /// Converts generic IWebElement into specified web element (Checkbox, Table, etc.) .
        /// </summary>
        /// <typeparam name="T">Specified web element class</typeparam>
        /// <param name="webElement">Generic IWebElement.</param>
        /// <returns>
        /// Specified web element (Checkbox, Table, etc.)
        /// </returns>
        /// <exception cref="System.ArgumentNullException">When constructor is null.</exception>
        private static T As<T>(this IWebElement webElement)
            where T : class, IWebElement
        {
            var constructor = typeof(T).GetConstructor(new[] { typeof(IWebElement) });

            if (constructor != null)
            {
                return constructor.Invoke(new object[] { webElement }) as T;
            }

            throw new ArgumentNullException(string.Format(CultureInfo.CurrentCulture, "Constructor for type {0} is null.", typeof(T)));
        }

        public static bool ElementIsVisible(this IWebElement webElement)
        {
            try
            {
                return webElement.Displayed && webElement.Enabled && webElement.Size != Size.Empty;
            }
            catch
            {
                return false;
            }
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

        public static IWebElement WaitForElementToAppear(this IWebElement element, int timeoutInSeconds = 9)
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
        public static IList<IWebElement> WaitForElementsToAppear(this IList<IWebElement> elements, int timeoutInSeconds = 9)
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
        public static bool ElementsAreVisible(this IList<IWebElement> webElements)
        {
            try
            {
                foreach (var element in webElements)
                {
                    if (!element.Displayed || !element.Enabled || element.Location.IsEmpty)
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
    }
}
