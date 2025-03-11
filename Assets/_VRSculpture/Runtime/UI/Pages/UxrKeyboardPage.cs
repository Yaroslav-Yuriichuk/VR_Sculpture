using System;
using _Sculpture.Runtime.UI;
using UltimateXR.UI.Helpers.Keyboard;
using UnityEngine;

namespace _VRSculpture.Runtime.UI.Pages
{
    internal sealed class UxrKeyboardPage : BasicPage<KeyboardArguments, KeyboardArguments>
    {
        [SerializeField] private UxrKeyboardUI _keyboard;

        private string _startLine;
        private Action<string> _lineChangedCallback;

        private string _currentLine;

        protected override void Open(KeyboardArguments arguments)
        {
            base.Open(arguments);

            _keyboard.Clear();
            _keyboard.KeyPressed += InvokeKeyCallback;

            _startLine = arguments.StartLine;
            _lineChangedCallback = arguments.LineChangedCallback;

            _currentLine = arguments.StartLine;
        }

        protected override void Reload(KeyboardArguments arguments)
        {
            base.Reload(arguments);

            _keyboard.Clear();

            _startLine = arguments.StartLine;
            _lineChangedCallback = arguments.LineChangedCallback;

            _currentLine = arguments.StartLine;
        }

        public override void Close(ICloseArguments arguments)
        {
            base.Close(arguments);
            _keyboard.KeyPressed -= InvokeKeyCallback;
        }

        private void InvokeKeyCallback(object sender, UxrKeyboardKeyEventArgs e)
        {
            if (!e.IsPress)
            {
                return;
            }

            if (_currentLine == _startLine && e.Key.KeyType == UxrKeyType.Backspace)
            {
                _currentLine = _currentLine.Length > 0 ? _currentLine[..^1] : _currentLine;
                _startLine = _currentLine;
            }
            else
            {
                _currentLine = string.Concat(_startLine, e.Line);
            }

            _lineChangedCallback?.Invoke(_currentLine);
        }
    }
}