using System;
using _Sculpture.Runtime.UI;

namespace _VRSculpture.Runtime.UI
{
    internal sealed class KeyboardArguments : IOpenArguments, IReloadArguments
    {
        public string StartLine { get; }
        public Action<string> LineChangedCallback { get; }

        public KeyboardArguments(string startLine, Action<string> lineChangedCallback)
        {
            StartLine = startLine;
            LineChangedCallback = lineChangedCallback;
        }
    }
}