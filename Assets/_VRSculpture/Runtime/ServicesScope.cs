using _Sculpture.Runtime.Auth.Services;
using VContainer;
using VContainer.Unity;

namespace VR__Sculpture.Runtime
{
    internal sealed class ServicesScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            base.Configure(builder);

            builder.Register<IAuthService, FireBaseAuthService>(Lifetime.Singleton);
        }
    }
}