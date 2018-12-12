namespace Pronet.Protractor
{
    /// <summary>
    /// Wrapper around the NgBy.NgByNgClick() static method to provide typed By selector for FindsByAttribute usage.
    /// </summary>
    public class NgByNgBind : JavaScriptBy
    {
        /// <summary>
        /// Creates a new instance of <see cref="NgByNgBind"/>.
        /// </summary>
        /// <param name="bindValue">The bind, e.g. ':: 'web.Apply' | appString'.</param>
        public NgByNgBind(string bindValue)
            : base(ClientSideScripts.FindNgBind, bindValue)
        {
            base.Description = "NgBy.NgBind: " + bindValue;
        }
    }
}