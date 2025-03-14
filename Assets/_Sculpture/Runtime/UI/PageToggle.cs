using System;
using _Sculpture.Runtime.UI.Services;
using UnityEngine;
using UnityEngine.UI;

namespace _Sculpture.Runtime.UI
{
    public abstract class PageToggle : MonoBehaviour
    {
        [SerializeField] private PageIdentifierData _pageIdentifier;

        protected abstract IUIService UIService { get; }

        private Toggle _toggle;

        private void Awake() => _toggle = GetComponent<Toggle>();

        private void OnEnable()
        {
            _toggle.onValueChanged.AddListener(OpenOrClosePage);

            UIService.PageOpened += EnableToggle;
            UIService.PageClosed += DisableToggle;

            OpenOrClosePage(_toggle.isOn);
        }

        private void OnDisable()
        {
            _toggle.onValueChanged.RemoveListener(OpenOrClosePage);

            UIService.PageOpened -= EnableToggle;
            UIService.PageClosed -= DisableToggle;
        }

        private void OpenOrClosePage(bool isOn)
        {
            if (!_pageIdentifier.TryGetIdentifier(out Enum identifier))
            {
                return;
            }

            if (isOn)
            {
                UIService.Open(identifier);
                return;
            }

            UIService.Close(identifier);
        }

        private void EnableToggle(Enum identifier, IOpenArguments arguments)
        {
            if (_pageIdentifier.TryGetIdentifier(out Enum pageIdentifier) && pageIdentifier.Equals(identifier))
            {
                _toggle.SetIsOnWithoutNotify(true);
            }
        }

        private void DisableToggle(Enum identifier, ICloseArguments arguments)
        {
            if (_pageIdentifier.TryGetIdentifier(out Enum pageIdentifier) && pageIdentifier.Equals(identifier))
            {
                _toggle.SetIsOnWithoutNotify(false);
            }
        }
    }
}
