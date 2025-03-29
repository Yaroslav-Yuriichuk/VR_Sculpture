using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace _Sculpture.Runtime.Content.Predefined
{
    [Serializable]
    internal sealed class TextAssetReference : AssetReferenceT<TextAsset>
    {
        public TextAssetReference(string guid) : base(guid) { }
    }
}