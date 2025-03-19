using _Sculpture.Runtime.UI;
using _Sculpture.Runtime.UI.Services;
using TMPro;
using UnityEngine;
using VContainer;

namespace _VRSculpture.Runtime.UI
{
    internal sealed class NameForm : MonoBehaviourPageElement
    {
        [SerializeField] private TMP_InputField _nameInputField;

        public string Name => _nameInputField.text;

        private IUIService _uiService;

        [Inject]
        private void Construct(IUIService uiService)
        {
            _uiService = uiService;
        }

        public override void HandleOpen(IOpenArguments arguments)
        {
            base.HandleOpen(arguments);

            _nameInputField.text = string.Empty;
            _nameInputField.onSelect.AddListener(SelectEmailInput);
        }

        public override void HandleClose(ICloseArguments arguments)
        {
            base.HandleClose(arguments);

            _nameInputField.onSelect.RemoveListener(SelectEmailInput);

            if (_uiService.IsOpen(HelperPageIdentifier.Keyboard))
            {
                _uiService.Close(HelperPageIdentifier.Keyboard);
            }
        }

        private void SelectEmailInput(string text)
        {
            string startLine = _nameInputField.text;
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
            _nameInputField.text = input;
        }
    }
}