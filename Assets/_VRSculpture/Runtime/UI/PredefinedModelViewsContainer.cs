using System.Collections.Generic;
using System.Linq;
using System.Threading;
using _Sculpture.Runtime.Content;
using _Sculpture.Runtime.Content.Predefined.Services;
using _Sculpture.Runtime.UI;
using Cysharp.Threading.Tasks;
using VContainer;

namespace _VRSculpture.Runtime.UI
{
    internal sealed class PredefinedModelViewsContainer : ModelViewsContainer
    {
        private IPredefinedContentService _predefinedContentService;

        private CancellationTokenSource _loadCts;

        [Inject]
        private void Construct(IPredefinedContentService predefinedContentService)
        {
            _predefinedContentService = predefinedContentService;
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
        }

        private async UniTask<IEnumerable<ModelDescriptor>> LoadModelsAsync(CancellationToken cancellationToken)
        {
            ModelsGetResult result = await _predefinedContentService.GetModelsAsync(cancellationToken);

            if (!result.IsSuccessful)
            {
                return Enumerable.Empty<ModelDescriptor>();
            }

            return result.Descriptors;
        }
    }
}