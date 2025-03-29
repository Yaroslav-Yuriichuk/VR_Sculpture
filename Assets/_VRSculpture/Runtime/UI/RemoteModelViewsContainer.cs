using System.Collections.Generic;
using System.Linq;
using System.Threading;
using _Sculpture.Runtime.Auth;
using _Sculpture.Runtime.Auth.Services;
using _Sculpture.Runtime.Content;
using _Sculpture.Runtime.Content.Remote.Services;
using _Sculpture.Runtime.UI;
using Cysharp.Threading.Tasks;
using VContainer;

namespace _VRSculpture.Runtime.UI
{
    internal sealed class RemoteModelViewsContainer : ModelViewsContainer
    {
        private IRemoteContentService _remoteContentService;
        private IAuthService _authService;

        private CancellationTokenSource _loadCts;

        [Inject]
        private void Construct(IRemoteContentService remoteContentService, IAuthService authService)
        {
            _remoteContentService = remoteContentService;
            _authService = authService;
        }

        public override void HandleOpen(IOpenArguments arguments)
        {
            base.HandleOpen(arguments);

            _loadCts?.Cancel();
            _loadCts = CancellationTokenSource.CreateLinkedTokenSource(OpenCancellationToken);

            UniTask.Create(async () =>
                {
                    IEnumerable<ModelDescriptor> descriptors = await LoadModelsAsync(_loadCts.Token);

                    foreach (ModelDescriptor descriptor in descriptors)
                    {
                        AddView(descriptor);
                    }
                })
                .Forget();

            _authService.UserSignedIn += LoadViews;
            _authService.UserSwitched += ReloadViews;
            _authService.UserSignedOut += ClearViews;

            _remoteContentService.ModelAdded += AddView;
            _remoteContentService.ModelDeleted += RemoveView;
        }

        public override void HandleReload(IReloadArguments arguments)
        {
            base.HandleReload(arguments);

            _loadCts?.Cancel();
            _loadCts = CancellationTokenSource.CreateLinkedTokenSource(OpenCancellationToken);

            ClearViews();

            UniTask.Create(async () =>
                {
                    IEnumerable<ModelDescriptor> descriptors = await LoadModelsAsync(_loadCts.Token);

                    foreach (ModelDescriptor descriptor in descriptors)
                    {
                        AddView(descriptor);
                    }
                })
                .Forget();
        }

        public override void HandleClose(ICloseArguments arguments)
        {
            base.HandleClose(arguments);

            ClearViews();

            _authService.UserSignedIn -= LoadViews;
            _authService.UserSignedOut -= ClearViews;

            _remoteContentService.ModelAdded -= AddView;
            _remoteContentService.ModelDeleted -= RemoveView;
        }

        private void LoadViews(User currentUser)
        {
            _loadCts?.Cancel();
            _loadCts = CancellationTokenSource.CreateLinkedTokenSource(OpenCancellationToken);

            UniTask.Create(async () =>
                {
                    IEnumerable<ModelDescriptor> descriptors = await LoadModelsAsync(_loadCts.Token);

                    foreach (ModelDescriptor descriptor in descriptors)
                    {
                        AddView(descriptor);
                    }
                })
                .Forget();
        }

        private void ReloadViews(User previousUser, User currentUser)
        {
            ClearViews();

            _loadCts?.Cancel();
            _loadCts = CancellationTokenSource.CreateLinkedTokenSource(OpenCancellationToken);

            UniTask.Create(async () =>
                {
                    IEnumerable<ModelDescriptor> descriptors = await LoadModelsAsync(_loadCts.Token);

                    foreach (ModelDescriptor descriptor in descriptors)
                    {
                        AddView(descriptor);
                    }
                })
                .Forget();
        }

        private void ClearViews(User currentUser)
        {
            _loadCts?.Cancel();
            ClearViews();
        }

        private async UniTask<IEnumerable<ModelDescriptor>> LoadModelsAsync(CancellationToken cancellationToken)
        {
            ModelsGetResult result = await _remoteContentService.GetModelsAsync(cancellationToken);

            if (!result.IsSuccessful)
            {
                return Enumerable.Empty<ModelDescriptor>();
            }

            return result.Descriptors;
        }
    }
}