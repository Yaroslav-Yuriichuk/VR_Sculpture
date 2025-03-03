using UnityEngine;

namespace _Sculpture.Runtime.UI
{
    public class MonoBehaviourPageElement : MonoBehaviour
    {
        public virtual void HandleOpen(IOpenArguments arguments) { }

        public virtual void HandleClose(ICloseArguments arguments) { }
    }
}