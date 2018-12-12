namespace Pronet.Protractor
{
    /// <summary>
    /// Wrapper around the NgBy.SelectedOption() static method to provide typed By selector for FindsByAttribute usage.
    /// </summary>
    public class NgBySelectedOptions : JavaScriptBy
    {
        /// <summary>
        /// Creates a new instance of <see cref="NgBySelectedOptions"/>.
        /// </summary>
        /// <param name="model">The model name.</param>
        public NgBySelectedOptions(string model)
            : base(ClientSideScripts.FindSelectedOptions, model)
        {
            base.Description = "NgBy.SelectedOption: " + model;
        }
    }
}
