using System;
using OpenQA.Selenium;

namespace Pronet.Protractor
{
    /// <summary>
    /// Mechanism used to locate elements within Angular applications by binding, model, etc.
    /// </summary>
    public static class NgBy
    {
        /// <summary>
        /// Gets a mechanism to find elements by their Angular binding.
        /// </summary>
        /// <param name="binding">The binding, e.g. '{{cat.name}}'.</param>
        /// <returns>A <see cref="By"/> object the driver can use to find the elements.</returns>
        public static By Binding(string binding)
        {
            return new NgByBinding(binding);
        }

        /// <summary>
        /// Gets a mechanism to find elements by their Angular binding.
        /// </summary>
        /// <param name="binding">The binding, e.g. '{{cat.name}}'.</param>
        /// <returns>A <see cref="By"/> object the driver can use to find the elements.</returns>
        /// <param name="exactMatch">
        /// Indicates whether or not the binding needs to be matched exactly. By default false.
        /// </param>
        [Obsolete("Use 'ExactBinding' instead.")]
        public static By Binding(string binding, bool exactMatch)
        {
            return new NgByBinding(binding, exactMatch);
        }

        /// <summary>
        /// Gets a mechanism to find elements by their exact Angular binding.
        /// </summary>
        /// <param name="binding">The exact binding, e.g. '{{cat.name}}'.</param>
        /// <returns>A <see cref="By"/> object the driver can use to find the elements.</returns>
        public static By ExactBinding(string binding)
        {
            return new NgByExactBinding(binding);
        }

        /// <summary>
        /// Gets a mechanism to find elements by their model name.
        /// </summary>
        /// <param name="model">The model name.</param>
        /// <returns>A <see cref="By"/> object the driver can use to find the elements.</returns>
        public static By Model(string model)
        {
            return new NgByModel(model);
        }

        /// <summary>
        /// Gets a mechanism to find select option elements by their model name.
        /// </summary>
        /// <param name="model">The model name.</param>
        /// <returns>A <see cref="By"/> object the driver can use to find the elements.</returns>
        public static By SelectedOption(string model)
        {
            return new NgBySelectedOptions(model);
        }

        /// <summary>
        /// Gets a mechanism to find all rows of an ng-repeat.
        /// </summary>
        /// <param name="repeat">The text of the repeater, e.g. 'cat in cats'.</param>
        /// <returns>A <see cref="By"/> object the driver can use to find the elements.</returns>
        public static By Repeater(string repeat)
        {
            return new NgByRepeater(repeat);
        }

        /// <summary>
        /// Gets a mechanism to find all rows of an ng-repeat.
        /// </summary>
        /// <param name="repeat">The text of the repeater, e.g. 'cat in cats'.</param>
        /// <param name="exactMatch">
        /// Indicates whether or not the repeater needs to be matched exactly. By default, false.
        /// </param>
        /// <returns>A <see cref="By"/> object the driver can use to find the elements.</returns>
        [Obsolete("Use 'ExactRepeater' instead.")]
        public static By Repeater(string repeat, bool exactMatch)
        {
            return new NgByRepeater(repeat, exactMatch);
        }

        /// <summary>
        /// Gets a mechanism to find all rows of an ng-repeat.
        /// </summary>
        /// <param name="repeat">The exact text of the repeater, e.g. 'cat in cats'.</param>
        /// <returns>A <see cref="By"/> object the driver can use to find the elements.</returns>
        public static By ExactRepeater(string repeat)
        {
            return new NgByExactRepeater(repeat);
        }
        /// <summary>
        /// Gets a mechanism to find the element with an ngclick().
        /// </summary>
        /// <param name="clickFunction">The name of the ngclick function, e.g. 'doThis()'.</param>
        /// <returns>A <see cref="By"/> object the driver can use to find the elements.</returns>
        public static By NgClick(string clickFunction)
        {
            return new NgByNgClick(clickFunction);
        }
        /// <summary>
        /// Gets a mechanism to find the element with an an ng-class
        /// </summary>
        /// <param name="ngClassName">The name of the ng-class, e.g. ':: doThis()'.</param>
        /// <returns>A <see cref="By"/> object the driver can use to find the elements.</returns>
        public static By NgClass(string ngClassName)
        {
            return new NgByNgClick(ngClassName);
        }
        /// <summary>
        /// Gets a mechanism to find the element with an ngdoubleclick().
        /// </summary>
        /// <param name="doubleClickFunction">The name of the doubleclick function, e.g. '$ctrl.dothis()'.</param>
        /// <returns>A <see cref="By"/> object the driver can use to find the elements.</returns>
        public static By NgDoubleClick(string doubleClickFunction)
        {
            return new NgByNgDoubleClick(doubleClickFunction);
        }
        /// <summary>
        /// Gets a mechanism to find the element with an attribute and a value.
        /// </summary>' <param name="selector">The string of the selector, e.g. custom-attribute="value" </param>
        /// <returns>A <see cref="By"/> object the driver can use to find the elements.</returns>
        public static By NgCustomSelector(string selector)
        {
            return new NgByCustomSelector(selector);
        }
        /// <summary>
        /// Gets a mechanism to find the element with a specific button text.
        /// </summary>
        /// <param name="buttonText">The exact text of the button, e.g. 'get started'.</param>
        /// <returns>A <see cref="By"/> object the driver can use to find the elements.</returns>
        public static By ButtonText(string buttonText)
        {
            return new NgByButtonText(buttonText);
        }
        /// <summary>
        /// Gets a mechanism to find the element with a specific ng-bind value.
        /// </summary>
        /// <param name="bindValue">The value of the ng-bind, e.g. '::'web.' bindValue'.</param>
        /// <returns>A <see cref="By"/> object the driver can use to find the elements.</returns>
        public static By NgBind(string bindValue)
        {
            return new NgByNgBind(bindValue);
        }
    }
}