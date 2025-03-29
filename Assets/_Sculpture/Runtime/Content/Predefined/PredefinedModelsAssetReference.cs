using System;

using UnityEngine.AddressableAssets;

namespace _Sculpture.Runtime.Content.Predefined
{
    [Serializable]
    public sealed class PredefinedModelsAssetReference : AssetReferenceT<PredefinedModelsAsset>
    {
        public PredefinedModelsAssetReference(string guid) : base(guid) { }
    }
}