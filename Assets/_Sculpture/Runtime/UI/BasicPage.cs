using System;
using _Sculpture.Runtime.UI.Visual;
using UnityEngine;

namespace _Sculpture.Runtime.UI
{
    public class BasicPage : MonoBehaviourPage
    {
        [Space]
        [SerializeField] private VisibilityComponent _visibilityComponent;

        public override void Open(IOpenArguments arguments)
        {
            base.Open(arguments);

            if (_visibilityComponent == null)
            {
                _visibilityComponent = gameObject.AddComponent<DefaultVisibilityComponent>();
            }

            if (_visibilityComponent != null)
            {
                _visibilityComponent.Show();
            }
        }

        public override void Close(ICloseArguments arguments)
        {
            base.Close(arguments);

            if (_visibilityComponent == null)
            {
                _visibilityComponent = gameObject.AddComponent<DefaultVisibilityComponent>();
            }

            if (_visibilityComponent != null)
            {
                _visibilityComponent.Hide();
            }
        }
    }

    public class BasicPage<TOpenArguments> : BasicPage
        where TOpenArguments : IOpenArguments
    {
        public sealed override void Open(IOpenArguments arguments)
        {
            base.Open(arguments);

            if (arguments is null)
            {
                throw new ArgumentNullException(nameof(arguments), $"Open arguments provided to page {nameof(BasicPage<TOpenArguments>)} cannot be null.");
            }

            TOpenArguments openArguments;

            try
            {
                openArguments = (TOpenArguments)arguments;
            }
            catch (Exception)
            {
                throw new ArgumentException($"Open arguments provided to page {nameof(BasicPage<TOpenArguments>)} must cast to {nameof(TOpenArguments)}.");
            }

            Open(openArguments);
        }

        protected virtual void Open(TOpenArguments arguments) { }
    }

    public class BasicPage<TOpenArguments, TReloadArguments> : BasicPage
        where TOpenArguments : IOpenArguments
        where TReloadArguments : IReloadArguments
    {
        public sealed override void Open(IOpenArguments arguments)
        {
            base.Open(arguments);

            if (arguments is null)
            {
                throw new ArgumentNullException(nameof(arguments), $"Open arguments provided to page {nameof(BasicPage<TOpenArguments, TReloadArguments>)} cannot be null.");
            }

            TOpenArguments openArguments;

            try
            {
                openArguments = (TOpenArguments)arguments;
            }
            catch (Exception)
            {
                throw new ArgumentException($"Open arguments provided to page {nameof(BasicPage<TOpenArguments, TReloadArguments>)} must cast to {nameof(TOpenArguments)}.");
            }

            Open(openArguments);
        }

        public sealed override void Reload(IReloadArguments arguments)
        {
            base.Reload(arguments);

            if (arguments is null)
            {
                throw new ArgumentNullException(nameof(arguments), $"Reload arguments provided to page {nameof(BasicPage<TOpenArguments, TReloadArguments>)} cannot be null.");
            }

            TReloadArguments reloadArguments;

            try
            {
                reloadArguments = (TReloadArguments)arguments;
            }
            catch (Exception)
            {
                throw new ArgumentException($"Reload arguments provided to page {nameof(BasicPage<TOpenArguments, TReloadArguments>)} must cast to {nameof(TReloadArguments)}.");
            }

            Reload(reloadArguments);
        }

        protected virtual void Open(TOpenArguments arguments) { }
        protected virtual void Reload(TReloadArguments arguments) { }
    }

    public class BasicPage<TOpenArguments, TReloadArguments, TCloseArguments> : BasicPage
        where TOpenArguments : IOpenArguments
        where TReloadArguments : IReloadArguments
        where TCloseArguments : ICloseArguments
    {
        public sealed override void Open(IOpenArguments arguments)
        {
            base.Open(arguments);

            if (arguments is null)
            {
                throw new ArgumentNullException(nameof(arguments), $"Open arguments provided to page {nameof(BasicPage<TOpenArguments, TReloadArguments, TCloseArguments>)} cannot be null.");
            }

            TOpenArguments openArguments;

            try
            {
                openArguments = (TOpenArguments)arguments;
            }
            catch (Exception)
            {
                throw new ArgumentException($"Open arguments provided to page {nameof(BasicPage<TOpenArguments, TReloadArguments, TCloseArguments>)} must cast to {nameof(TOpenArguments)}.");
            }

            Open(openArguments);
        }

        public sealed override void Reload(IReloadArguments arguments)
        {
            base.Reload(arguments);

            if (arguments is null)
            {
                throw new ArgumentNullException(nameof(arguments), $"Reload arguments provided to page {nameof(BasicPage<TOpenArguments, TReloadArguments>)} cannot be null.");
            }

            TReloadArguments reloadArguments;

            try
            {
                reloadArguments = (TReloadArguments)arguments;
            }
            catch (Exception)
            {
                throw new ArgumentException($"Reload arguments provided to page {nameof(BasicPage<TOpenArguments, TReloadArguments>)} must cast to {nameof(TReloadArguments)}.");
            }

            Reload(reloadArguments);
        }

        public sealed override void Close(ICloseArguments arguments)
        {
            base.Close(arguments);

            if (arguments is null)
            {
                throw new ArgumentNullException(nameof(arguments), $"Close arguments provided to page {nameof(BasicPage<TOpenArguments, TReloadArguments, TCloseArguments>)} cannot be null.");
            }

            TCloseArguments closeArguments;

            try
            {
                closeArguments = (TCloseArguments)arguments;
            }
            catch (Exception)
            {
                throw new ArgumentException($"Close arguments provided to page {nameof(BasicPage<TOpenArguments, TReloadArguments, TCloseArguments>)} must cast to {nameof(TCloseArguments)}.");
            }

            Close(closeArguments);
        }

        protected virtual void Open(TOpenArguments arguments) { }
        protected virtual void Reload(TReloadArguments arguments) { }
        protected virtual void Close(TCloseArguments arguments) { }
    }
}