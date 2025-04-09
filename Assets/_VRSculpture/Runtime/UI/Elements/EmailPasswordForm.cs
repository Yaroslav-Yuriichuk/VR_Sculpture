using _Sculpture.Runtime.UI;
using _Sculpture.Runtime.UI.Services;
using _VRSculpture.Runtime.UI.Pages;
using _VRSculpture.Runtime.UI.Pages.Helper;
using TMPro;
using UnityEngine;
using VContainer;

namespace _VRSculpture.Runtime.UI.Elements
{
    internal sealed class EmailPasswordForm : MonoBehaviourPageElement
    {
        private enum SelectedInput
        {
            None,
            Email,
            Password,
        }

        [SerializeField] private TMP_InputField _emailInputField;
        [SerializeField] private TMP_InputField _passwordInputField;

        public string Email => _emailInputField.text;
        public string Password => _passwordInputField.text;

        private IUIService _uiService;

        private SelectedInput _selectedInput;

        [Inject]
        private void Construct(IUIService uiService)
        {
            _uiService = uiService;
        }

        public override void HandleOpen(IOpenArguments arguments)
        {
            base.HandleOpen(arguments);

            _selectedInput = SelectedInput.None;

            _emailInputField.text = string.Empty;
            _passwordInputField.text = string.Empty;

            _emailInputField.onSelect.AddListener(SelectEmailInput);
            _passwordInputField.onSelect.AddListener(SelectPasswordInput);
        }

        public override void HandleClose(ICloseArguments arguments)
        {
            base.HandleClose(arguments);

            _emailInputField.onSelect.RemoveListener(SelectEmailInput);
            _passwordInputField.onSelect.RemoveListener(SelectPasswordInput);

            if (_uiService.IsOpen(HelperPageIdentifier.Keyboard))
            {
                _uiService.Close(HelperPageIdentifier.Keyboard);
            }
        }

        private void SelectEmailInput(string text)
        {
            _selectedInput = SelectedInput.Email;

            string startLine = _emailInputField.text;
            KeyboardArguments keyboardArguments = new KeyboardArguments(startLine, SetInputLine);

            if (_uiService.IsOpen(HelperPageIdentifier.Keyboard))
            {
                _uiService.Reload(HelperPageIdentifier.Keyboard, keyboardArguments);
                return;
            }

            _uiService.Open(HelperPageIdentifier.Keyboard , keyboardArguments);
        }

        private void SelectPasswordInput(string text)
        {
            _selectedInput = SelectedInput.Password;

            string startLine = _passwordInputField.text;
            KeyboardArguments keyboardArguments = new KeyboardArguments(startLine, SetInputLine);

            if (_uiService.IsOpen(HelperPageIdentifier.Keyboard))
            {
                _uiService.Reload(HelperPageIdentifier.Keyboard, keyboardArguments);
                return;
            }

            _uiService.Open(HelperPageIdentifier.Keyboard , keyboardArguments);
        }

        private void SetInputLine(string input)
        {
            switch (_selectedInput)
            {
                case SelectedInput.Email:
                    _emailInputField.text = input;
                    break;
                case SelectedInput.Password:
                    _passwordInputField.text = input;
                    break;
            }
        }
    }
}