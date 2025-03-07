using System.Threading;
using _Sculpture.Runtime.Auth;
using _Sculpture.Runtime.Auth.Services;
using _Sculpture.Runtime.UI;
using _Sculpture.Runtime.UI.Visual;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace _VRSculpture.Runtime.UI.Pages
{
    internal sealed class SignInPage : BasicPage
    {
        [Space]
        [SerializeField] private TMP_InputField _emailInputField;
        [SerializeField] private TMP_InputField _passwordInputField;

        [Space]
        [SerializeField] private Button _signInButton;
        [SerializeField] private CanvasGroup _canvasGroup;

        [Space]
        [SerializeField] private VisibilityComponent _successVisual;
        [SerializeField] private SignInErrorVisual[] _errorVisuals;

        private IAuthService _authService;

        [Inject]
        private void Construct(IAuthService authService)
        {
            _authService = authService;
        }

        public override void Open(IOpenArguments arguments)
        {
            base.Open(arguments);

            _emailInputField.text = string.Empty;
            _passwordInputField.text = string.Empty;

            _canvasGroup.interactable = true;
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
        }

        private void SignIn() => SignInAsync(OpenCancellationToken).Forget();

        private async UniTaskVoid SignInAsync(CancellationToken cancellationToken)
        {
            _canvasGroup.interactable = false;

            _successVisual.Hide(immediately: true);

            foreach (SignInErrorVisual errorVisual in _errorVisuals)
            {
                errorVisual.Hide(immediately: true);
            }

            string email = _emailInputField.text;
            string password = _passwordInputField.text;

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
                _canvasGroup.interactable = true;
            }
        }
    }
}