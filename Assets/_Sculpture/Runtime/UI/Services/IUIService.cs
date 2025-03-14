using System;

namespace _Sculpture.Runtime.UI.Services
{
    /// <summary>
    /// Interface for a UI service managing pages.
    /// </summary>
    public interface IUIService
    {
        /// <summary>
        /// Event that is raised when a page is opened.
        /// </summary>
        event Action<Enum, IOpenArguments> PageOpened;

        /// <summary>
        /// Event that is raised when a page is closed.
        /// </summary>
        event Action<Enum, ICloseArguments> PageClosed;

        /// <summary>
        /// Checks if the specified page is open.
        /// </summary>
        bool IsOpen(Enum pageIdentifier);

        /// <summary>
        /// Opens the specified page with the given arguments.
        /// </summary>
        /// <param name="pageIdentifier">The page to open.</param>
        /// <param name="arguments">Optional arguments for opening the page.</param>
        void Open(Enum pageIdentifier, IOpenArguments arguments = null);

        /// <summary>
        /// Reloads the specified page with the given arguments.
        /// </summary>
        /// <param name="pageIdentifier">The page to reload.</param>
        /// <param name="arguments">Optional arguments for reloading the page.</param>
        void Reload(Enum pageIdentifier, IReloadArguments arguments = null);

        /// <summary>
        /// Closes the specified page with the given arguments.
        /// </summary>
        /// <param name="pageIdentifier">The page to close.</param>
        /// <param name="arguments">Optional arguments for closing the page.</param>
        void Close(Enum pageIdentifier, ICloseArguments arguments = null);

        /// <summary>
        /// Links a UI page to a specific page type.
        /// </summary>
        /// <param name="page">The UI page to link.</param>
        /// <param name="pageIdentifier">The page identifier to link to.</param>
        void Link(IPage page, Enum pageIdentifier);

        /// <summary>
        /// Unlinks a specific page type.
        /// </summary>
        /// <param name="pageIdentifier">The page identifier to unlink.</param>
        void Unlink(Enum pageIdentifier);
    }
}