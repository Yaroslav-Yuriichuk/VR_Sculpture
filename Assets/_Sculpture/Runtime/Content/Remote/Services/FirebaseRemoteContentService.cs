using System;
using System.Linq;
using System.Threading;
using _Sculpture.Runtime.Auth.Services;
using Cysharp.Threading.Tasks;
using Firebase.Database;
using Newtonsoft.Json;
using VoxelArt.Runtime;

namespace _Sculpture.Runtime.Content.Remote.Services
{
    public sealed class FirebaseRemoteContentService : IRemoteContentService
    {
        private readonly IAuthService _authService;
        private readonly IRemoteStorageService _remoteStorageService;

        public event Action<ModelDescriptor> ModelAdded;
        public event Action<ModelDescriptor> ModelDeleted;

        private DatabaseReference RootReference => FirebaseDatabase.DefaultInstance.RootReference;

        public FirebaseRemoteContentService(IAuthService authService, IRemoteStorageService remoteStorageService)
        {
            _authService = authService;
            _remoteStorageService = remoteStorageService;
        }

        public async UniTask<ModelsGetResult> GetModelsAsync(CancellationToken cancellationToken = default)
        {
            if (_authService.CurrentUser == null)
            {
                return new ModelsGetResult(false);
            }

            string userId = _authService.CurrentUser.Id;
            DatabaseReference modelsReference = RootReference.Child("users").Child(userId).Child("models");

            try
            {
                DataSnapshot dataSnapshot = await modelsReference.GetValueAsync()
                    .AsUniTask()
                    .AttachExternalCancellation(cancellationToken);

                if (!dataSnapshot.Exists)
                {
                    return new ModelsGetResult(true, Enumerable.Empty<ModelDescriptor>());
                }

                return new ModelsGetResult(true, dataSnapshot.Children
                    .Select(child => JsonConvert.DeserializeObject<ModelDescriptor>(child.GetRawJsonValue())));
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch (Exception)
            {
                return new ModelsGetResult(false);
            }
        }

        public async UniTask<ModelGetResult> GetModelAsync(ModelDescriptor descriptor, CancellationToken cancellationToken = default)
        {
            if (_authService.CurrentUser == null)
            {
                return new ModelGetResult(false);
            }

            try
            {
                ModelGetResult storageGetResult = await _remoteStorageService.GetModelAsync(descriptor, cancellationToken);

                if (!storageGetResult.IsSuccessful)
                {
                    return new ModelGetResult(false);
                }

                return new ModelGetResult(true, storageGetResult.Model);
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch (Exception)
            {
                return new ModelGetResult(false);
            }
        }

        public async UniTask<ModelAddResult> AddModelAsync(string name, Model model, CancellationToken cancellationToken = default)
        {
            if (_authService.CurrentUser == null)
            {
                return new ModelAddResult(false);
            }

            string userId = _authService.CurrentUser.Id;
            DatabaseReference modelsReference = RootReference.Child("users").Child(userId).Child("models");

            string modelId = modelsReference.Push().Key;

            ModelDescriptor modelDescriptor = new ModelDescriptor
            {
                Id = modelId,
                Name = name,
            };

            ModelAddResult storageAddResult = await _remoteStorageService.AddModelAsync(model, modelDescriptor, cancellationToken);

            if (!storageAddResult.IsSuccessful)
            {
                return new ModelAddResult(false);
            }

            string modelDescriptorJson = JsonConvert.SerializeObject(modelDescriptor);

            try
            {
                await modelsReference.Child(modelId).SetRawJsonValueAsync(modelDescriptorJson)
                    .AsUniTask()
                    .AttachExternalCancellation(cancellationToken);

                ModelAdded?.Invoke(modelDescriptor);

                return new ModelAddResult(true, modelDescriptor);
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch (Exception)
            {
                return new ModelAddResult(false);
            }
        }

        public async UniTask<ModelUpdateResult> UpdateModelAsync(ModelDescriptor descriptor, Model model, CancellationToken cancellationToken = default)
        {
            if (_authService.CurrentUser == null)
            {
                return new ModelUpdateResult(false);
            }

            ModelUpdateResult storageUpdateResult = await _remoteStorageService.UpdateModelAsync(model, descriptor, cancellationToken);

            if (!storageUpdateResult.IsSuccessful)
            {
                return new ModelUpdateResult(false);
            }

            return new ModelUpdateResult(true, descriptor);
        }

        public async UniTask<ModelDeleteResult> DeleteModelAsync(ModelDescriptor descriptor, CancellationToken cancellationToken = default)
        {
            if (_authService.CurrentUser == null)
            {
                return new ModelDeleteResult(false);
            }

            string userId = _authService.CurrentUser.Id;
            DatabaseReference modelReference = RootReference.Child("users").Child(userId).Child("models").Child(descriptor.Id);

            try
            {
                await modelReference.RemoveValueAsync()
                    .AsUniTask()
                    .AttachExternalCancellation(cancellationToken);

                ModelDeleteResult storageDeleteResult = await _remoteStorageService.DeleteModelAsync(descriptor, cancellationToken);

                if (!storageDeleteResult.IsSuccessful)
                {
                    return new ModelDeleteResult(false);
                }

                ModelDeleted?.Invoke(descriptor);

                return new ModelDeleteResult(true);
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch (Exception)
            {
                return new ModelDeleteResult(false);
            }
        }
    }
}
