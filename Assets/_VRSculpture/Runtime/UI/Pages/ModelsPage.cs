using System.Collections.Generic;
using System.Linq;
using System.Threading;
using _Sculpture.Runtime.Auth;
using _Sculpture.Runtime.Auth.Services;
using _Sculpture.Runtime.Content;
using _Sculpture.Runtime.Content.Services;
using _Sculpture.Runtime.UI;
using _Sculpture.Runtime.UI.Services;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace _VRSculpture.Runtime.UI.Pages
{
    internal sealed class ModelsPage : BasicPage
    {
        [Space]
        [SerializeField] private ModelView _remoteModelViewPrefab;
        [SerializeField] private Transform _viewsParent;

        [Space]
        [SerializeField] private Button _createButton;
        [SerializeField] private CanvasGroup _canvasGroup;

        [Space]
        [SerializeField] private TextAsset[] _predefinedModels;

        private readonly Dictionary<string, ModelView> _views = new();

        private IUIService _uiService;
        private IContentService _contentService;
        private IAuthService _authService;

        [Inject]
        private void Construct(IUIService uiService, IContentService contentService, IAuthService authService)
        {
            _uiService = uiService;
            _contentService = contentService;
            _authService = authService;
        }

        public override void Open(IOpenArguments arguments)
        {
            base.Open(arguments);

            UniTask.Create(async () =>
                {
                    IEnumerable<ModelDescriptor> descriptors = await LoadModelsAsync(OpenCancellationToken);

                    foreach (ModelDescriptor descriptor in descriptors)
                    {
                        AddView(descriptor);
                    }
                })
                .Forget();

            _createButton.onClick.AddListener(CreateModel);

            _authService.UserSignedOut += ClearViews;
        }

        public override void Close(ICloseArguments arguments)
        {
            base.Close(arguments);

            ClearViews();

            _createButton.onClick.RemoveListener(CreateModel);

            if (_uiService.IsOpen(MainPageIdentifier.CreateModel))
            {
                _uiService.Close(MainPageIdentifier.CreateModel);
            }

            if (_uiService.IsOpen(HelperPageIdentifier.MainLoader))
            {
                _uiService.Close(HelperPageIdentifier.MainLoader);
            }

            _authService.UserSignedOut -= ClearViews;
        }

        private void CreateModel() => _uiService.Open(MainPageIdentifier.CreateModel);

        private async UniTask<IEnumerable<ModelDescriptor>> LoadModelsAsync(CancellationToken cancellationToken)
        {
            ModelsGetResult result = await _contentService.GetModelsAsync(cancellationToken);

            if (!result.IsSuccessful)
            {
                return Enumerable.Empty<ModelDescriptor>();
            }

            return result.Descriptors;
        }

        private void ClearViews(User currentUser) => ClearViews();

        private void AddView(ModelDescriptor descriptor)
        {
            if (_views.ContainsKey(descriptor.Id))
            {
                return;
            }

            ModelView view = Instantiate(_remoteModelViewPrefab, _viewsParent);
            view.Initialize(descriptor);

            _views.Add(descriptor.Id, view);
        }

        private void RemoveView(ModelDescriptor descriptor)
        {
            if (!_views.TryGetValue(descriptor.Id, out ModelView view))
            {
                return;
            }

            view.Uninitialize();
            Destroy(view.gameObject);

            _views.Remove(descriptor.Id);
        }

        private void ClearViews()
        {
            foreach (ModelView view in _views.Values)
            {
                view.Uninitialize();
                Destroy(view.gameObject);
            }

            _views.Clear();
        }
    }
}
