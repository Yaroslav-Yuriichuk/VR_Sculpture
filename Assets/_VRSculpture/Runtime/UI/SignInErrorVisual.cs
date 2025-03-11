using _Sculpture.Runtime.Auth;
using UnityEngine;

namespace _VRSculpture.Runtime.UI
{
    internal sealed class SignInErrorVisual : AuthResultVisual
    {
        [field: Space]
        [field: SerializeField] public SignInError Error { get; private set; }
    }
}