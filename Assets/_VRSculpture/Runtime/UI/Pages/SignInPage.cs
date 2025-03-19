using System.Threading;
using _Sculpture.Runtime.Auth;
using _Sculpture.Runtime.Auth.Services;
using _Sculpture.Runtime.UI;
using _Sculpture.Runtime.UI.Services;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace _VRSculpture.Runtime.UI.Pages
{
    internal sealed class SignInPage : BasicPage
    {
        [Space]
        [SerializeField] private EmailPasswordForm _emailPasswordForm;

        [Space]
        [SerializeField] private Button _signInButton;

        [Space]
        [SerializeField] private AuthResultVisual _successVisual;
        [SerializeField] private SignInErrorVisual[] _errorVisuals;

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

            _signInButton.onClick.AddListener(SignIn);

            _successVisual.Hide(immediately: true);

            foreach (SignInErrorVisual errorVisual in _errorVisuals)
            {
                errorVisual.Hide(immediately: true);
            }
        }

        public override void Close(ICloseArguments arguments)
        {
            base.Close(arguments);

            _signInButton.onClick.RemoveListener(SignIn);

            if (_uiService.IsOpen(HelperPageIdentifier.MainLoader))
            {
                _uiService.Close(HelperPageIdentifier.MainLoader);
            }
        }

        private void SignIn() => SignInAsync(OpenCancellationToken).Forget();

        private async UniTaskVoid SignInAsync(CancellationToken cancellationToken)
        {
            _uiService.Open(HelperPageIdentifier.MainLoader);

            _successVisual.Hide(immediately: true);

            foreach (SignInErrorVisual errorVisual in _errorVisuals)
            {
                errorVisual.Hide(immediately: true);
            }

            string email = _emailPasswordForm.Email;
            string password = _emailPasswordForm.Password;

            try
            {
                SignInResult result = await _authService.SignInAsync(email, password, cancellationToken);

                if (result.IsSuccessful)
                {
                    _successVisual.Show();
                }
                else
                {
                    foreach (SignInErrorVisual errorVisual in _errorVisuals)
                    {
                        if (errorVisual.Error == result.Error)
                        {
                            errorVisual.Show();
                        }
                        else
                        {
                            errorVisual.Hide();
                        }
                    }
                }
            }
            finally
            {
                _uiService.Close(HelperPageIdentifier.MainLoader);
            }
        }
    }
}