using System;
using System.Threading;
using _Sculpture.Runtime.Auth.Services;
using Cysharp.Threading.Tasks;
using Firebase.Storage;
using VoxelArt.Runtime;
using VoxelArt.Runtime.Serialization;

namespace _Sculpture.Runtime.Content.Remote.Services
{
    public sealed class FirebaseRemoteStorageService : IRemoteStorageService
    {
        private readonly IAuthService _authService;

        private FirebaseStorage Storage => FirebaseStorage.DefaultInstance;

        public FirebaseRemoteStorageService(IAuthService authService)
        {
            _authService = authService;
        }

        public async UniTask<ModelAddResult> AddModelAsync(Model model, ModelDescriptor descriptor, CancellationToken cancellationToken = default)
        {
            if (_authService.CurrentUser == null)
            {
                return new ModelAddResult(false);
            }

            string userId = _authService.CurrentUser.Id;
            StorageReference storageReference = Storage.GetReference($"users/{userId}/models/{descriptor.Id}.voxelart");

            try
            {
                SerializationResult<byte[]> serializationResult = VoxelArtSystems.Serialization.ToBytes(model);

                if (!serializationResult.IsSuccessful)
                {
                    return new ModelAddResult(false);
                }

                await storageReference.PutBytesAsync(serializationResult.Value)
                    .AsUniTask()
                    .AttachExternalCancellation(cancellationToken);

                return new ModelAddResult(true, descriptor);
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch
            {
                return new ModelAddResult(false);
            }
        }

        public async UniTask<ModelGetResult> GetModelAsync(ModelDescriptor descriptor, CancellationToken cancellationToken = default)
        {
            if (_authService.CurrentUser == null)
            {
                return new ModelGetResult(false);
            }

            string userId = _authService.CurrentUser.Id;
            StorageReference storageReference = Storage.GetReference($"users/{userId}/models/{descriptor.Id}.voxelart");

            try
            {
                byte[] bytes = await storageReference.GetBytesAsync(int.MaxValue)
                    .AsUniTask()
                    .AttachExternalCancellation(cancellationToken);

                SerializationResult<Model> serializationResult = VoxelArtSystems.Serialization.FromBytes<Model>(bytes);

                if (!serializationResult.IsSuccessful)
                {
                    return new ModelGetResult(false);
                }

                return new ModelGetResult(true, serializationResult.Value);
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch
            {
                return new ModelGetResult(false);
            }
        }

        public async UniTask<ModelUpdateResult> UpdateModelAsync(Model model, ModelDescriptor descriptor, CancellationToken cancellationToken = default)
        {
            if (_authService.CurrentUser == null)
            {
                return new ModelUpdateResult(false);
            }

            string userId = _authService.CurrentUser.Id;
            StorageReference storageReference = Storage.GetReference($"users/{userId}/models/{descriptor.Id}.voxelart");

            try
            {
                SerializationResult<byte[]> serializationResult = VoxelArtSystems.Serialization.ToBytes(model);

                if (!serializationResult.IsSuccessful)
                {
                    return new ModelUpdateResult(false);
                }

                await storageReference.PutBytesAsync(serializationResult.Value)
                    .AsUniTask()
                    .AttachExternalCancellation(cancellationToken);

                return new ModelUpdateResult(true, descriptor);
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch
            {
                return new ModelUpdateResult(false);
            }
        }

        public async UniTask<ModelDeleteResult> DeleteModelAsync(ModelDescriptor descriptor, CancellationToken cancellationToken = default)
        {
            if (_authService.CurrentUser == null)
            {
                return new ModelDeleteResult(false);
            }

            string userId = _authService.CurrentUser.Id;
            StorageReference storageReference = Storage.GetReference($"users/{userId}/models/{descriptor.Id}.voxelart");

            try
            {
                await storageReference.DeleteAsync()
                    .AsUniTask()
                    .AttachExternalCancellation(cancellationToken);

                return new ModelDeleteResult(true);
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch
            {
                return new ModelDeleteResult(false);
            }
        }
    }
}