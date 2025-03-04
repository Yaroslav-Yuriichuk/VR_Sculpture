using UnityEngine;

namespace _VRSculpture.Runtime.Emulator
{
    internal sealed class EmulatorPrefabResolver : MonoBehaviour
    {
        [SerializeField] private GameObject _originalPrefab;
        [SerializeField] private GameObject _emulatorPrefab;

        [Space]
        [SerializeField] private Transform _parent;

        private void Start()
        {
#if !UNITY_EDITOR
            Instantiate(_originalPrefab, _parent);
#else
            Instantiate(_emulatorPrefab, _parent);
#endif
        }
    }
}