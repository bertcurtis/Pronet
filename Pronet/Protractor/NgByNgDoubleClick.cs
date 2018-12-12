
namespace Pronet.Protractor
{
    /// <summary>
    /// Wrapper around the NgBy.NgByNgDoubleClick() static method to provide typed By selector for FindsByAttribute usage.
    /// </summary>
    public class NgByNgDoubleClick : JavaScriptBy
    {
        /// <summary>
        /// Creates a new instance of <see cref="NgByNgDoubleClick"/>.
        /// </summary>
        /// <param name="dblClickFunction">The double click function, e.g. '$ctrl.showOptionAlert()'.</param>
        public NgByNgDoubleClick(string dblClickFunction)
            : base(ClientSideScripts.FindNgDoubleClick, dblClickFunction)
        {
            base.Description = "NgBy.NgDoubleClick: " + dblClickFunction;
        }
    }
}
