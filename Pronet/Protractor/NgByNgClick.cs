
namespace Pronet.Protractor
{
    /// <summary>
    /// Wrapper around the NgBy.NgByNgClick() static method to provide typed By selector for FindsByAttribute usage.
    /// </summary>
    public class NgByNgClick : JavaScriptBy
    {
        /// <summary>
        /// Creates a new instance of <see cref="NgByNgClick"/>.
        /// </summary>
        /// <param name="clickFunction">The click function, e.g. '$ctrl.doThis()'.</param>
        public NgByNgClick(string clickFunction)
            : base(ClientSideScripts.FindNgClick, clickFunction)
        {
            base.Description = "NgBy.NgClick: " + clickFunction;
        }
    }
}
