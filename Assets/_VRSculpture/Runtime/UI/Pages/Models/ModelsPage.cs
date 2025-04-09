using _Sculpture.Runtime.Auth;
using _Sculpture.Runtime.Auth.Services;
using _Sculpture.Runtime.UI;
using _Sculpture.Runtime.UI.Services;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace _VRSculpture.Runtime.UI.Pages.Models
{
    internal sealed class ModelsPage : BasicPage
    {
        [Space]
        [SerializeField] private Button _createButton;

        private IUIService _uiService;
        private IAuthService _authService;

        [Inject]
        private void Construct(IUIService uiService, IAuthService authService)
        {
            _uiService = uiService;
            _authService = authService;
        }

        public override void Open(IOpenArguments arguments)
        {
            base.Open(arguments);

            _authService.UserSignedIn += ActivateCreateModelButton;
            _authService.UserSwitched += ActivateCreateModelButton;
            _authService.UserSignedOut += ActivateCreateModelButton;

            ActivateCreateModelButton();
            _createButton.onClick.AddListener(CreateModel);
        }

        public override void Close(ICloseArguments arguments)
        {
            base.Close(arguments);

            _authService.UserSignedIn -= ActivateCreateModelButton;
            _authService.UserSwitched -= ActivateCreateModelButton;
            _authService.UserSignedOut -= ActivateCreateModelButton;

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

        private void ActivateCreateModelButton(User previousUser, User currentUser)
        {
            ActivateCreateModelButton(currentUser);
        }

        private void ActivateCreateModelButton(User currentUser = null)
        {
            _createButton.gameObject.SetActive(_authService.CurrentUser != null);

            if (_authService.CurrentUser == null && _uiService.IsOpen(MainPageIdentifier.CreateModel))
            {
                _uiService.Close(MainPageIdentifier.CreateModel);
            }
        }
    }
}
