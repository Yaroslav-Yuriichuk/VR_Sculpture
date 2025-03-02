using System;
using System.Collections.Generic;

namespace _Sculpture.Runtime.UI
{
    internal sealed class UIService : IUIService
    {
        private readonly Dictionary<Enum, IPage> _pages = new();

        public void Open<TPageType>(TPageType pageType, IOpenArguments arguments = null) where TPageType : Enum
        {
            if (!_pages.TryGetValue(pageType, out IPage page))
            {
                throw new InvalidOperationException($"Page of type {pageType} is not linked, cannot open.");
            }

            page.Open(arguments);
        }

        public void Open(Enum pageType, IOpenArguments arguments = null)
        {
            if (!_pages.TryGetValue(pageType, out IPage page))
            {
                throw new InvalidOperationException($"Page of type {pageType} is not linked, cannot open.");
            }

            page.Open(arguments);
        }

        public void Close<TPageType>(TPageType pageType, ICloseArguments arguments = null) where TPageType : Enum
        {
            if (!_pages.TryGetValue(pageType, out IPage page))
            {
                throw new InvalidOperationException($"Page of type {pageType} is not linked, cannot close.");
            }

            page.Close(arguments);
        }

        public void Close(Enum pageType, ICloseArguments arguments = null)
        {
            if (!_pages.TryGetValue(pageType, out IPage page))
            {
                throw new InvalidOperationException($"Page of type {pageType} is not linked, cannot close.");
            }

            page.Close(arguments);
        }

        public void Link<TPageType>(IPage page, TPageType pageType) where TPageType : Enum
        {
            if (page is null)
            {
                throw new ArgumentNullException(nameof(page), "Page provided mustn't null.");
            }

            if (!_pages.TryAdd(pageType, page))
            {
                throw new InvalidOperationException($"Page of type {pageType} is already linked, cannot link.");
            }
        }

        public void Link(IPage page, Enum pageType)
        {
            if (page is null)
            {
                throw new ArgumentNullException(nameof(page), "Page provided mustn't null.");
            }

            if (!_pages.TryAdd(pageType, page))
            {
                throw new InvalidOperationException($"Page of type {pageType} is already linked, cannot link.");
            }
        }

        public void Unlink<TPageType>(TPageType pageType) where TPageType : Enum
        {
            if (!_pages.Remove(pageType))
            {
                throw new InvalidOperationException($"Page of type {pageType} is not linked, cannot unlink.");
            }
        }

        public void Unlink(Enum pageType)
        {
            if (!_pages.Remove(pageType))
            {
                throw new InvalidOperationException($"Page of type {pageType} is not linked, cannot unlink.");
            }
        }
    }
}