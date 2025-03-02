namespace _Sculpture.Runtime.UI
{
    /// <summary>
    /// Interface representing a UI page that can be opened and closed with specific arguments.
    /// </summary>
    public interface IPage
    {
        /// <summary>
        /// Opens the page with the specified arguments.
        /// </summary>
        /// <param name="arguments">The arguments used to open the page.</param>
        void Open(IOpenArguments arguments);

        /// <summary>
        /// Closes the page with the specified arguments.
        /// </summary>
        /// <param name="arguments">The arguments used to close the page.</param>
        void Close(ICloseArguments arguments);
    }
}