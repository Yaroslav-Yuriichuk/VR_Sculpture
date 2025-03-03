using System;
using System.Collections.Generic;

namespace _Sculpture.Runtime.UI
{
    public sealed class UIService : IUIService
    {
        public event Action<Enum, IOpenArguments> PageOpened;
        public event Action<Enum, ICloseArguments> PageClosed;

        private readonly Dictionary<Enum, IPage> _pages = new();

        public void Open<TPageType>(TPageType pageIdentifier, IOpenArguments arguments = null) where TPageType : Enum
        {
            if (!_pages.TryGetValue(pageIdentifier, out IPage page))
            {
                throw new InvalidOperationException($"Page of type {pageIdentifier} is not linked, cannot open.");
            }

            if (!page.IsOpen)
            {
                page.Open(arguments);
                PageOpened?.Invoke(pageIdentifier, arguments);
            }
        }

        public void Open(Enum pageIdentifier, IOpenArguments arguments = null)
        {
            if (!_pages.TryGetValue(pageIdentifier, out IPage page))
            {
                throw new InvalidOperationException($"Page of type {pageIdentifier} is not linked, cannot open.");
            }

            if (!page.IsOpen)
            {
                page.Open(arguments);
                PageOpened?.Invoke(pageIdentifier, arguments);
            }
        }

        public void Close<TPageType>(TPageType pageIdentifier, ICloseArguments arguments = null) where TPageType : Enum
        {
            if (!_pages.TryGetValue(pageIdentifier, out IPage page))
            {
                throw new InvalidOperationException($"Page of type {pageIdentifier} is not linked, cannot close.");
            }

            if (page.IsOpen)
            {
                page.Close(arguments);
                PageClosed?.Invoke(pageIdentifier, arguments);
            }
        }

        public void Close(Enum pageIdentifier, ICloseArguments arguments = null)
        {
            if (!_pages.TryGetValue(pageIdentifier, out IPage page))
            {
                throw new InvalidOperationException($"Page of type {pageIdentifier} is not linked, cannot close.");
            }

            if (page.IsOpen)
            {
                page.Close(arguments);
                PageClosed?.Invoke(pageIdentifier, arguments);
            }
        }

        public void Link<TPageType>(IPage page, TPageType pageIdentifier) where TPageType : Enum
        {
            if (page is null)
            {
                throw new ArgumentNullException(nameof(page), "Page provided mustn't null.");
            }

            if (!_pages.TryAdd(pageIdentifier, page))
            {
                throw new InvalidOperationException($"Page of type {pageIdentifier} is already linked, cannot link.");
            }
        }

        public void Link(IPage page, Enum pageIdentifier)
        {
            if (page is null)
            {
                throw new ArgumentNullException(nameof(page), "Page provided mustn't null.");
            }

            if (!_pages.TryAdd(pageIdentifier, page))
            {
                throw new InvalidOperationException($"Page of type {pageIdentifier} is already linked, cannot link.");
            }
        }

        public void Unlink<TPageType>(TPageType pageIdentifier) where TPageType : Enum
        {
            if (!_pages.Remove(pageIdentifier))
            {
                throw new InvalidOperationException($"Page of type {pageIdentifier} is not linked, cannot unlink.");
            }
        }

        public void Unlink(Enum pageIdentifier)
        {
            if (!_pages.Remove(pageIdentifier))
            {
                throw new InvalidOperationException($"Page of type {pageIdentifier} is not linked, cannot unlink.");
            }
        }
    }
}