
namespace Pronet.Protractor
{
    /// <summary>
    /// Wrapper around the NgBy.NgByButtonText() static method to provide typed By selector for FindsByAttribute usage.
    /// </summary>
    public class NgByButtonText : JavaScriptBy
    {
        /// <summary>
        /// Creates a new instance of <see cref="NgByButtonText"/>.
        /// </summary>
        /// <param name="buttonText">The button text, e.g. 'Get Started'.</param>
        public NgByButtonText(string buttonText)
            : base(ClientSideScripts.FindButtonText, buttonText)
        {
            base.Description = "NgBy.ButtonText: " + buttonText;
        }
    }
}
