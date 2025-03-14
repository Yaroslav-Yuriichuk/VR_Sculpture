using _Sculpture.Runtime.Auth;
using _Sculpture.Runtime.Auth.Services;
using _Sculpture.Runtime.UI;
using VContainer;

namespace _VRSculpture.Runtime.UI.Pages
{
    internal sealed class UserAuthPage : BasicPage
    {
        private IAuthService _authService;
        private IUIService _uiService;

        [Inject]
        private void Construct(IAuthService authService, IUIService uiService)
        {
            _authService = authService;
            _uiService = uiService;
        }

        public override void Open(IOpenArguments arguments)
        {
            base.Open(arguments);

            User user = _authService.CurrentUser;
            _uiService.Open(user == null ? UserAuthPageIdentifier.SignedOut : UserAuthPageIdentifier.SignedIn);

            _authService.UserSignedIn += SwitchToSignedInPage;
            _authService.UserSwitched += ReloadSignedInPage;
            _authService.UserSignedOut += SwitchToSignedOutPage;
        }

        public override void Close(ICloseArguments arguments)
        {
            base.Close(arguments);

            _authService.UserSignedIn -= SwitchToSignedInPage;
            _authService.UserSwitched -= ReloadSignedInPage;
            _authService.UserSignedOut -= SwitchToSignedOutPage;
        }

        private void SwitchToSignedInPage(User user)
        {
            if (_uiService.IsOpen(UserAuthPageIdentifier.SignedOut))
            {
                _uiService.Close(UserAuthPageIdentifier.SignedOut);
            }

            if (_uiService.IsOpen(UserAuthPageIdentifier.SignedIn))
            {
                _uiService.Reload(UserAuthPageIdentifier.SignedIn);
                return;
            }

            _uiService.Open(UserAuthPageIdentifier.SignedIn);
        }

        private void ReloadSignedInPage(User previousUser, User currentUser)
        {
            if (_uiService.IsOpen(UserAuthPageIdentifier.SignedIn))
            {
                _uiService.Reload(UserAuthPageIdentifier.SignedIn);
            }

            if (_uiService.IsOpen(UserAuthPageIdentifier.SignedIn))
            {
                _uiService.Reload(UserAuthPageIdentifier.SignedIn);
                return;
            }

            _uiService.Open(UserAuthPageIdentifier.SignedIn);
        }

        private void SwitchToSignedOutPage(User user)
        {
            if (_uiService.IsOpen(UserAuthPageIdentifier.SignedIn))
            {
                _uiService.Close(UserAuthPageIdentifier.SignedIn);
            }

            if (_uiService.IsOpen(UserAuthPageIdentifier.SignedOut))
            {
                _uiService.Reload(UserAuthPageIdentifier.SignedOut);
                return;
            }

            _uiService.Open(UserAuthPageIdentifier.SignedOut);
        }
    }
}