using _Sculpture.Runtime.UI;
using _Sculpture.Runtime.UI.Visual;
using UnityEngine;
using UnityEngine.UI;

namespace _VRSculpture.Runtime.UI
{
    internal class AuthResultVisual : MonoBehaviourPageElement
    {
        [SerializeField] private VisibilityComponent _visibilityComponent;
        [SerializeField] private Button _closeButton;

        public override void HandleOpen(IOpenArguments arguments)
        {
            base.HandleOpen(arguments);
            _closeButton.onClick.AddListener(Close);
        }

        public override void HandleClose(ICloseArguments arguments)
        {
            base.HandleClose(arguments);
            _closeButton.onClick.RemoveListener(Close);
        }

        public void Show(bool immediately = false)
        {
            _visibilityComponent.Show(immediately);
        }

        public void Hide(bool immediately = false)
        {
            _visibilityComponent.Hide(immediately);
        }

        private void Close() => Hide();
    }
}
