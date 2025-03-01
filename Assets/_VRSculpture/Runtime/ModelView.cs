using System;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace VR__Sculpture.Runtime
{
    public sealed class ModelDataJson
    {
        [JsonProperty]
        public string Name { get; set; }

        [JsonProperty]
        public string Path { get; set; }
    }

    internal sealed class ModelView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _nameText;

        [Space]
        [SerializeField] private Button _loadButton;
        [SerializeField] private Button _saveButton;

        private ModelDataJson _data;
        private Action<ModelDataJson> _loadCallback;
        private Action<ModelDataJson> _saveCallback;

        public void Initialize(ModelDataJson data, Action<ModelDataJson> loadCallback, Action<ModelDataJson> saveCallback)
        {
            _data = data;
            _loadCallback = loadCallback;
            _saveCallback = saveCallback;

            _nameText.text = _data.Name;
        }

        private void OnEnable()
        {
            _loadButton.onClick.AddListener(RequestLoad);
            _saveButton.onClick.AddListener(RequestSave);
        }

        private void OnDisable()
        {
            _loadButton.onClick.RemoveListener(RequestLoad);
            _saveButton.onClick.RemoveListener(RequestSave);
        }

        private void RequestLoad() => _loadCallback?.Invoke(_data);
        private void RequestSave() => _saveCallback?.Invoke(_data);
    }
}