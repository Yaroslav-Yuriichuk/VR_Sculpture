using _Sculpture.Runtime.Auth.Services;
using _Sculpture.Runtime.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace _VRSculpture.Runtime.UI.Pages
{
    internal sealed class UserSignedInPage : BasicPage
    {
        [Space]
        [SerializeField] private TMP_Text _emailText;
        [SerializeField] private Button _signOutButton;

        private IAuthService _authService;

        [Inject]
        private void Construct(IAuthService authService)
        {
            _authService = authService;
        }

        public override void Open(IOpenArguments arguments)
        {
            base.Open(arguments);

            _emailText.text = _authService.CurrentUser.Email;
            _signOutButton.onClick.AddListener(SignOut);
        }

        public override void Reload(IReloadArguments arguments)
        {
            base.Reload(arguments);
            _emailText.text = _authService.CurrentUser.Email;
        }

        public override void Close(ICloseArguments arguments)
        {
            base.Close(arguments);
            _signOutButton.onClick.RemoveListener(SignOut);
        }

        private void SignOut() => _authService.SignOut();
    }
}