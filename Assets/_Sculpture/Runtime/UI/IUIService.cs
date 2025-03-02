using System;

namespace _Sculpture.Runtime.UI
{
    /// <summary>
    /// Interface for a UI service managing pages.
    /// </summary>
    public interface IUIService
    {
        /// <summary>
        /// Opens the specified page with the given arguments.
        /// </summary>
        /// <typeparam name="TPageType">The type of the page, which must be an enumeration.</typeparam>
        /// <param name="pageType">The page to open.</param>
        /// <param name="arguments">Optional arguments for opening the page.</param>
        void Open<TPageType>(TPageType pageType, IOpenArguments arguments = null) where TPageType : Enum;

        /// <summary>
        /// Opens the specified page with the given arguments.
        /// </summary>
        /// <param name="pageType">The page to open.</param>
        /// <param name="arguments">Optional arguments for opening the page.</param>
        void Open(Enum pageType, IOpenArguments arguments = null);

        /// <summary>
        /// Closes the specified page with the given arguments.
        /// </summary>
        /// <typeparam name="TPageType">The type of the page, which must be an enumeration.</typeparam>
        /// <param name="pageType">The page to close.</param>
        /// <param name="arguments">Optional arguments for closing the page.</param>
        void Close<TPageType>(TPageType pageType, ICloseArguments arguments = null) where TPageType : Enum;

        /// <summary>
        /// Closes the specified page with the given arguments.
        /// </summary>
        /// <param name="pageType">The page to close.</param>
        /// <param name="arguments">Optional arguments for closing the page.</param>
        void Close(Enum pageType, ICloseArguments arguments = null);

        /// <summary>
        /// Links a UI page to a specific page type.
        /// </summary>
        /// <typeparam name="TPageType">The type of the page, which must be an enumeration.</typeparam>
        /// <param name="page">The UI page to link.</param>
        /// <param name="pageType">The page type to link to.</param>
        void Link<TPageType>(IPage page, TPageType pageType) where TPageType : Enum;

        /// <summary>
        /// Links a UI page to a specific page type.
        /// </summary>
        /// <param name="page">The UI page to link.</param>
        /// <param name="pageType">The page type to link to.</param>
        void Link(IPage page, Enum pageType);

        /// <summary>
        /// Unlinks a specific page type.
        /// </summary>
        /// <typeparam name="TPageType">The type of the page, which must be an enumeration.</typeparam>
        /// <param name="pageType">The page type to unlink.</param>
        void Unlink<TPageType>(TPageType pageType) where TPageType : Enum;

        /// <summary>
        /// Unlinks a specific page type.
        /// </summary>
        /// <param name="pageType">The page type to unlink.</param>
        void Unlink(Enum pageType);
    }
}