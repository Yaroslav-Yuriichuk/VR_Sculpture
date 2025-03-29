using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using VoxelArt.Runtime;
using VoxelArt.Runtime.Saving;

namespace _Sculpture.Runtime.Content.Predefined.Services
{
    public sealed class AddressablesPredefinedContentService : IPredefinedContentService
    {
        private readonly PredefinedModelsAssetReference _predefinedModelsAssetReference;
        private readonly DefaultModelsAsset _defaultModels;

        public AddressablesPredefinedContentService(PredefinedModelsAssetReference predefinedModelsAssetReference,
            DefaultModelsAsset defaultModels)
        {
            _predefinedModelsAssetReference = predefinedModelsAssetReference;
            _defaultModels = defaultModels;
        }

        public async UniTask<ModelsGetResult> GetModelsAsync(CancellationToken cancellationToken = default)
        {
            AsyncOperationHandle<PredefinedModelsAsset> operation = Addressables.LoadAssetAsync<PredefinedModelsAsset>(_predefinedModelsAssetReference.AssetGUID);

            try
            {
                await operation.ToUniTask(cancellationToken: cancellationToken);
            }
            catch (OperationCanceledException)
            {
                Addressables.Release(operation);
                throw;
            }

            PredefinedModelsAsset modelsAsset = operation.Result;

            IEnumerable<ModelDescriptor> descriptors;

            if (operation.Status == AsyncOperationStatus.Succeeded && modelsAsset != null)
            {
                descriptors = modelsAsset.Descriptors;
                return new ModelsGetResult(true, descriptors);
            }

            descriptors = _defaultModels.Descriptors;
            return new ModelsGetResult(true, descriptors);
        }

        public async UniTask<ModelGetResult> GetModelAsync(ModelDescriptor descriptor, CancellationToken cancellationToken = default)
        {
            AsyncOperationHandle<PredefinedModelsAsset> modelsAssetOperation = Addressables.LoadAssetAsync<PredefinedModelsAsset>(_predefinedModelsAssetReference.AssetGUID);

            try
            {
                await modelsAssetOperation.ToUniTask(cancellationToken: cancellationToken);
            }
            catch (OperationCanceledException)
            {
                Addressables.Release(modelsAssetOperation);
                throw;
            }

            PredefinedModelsAsset modelsAsset = modelsAssetOperation.Result;

            bool isAddressableAsset = false;
            TextAsset textAsset = null;

            if (modelsAssetOperation.Status == AsyncOperationStatus.Succeeded && modelsAsset != null)
            {
                TextAssetReference assetReference = modelsAsset.GetAssetReference(descriptor);

                if (assetReference != null)
                {
                    AsyncOperationHandle<TextAsset> textAssetOperation = Addressables.LoadAssetAsync<TextAsset>(assetReference.AssetGUID);

                    try
                    {
                        await textAssetOperation.ToUniTask(cancellationToken: cancellationToken);
                    }
                    catch (OperationCanceledException)
                    {
                        Addressables.Release(textAssetOperation);
                        throw;
                    }

                    textAsset = textAssetOperation.Result;
                    isAddressableAsset = true;
                }
            }
            else
            {
                textAsset = _defaultModels.GetAsset(descriptor);
            }

            try
            {
                ReadSource source = ReadSource.TextAsset(textAsset);
                ModelReadResult readResult = await VoxelArtSystems.Saving.ReadAsync(source, Serialization.RawVoxelArt, cancellationToken);

                return readResult.IsSuccessful
                    ? new ModelGetResult(true, readResult.Model)
                    : new ModelGetResult(false);
            }
            finally
            {
                if (isAddressableAsset)
                {
                    Addressables.Release(modelsAsset);
                    Addressables.Release(textAsset);
                }
            }
        }
    }
}