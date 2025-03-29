using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace _Sculpture.Runtime.Content.Predefined
{
    [CreateAssetMenu(fileName = "Predefined Models", menuName = "Sculpture/Predefined Models")]
    public sealed class PredefinedModelsAsset : ScriptableObject
    {
        [Serializable]
        private sealed class ModelData
        {
            [field: SerializeField] public string Id { get; private set; }
            [field: SerializeField] public string Name { get; private set; }
            [field: SerializeField] public TextAssetReference AssetReference { get; private set; }
        }

        [SerializeField] private ModelData[] _models;

        internal IEnumerable<ModelDescriptor> Descriptors
        {
            get
            {
                if (_models is null)
                {
                    return Enumerable.Empty<ModelDescriptor>();
                }

                return _models.Select(data => new ModelDescriptor { Id = data.Id, Name = data.Name });
            }
        }

        internal TextAssetReference GetAssetReference(ModelDescriptor descriptor)
        {
            if (_models is null)
            {
                return null;
            }

            ModelData data = _models.FirstOrDefault(d => d.Id == descriptor.Id);
            return data?.AssetReference;
        }
    }
}