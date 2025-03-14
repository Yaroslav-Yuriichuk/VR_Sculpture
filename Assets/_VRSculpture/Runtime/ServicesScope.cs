using _Sculpture.Runtime.Auth.Services;
using _Sculpture.Runtime.UI;
using _Sculpture.Runtime.UI.Services;
using VContainer;
using VContainer.Unity;

namespace _VRSculpture.Runtime
{
    internal sealed class ServicesScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            base.Configure(builder);

            builder.Register<IUIService, UIService>(Lifetime.Singleton);
            builder.Register<IAuthService, FirebaseAuthService>(Lifetime.Singleton);
        }
    }
}