using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VoxelArt.Runtime;
using VoxelArt.Runtime.Saving;

namespace _Sculpture.Runtime.Content.Predefined.Services
{
    public sealed class AddressablesPredefinedContentService : IPredefinedContentService
    {
        private readonly PredefinedModelsAsset _defaultModels;

        public AddressablesPredefinedContentService(PredefinedModelsAsset defaultModels)
        {
            _defaultModels = defaultModels;
        }

        public async UniTask<ModelsGetResult> GetModelsAsync(CancellationToken cancellationToken = default)
        {
            // TODO: Load addressable models.

            IEnumerable<ModelDescriptor> descriptors = _defaultModels.Descriptors;
            return new ModelsGetResult(true, descriptors);
        }

        public async UniTask<ModelGetResult> GetModelAsync(ModelDescriptor descriptor, CancellationToken cancellationToken = default)
        {
            TextAsset asset = _defaultModels.GetAsset(descriptor);

            ReadSource source = ReadSource.TextAsset(asset);
            ModelReadResult readResult = await VoxelArtSystems.Saving.ReadAsync(source, Serialization.RawVoxelArt, cancellationToken);

            return readResult.IsSuccessful
                ? new ModelGetResult(true, readResult.Model)
                : new ModelGetResult(false);
        }
    }
}