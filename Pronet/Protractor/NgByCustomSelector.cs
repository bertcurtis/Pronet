namespace Pronet.Protractor
{
    /// <summary>
    /// Wrapper around the NgBy.NgByCustomSelector() static method to provide typed By selector for FindsByAttribute usage.
    /// </summary>
    public class NgByCustomSelector : JavaScriptBy
    {
        /// <summary>
        /// Creates a new instance of <see cref="NgByNgClick"/>.
        /// </summary>
        /// <param name="selector">The selector string, e.g. 'custom-attrbiute="valueOfAttribute"'.</param>
        public NgByCustomSelector(string selector)
            : base(ClientSideScripts.FindCustomSelector, selector)
        {
            base.Description = "NgBy.CustomSelector: " + selector;
        }
    }
}