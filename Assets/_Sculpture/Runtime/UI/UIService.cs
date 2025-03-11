using System;
using System.Collections.Generic;

namespace _Sculpture.Runtime.UI
{
    public sealed class UIService : IUIService
    {
        public event Action<Enum, IOpenArguments> PageOpened;
        public event Action<Enum, ICloseArguments> PageClosed;

        private readonly Dictionary<Enum, IPage> _pages = new();

        public bool IsOpen(Enum pageIdentifier)
        {
            return _pages.TryGetValue(pageIdentifier, out IPage page) && page.IsOpen;
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

        public void Reload(Enum pageIdentifier, IReloadArguments arguments = null)
        {
            if (!_pages.TryGetValue(pageIdentifier, out IPage page))
            {
                throw new InvalidOperationException($"Page of type {pageIdentifier} is not linked, cannot reload.");
            }

            if (page.IsOpen)
            {
                page.Reload(arguments);
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

        public void Link(IPage page, Enum pageIdentifier)
        {
            if (page is null)
            {
                throw new ArgumentNullException(nameof(page), "Page provided mustn't be null.");
            }

            if (!_pages.TryAdd(pageIdentifier, page))
            {
                throw new InvalidOperationException($"Page of type {pageIdentifier} is already linked, cannot link.");
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