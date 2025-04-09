using System.Threading;
using _Sculpture.Runtime.Content.Remote.Services;
using _Sculpture.Runtime.UI;
using _Sculpture.Runtime.UI.Services;
using _VRSculpture.Runtime.UI.Elements;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using VContainer;
using VoxelArt.Runtime;

namespace _VRSculpture.Runtime.UI.Pages.Models
{
    internal sealed class CreateModelPage : BasicPage
    {
        [Space]
        [SerializeField] private NameForm _nameForm;

        [Space]
        [SerializeField] private Button _createButton;
        [SerializeField] private Button _closeButton;

        private IUIService _uiService;
        private IRemoteContentService _remoteContentService;

        [Inject]
        private void Construct(IUIService uiService, IRemoteContentService remoteContentService)
        {
            _uiService = uiService;
            _remoteContentService = remoteContentService;
        }

        public override void Open(IOpenArguments arguments)
        {
            base.Open(arguments);

            _closeButton.onClick.AddListener(Close);
            _createButton.onClick.AddListener(CreateModel);
        }

        public override void Close(ICloseArguments arguments)
        {
            base.Close(arguments);

            _closeButton.onClick.RemoveListener(Close);
            _createButton.onClick.RemoveListener(CreateModel);

            if (_uiService.IsOpen(HelperPageIdentifier.MainLoader))
            {
                _uiService.Close(HelperPageIdentifier.MainLoader);
            }
        }

        private void Close() => _uiService.Close(MainPageIdentifier.CreateModel);

        private void CreateModel() => CreateModelAsync(OpenCancellationToken).Forget();

        private async UniTask CreateModelAsync(CancellationToken cancellationToken)
        {
            if (!VoxelArtSystems.Components.TryGetModelObject(_ => true, out ModelObject modelObject) ||
                modelObject.Model is null)
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(_nameForm.Name))
            {
                return;
            }

            _uiService.Open(HelperPageIdentifier.MainLoader);

            try
            {
                await _remoteContentService.AddModelAsync(_nameForm.Name, modelObject.Model, cancellationToken);
                _uiService.Close(MainPageIdentifier.CreateModel);
            }
            finally
            {
                _uiService.Close(HelperPageIdentifier.MainLoader);
            }
        }
    }
}