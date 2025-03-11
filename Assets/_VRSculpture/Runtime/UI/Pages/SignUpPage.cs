using System.Threading;
using _Sculpture.Runtime.Auth;
using _Sculpture.Runtime.Auth.Services;
using _Sculpture.Runtime.UI;
using _Sculpture.Runtime.UI.Visual;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;
using Button = UnityEngine.UI.Button;

namespace _VRSculpture.Runtime.UI.Pages
{
    internal sealed class SignUpPage : BasicPage
    {
        [Space]
        [SerializeField] private EmailPasswordForm _emailPasswordForm;

        [Space]
        [SerializeField] private Button _signUpButton;
        [SerializeField] private CanvasGroup _canvasGroup;

        [Space]
        [SerializeField] private AuthResultVisual _successVisual;
        [SerializeField] private SignUpErrorVisual[] _errorVisuals;

        private IAuthService _authService;

        [Inject]
        private void Construct(IAuthService authService)
        {
            _authService = authService;
        }

        public override void Open(IOpenArguments arguments)
        {
            base.Open(arguments);

            _canvasGroup.interactable = true;
            _signUpButton.onClick.AddListener(SignUp);

            _successVisual.Hide(immediately: true);

            foreach (SignUpErrorVisual errorVisual in _errorVisuals)
            {
                errorVisual.Hide(immediately: true);
            }
        }

        public override void Close(ICloseArguments arguments)
        {
            base.Close(arguments);
            _signUpButton.onClick.RemoveListener(SignUp);
        }

        private void SignUp() => SignUpAsync(OpenCancellationToken).Forget();

        private async UniTaskVoid SignUpAsync(CancellationToken cancellationToken)
        {
            _canvasGroup.interactable = false;

            _successVisual.Hide(immediately: true);

            foreach (SignUpErrorVisual errorVisual in _errorVisuals)
            {
                errorVisual.Hide(immediately: true);
            }

            string email = _emailPasswordForm.Email;
            string password = _emailPasswordForm.Password;

            try
            {
                SignUpResult result = await _authService.SignUpAsync(email, password, cancellationToken);

                if (result.IsSuccessful)
                {
                    _successVisual.Show();
                }
                else
                {
                    foreach (SignUpErrorVisual errorVisual in _errorVisuals)
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
                _canvasGroup.interactable = true;
            }
        }
    }
}