using UnityEngine;

namespace _Sculpture.Runtime.UI.Visual
{
    public abstract class VisibilityComponent : MonoBehaviour
    {
        public abstract void Show(bool immediately = false);

        public abstract void Hide(bool immediately = false);
    }
}