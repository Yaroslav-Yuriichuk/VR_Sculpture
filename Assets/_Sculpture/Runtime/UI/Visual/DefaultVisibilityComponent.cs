using UnityEngine;

namespace _Sculpture.Runtime.UI.Visual
{
    internal sealed class DefaultVisibilityComponent : VisibilityComponent
    {
        [SerializeField] private GameObject _object;

        public override void Show(bool immediately = false)
        {
            if (_object == null)
            {
                _object = gameObject;
            }

            _object.SetActive(true);
        }

        public override void Hide(bool immediately = false)
        {
            if (_object == null)
            {
                _object = gameObject;
            }

            _object.SetActive(false);
        }
    }
}