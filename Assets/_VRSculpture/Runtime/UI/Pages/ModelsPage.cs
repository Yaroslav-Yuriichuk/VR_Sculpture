using _Sculpture.Runtime.UI;
using _Sculpture.Runtime.UI.Services;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace _VRSculpture.Runtime.UI.Pages
{
    internal sealed class ModelsPage : BasicPage
    {
        [Space]
        [SerializeField] private Button _createButton;

        private IUIService _uiService;

        [Inject]
        private void Construct(IUIService uiService)
        {
            _uiService = uiService;
        }

        public override void Open(IOpenArguments arguments)
        {
            base.Open(arguments);

            _createButton.onClick.AddListener(CreateModel);
        }

        public override void Close(ICloseArguments arguments)
        {
            base.Close(arguments);

            _createButton.onClick.RemoveListener(CreateModel);

            if (_uiService.IsOpen(MainPageIdentifier.CreateModel))
            {
                _uiService.Close(MainPageIdentifier.CreateModel);
            }

            if (_uiService.IsOpen(HelperPageIdentifier.MainLoader))
            {
                _uiService.Close(HelperPageIdentifier.MainLoader);
            }
        }

        private void CreateModel() => _uiService.Open(MainPageIdentifier.CreateModel);
    }
}
