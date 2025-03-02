using UnityEngine;

namespace _Sculpture.Runtime.UI
{
    public class MonoBehaviourPage : MonoBehaviour, IPage
    {
        public virtual void Open(IOpenArguments arguments) { }

        public virtual void Close(ICloseArguments arguments) { }
    }
}