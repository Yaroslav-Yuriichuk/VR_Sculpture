using _Sculpture.Runtime.Auth.Services;
using _Sculpture.Runtime.Content.Predefined;
using _Sculpture.Runtime.Content.Predefined.Services;
using _Sculpture.Runtime.Content.Remote.Services;
using _Sculpture.Runtime.UI.Services;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace _VRSculpture.Runtime
{
    internal sealed class ServicesScope : LifetimeScope
    {
        [Space]
        [SerializeField] private PredefinedModelsAssetReference _predefinedModelsReference;
        [SerializeField] private DefaultModelsAsset _defaultModels;

        protected override void Configure(IContainerBuilder builder)
        {
            base.Configure(builder);

            builder.Register<IUIService, UIService>(Lifetime.Singleton);
            builder.Register<IAuthService, FirebaseAuthService>(Lifetime.Singleton);
            builder.Register<IRemoteContentService, FirebaseRemoteContentService>(Lifetime.Singleton);
            builder.Register<IRemoteStorageService, FirebaseRemoteStorageService>(Lifetime.Singleton);
            builder.Register<IPredefinedContentService, AddressablesPredefinedContentService>(Lifetime.Singleton)
                .WithParameter(_predefinedModelsReference)
                .WithParameter(_defaultModels);
        }
    }
}