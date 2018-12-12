using System.Linq;
using System.Drawing;
using System.Collections.ObjectModel;

using OpenQA.Selenium;
using OpenQA.Selenium.Internal;
using System;
using System.Threading;

namespace Pronet.Protractor
{
    /// <summary>
    /// Provides a mechanism to get elements off the page for test.
    /// </summary>
    public class NgWebElement : IWebElement, IWrapsElement
    {
        private NgWebDriver ngDriver;
        private IWebElement element;
        private By by;

        /// <summary>
        /// Creates a new instance of <see cref="NgWebElement"/> by wrapping a <see cref="IWebElement"/> instance.
        /// </summary>
        /// <param name="ngDriver">The <see cref="NgWebDriver"/> in use.</param>
        /// <param name="element">The existing <see cref="IWebElement"/> instance.</param>
        /// <param name="by">The <see cref="By"/> being used to find the element.</param>
        public NgWebElement(NgWebDriver ngDriver, IWebElement element, By by)
        {
            this.ngDriver = ngDriver;
            this.element = element;
            this.by = by;
        }

        #region IWrapsElement Members

        /// <summary>
        /// Gets the wrapped <see cref="IWebElement"/> instance.
        /// </summary>
        public IWebElement WrappedElement
        {
            get { return this.element; }
        }

        #endregion

        #region IWebElement Members

        /// <summary>
        /// Gets a value indicating whether or not this element is displayed.
        /// </summary>
        public bool Displayed
        {
            get 
            {
                this.ngDriver.WaitForAngular();
                return this.element.Displayed;
            }
        }

        /// <summary>
        /// Gets a value indicating whether or not this element is enabled.
        /// </summary>
        public bool Enabled
        {
            get
            {
                this.ngDriver.WaitForAngular();
                return this.element.Enabled;
            }
        }

        /// <summary>
        /// Gets a <see cref="Point"/> object containing the coordinates of the upper-left corner
        /// of this element relative to the upper-left corner of the page.
        /// </summary>
        public Point Location
        {
            get
            {
                this.ngDriver.WaitForAngular();
                return this.element.Location;
            }
        }

        /// <summary>
        /// Gets a value indicating whether or not this element is selected.
        /// </summary>
        public bool Selected
        {
            get
            {
                this.ngDriver.WaitForAngular();
                return this.element.Selected;
            }
        }

        /// <summary>
        /// Gets a <see cref="Size"/> object containing the height and width of this element.
        /// </summary>
        public Size Size
        {
            get
            {
                this.ngDriver.WaitForAngular();
                return this.element.Size;
            }
        }

        /// <summary>
        /// Gets the tag name of this element.
        /// </summary>
        public string TagName
        {
            get
            {
                this.ngDriver.WaitForAngular();
                return this.element.TagName;
            }
        }

        /// <summary>
        /// Gets the innerText of this element, without any leading or trailing whitespace,
        /// and with other whitespace collapsed.
        /// </summary>
        public string Text
        {
            get
            {
                this.ngDriver.WaitForAngular();
                return this.element.Text;
            }
        }

        /// <summary>
        /// Clears the content of this element.
        /// </summary>
        public void Clear()
        {
            this.ngDriver.WaitForAngular();
            this.ngDriver.WaitForDom();
            this.element.Clear();
        }
        /// <summary>
        /// Clicks this element. 
        /// </summary>
        public void Click()
        {
            this.ngDriver.WaitForAngular();
            this.ngDriver.WaitForDom();
            try
            {
                this.element.Click();
            }
            catch(WebDriverException e)
            {
                if(e.Message.ToUpper().Contains("Other element would receive the click".ToUpper()))
                {
                    Thread.Sleep(3000);
                    this.element.Click();
                }
            }
        }

        /// <summary>
        /// Gets the value of the specified attribute for this element.
        /// </summary>
        public string GetAttribute(string attributeName)
        {
            this.ngDriver.WaitForAngular();
            return this.element.GetAttribute(attributeName);
        }

        /// <summary>
        /// Gets the value of the specified property for this element.
        /// </summary>
        public string GetProperty(string propertyName)
        {
            this.ngDriver.WaitForAngular();
            this.ngDriver.WaitForDom();
            return this.element.GetProperty(propertyName);
        }

        /// <summary>
        /// Gets the value of a CSS property of this element.
        /// </summary>
        public string GetCssValue(string propertyName)
        {
            this.ngDriver.WaitForAngular();
            this.ngDriver.WaitForDom();
            return this.element.GetCssValue(propertyName);
        }

        /// <summary>
        /// Simulates typing text into the element.
        /// </summary>
        public void SendKeys(string text)
        {
            this.ngDriver.WaitForAngular();
            this.ngDriver.WaitForDom();
            this.element.SendKeys(text);
        }

        /// <summary>
        /// Submits this element to the web server.
        /// </summary>
        public void Submit()
        {
            this.ngDriver.WaitForAngular();
            this.ngDriver.WaitForDom();
            this.element.Submit();
        }

        /// <summary>
        /// Finds the first <see cref="NgWebElement"/> using the given mechanism. 
        /// </summary>
        /// <param name="by">The locating mechanism to use.</param>
        /// <returns>The first matching <see cref="NgWebElement"/> on the current context.</returns>
        /// <exception cref="NoSuchElementException">If no element matches the criteria.</exception>
        public NgWebElement FindElement(By by)
        {
            if (by is JavaScriptBy)
            {
                ((JavaScriptBy)by).AdditionalScriptArguments = new object[] { this.ngDriver.RootElement, this.element };
            }
            this.ngDriver.WaitForAngular();
            return new NgWebElement(this.ngDriver, this.element.FindElement(by), by);
        }

        /// <summary>
        /// Overload for FindElement. Tries the first by method and if it can't find anything with that 
        /// <see cref="By"/> then it tries the second one
        /// </summary>
        /// <param name="by">The first locating mechanism to use.</param>
        /// <param name="altBy">The second locating mechanism to use.</param>
        /// <returns>The first matching <see cref="NgWebElement"/> on the current context.</returns>
        /// <exception cref="NoSuchElementException">If no element matches the criteria.</exception>
        public NgWebElement FindElement(By by, By altBy)
        {
            if (by is JavaScriptBy)
            {
                ((JavaScriptBy)by).AdditionalScriptArguments = new object[] { this.ngDriver.RootElement, this.element };
            }
            if (altBy is JavaScriptBy)
            {
                ((JavaScriptBy)altBy).AdditionalScriptArguments = new object[] { this.ngDriver.RootElement, this.element };
            }
            this.ngDriver.WaitForAngular();
            try
            {
                return new NgWebElement(this.ngDriver, this.element.FindElement(by), by);
            }
            catch
            {
                try
                {
                    return new NgWebElement(this.ngDriver, this.element.FindElement(altBy), altBy);
                }
                catch
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Finds all <see cref="NgWebElement"/>s within the current context 
        /// using the given mechanism.
        /// </summary>
        /// <param name="by">The locating mechanism to use.</param>
        /// <returns>
        /// A <see cref="ReadOnlyCollection{T}"/> of all <see cref="NgWebElement"/>s 
        /// matching the current criteria, or an empty list if nothing matches.
        /// </returns>
        public ReadOnlyCollection<NgWebElement> FindElements(By by)
        {
            if (by is JavaScriptBy)
            {
                ((JavaScriptBy)by).AdditionalScriptArguments = new object[] { this.ngDriver.RootElement, this.element };
            }
            this.ngDriver.WaitForAngular();
            return new ReadOnlyCollection<NgWebElement>(this.element.FindElements(by).Select(e => new NgWebElement(this.ngDriver, e, by)).ToList());
        }

        /// <summary>
        /// Finds all <see cref="NgWebElement"/>s within the current context 
        /// using the given mechanism by trying 2 different by mechanisms
        /// </summary>
        /// <param name="by">The first locating mechanism to use.</param>
        /// <param name="altBy">The second locating mechanism to use.</param>
        /// <returns>
        /// A <see cref="ReadOnlyCollection{T}"/> of all <see cref="NgWebElement"/>s 
        /// matching the current criteria, or an empty list if nothing matches.
        /// </returns>
        public ReadOnlyCollection<NgWebElement> FindElements(By by, By altBy)
        {
            if (by is JavaScriptBy)
            {
                ((JavaScriptBy)by).AdditionalScriptArguments = new object[] { this.ngDriver.RootElement, this.element };
            }
            if (altBy is JavaScriptBy)
            {
                ((JavaScriptBy)altBy).AdditionalScriptArguments = new object[] { this.ngDriver.RootElement, this.element };
            }
            this.ngDriver.WaitForAngular();
            try
            {
                return new ReadOnlyCollection<NgWebElement>(this.element.FindElements(by).Select(e => new NgWebElement(this.ngDriver, e, by)).ToList());
            }
            catch
            {
                try
                {
                    return new ReadOnlyCollection<NgWebElement>(this.element.FindElements(altBy).Select(e => new NgWebElement(this.ngDriver, e, altBy)).ToList());
                }
                catch
                {
                    throw;
                }
            }
            
        }

        IWebElement ISearchContext.FindElement(By by)
        {
            return this.FindElement(by);
        }

        ReadOnlyCollection<IWebElement> ISearchContext.FindElements(By by)
        {
            if (by is JavaScriptBy)
            {
                ((JavaScriptBy)by).AdditionalScriptArguments = new object[] { this.ngDriver.RootElement, this.element };
            }
            this.ngDriver.WaitForAngular();
            return new ReadOnlyCollection<IWebElement>(this.element.FindElements(by).Select(e => (IWebElement)new NgWebElement(this.ngDriver, e, by)).ToList());
        }

        #endregion

        /// <summary>
        /// Evaluates the expression as if it were on the scope of the current element.
        /// </summary>
        /// <param name="expression">The expression to evaluate.</param>
        /// <returns>The expression evaluated by Angular.</returns>
        public object Evaluate(string expression)
        {
            this.ngDriver.WaitForAngular();
            return this.ngDriver.ExecuteScript(ClientSideScripts.Evaluate, this.element, expression);
        }
        /// <summary>
        /// Exposes the used NgDriver
        /// </summary>
        /// <returns>The NgDriver used to initialize the element.</returns>
        public NgWebDriver NgDriver
        {
            get
            {
                return this.ngDriver;
            }
        }

        /// <summary>
        /// Exposes the used by locator
        /// </summary>
        public By By
        {
            get
            {
                return this.by;
            }
        }
    }
}
