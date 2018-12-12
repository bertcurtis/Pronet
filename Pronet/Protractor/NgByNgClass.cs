namespace Pronet.Protractor
{
    /// <summary>
    /// Wrapper around the NgBy.NgByNgClick() static method to provide typed By selector for FindsByAttribute usage.
    /// </summary>
    public class NgByNgClass : JavaScriptBy
    {
        /// <summary>
        /// Creates a new instance of <see cref="NgByNgClick"/>.
        /// </summary>
        /// <param name="className">The class name, e.g. ':: $ctrl.doThis()'.</param>
        public NgByNgClass(string className)
            : base(ClientSideScripts.FindNgClass, className)
        {
            base.Description = "NgBy.NgClass: " + className;
        }
    }
}
