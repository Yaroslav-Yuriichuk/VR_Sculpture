using _Sculpture.Runtime.Auth;
using _Sculpture.Runtime.UI.Visual;
using UnityEngine;

namespace _VRSculpture.Runtime.UI
{
    internal sealed class SignInErrorVisual : VisibilityComponent
    {
        [SerializeField] private VisibilityComponent _visibilityComponent;

        [field: SerializeField] public SignInError Error { get; private set; }

        public override void Show(bool immediately = false)
        {
            _visibilityComponent.Show(immediately);
        }

        public override void Hide(bool immediately = false)
        {
            _visibilityComponent.Hide(immediately);
        }
    }
}