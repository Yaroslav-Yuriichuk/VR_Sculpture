using _Sculpture.Runtime.Auth;
using UnityEngine;

namespace _VRSculpture.Runtime.UI.Pages.Auth
{
    internal sealed class SignUpErrorVisual : AuthResultVisual
    {
        [field: Space]
        [field: SerializeField] public SignUpError Error { get; private set; }
    }
}