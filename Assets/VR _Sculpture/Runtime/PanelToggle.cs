using UnityEngine;
using UnityEngine.UI;

namespace VR__Sculpture.Runtime
{
    [RequireComponent(typeof(Toggle))]
    internal sealed class PanelToggle : MonoBehaviour
    {
        [SerializeField] private GameObject _panel;

        private Toggle _toggle;

        private void Awake() => _toggle = GetComponent<Toggle>();
        private void Start() => EnablePanel(_toggle.isOn);

        private void OnEnable() => _toggle.onValueChanged.AddListener(EnablePanel);
        private void OnDisable() => _toggle.onValueChanged.RemoveListener(EnablePanel);

        private void EnablePanel(bool isOn) => _panel.SetActive(isOn);
    }
}