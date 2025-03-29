using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using VoxelArt.Runtime;

namespace _Sculpture.Runtime.Content.Remote.Services
{
    public interface IRemoteContentService
    {
        event Action<ModelDescriptor> ModelAdded;

        event Action<ModelDescriptor> ModelDeleted;

        UniTask<ModelsGetResult> GetModelsAsync(CancellationToken cancellationToken = default);

        UniTask<ModelGetResult> GetModelAsync(ModelDescriptor descriptor, CancellationToken cancellationToken = default);

        UniTask<ModelAddResult> AddModelAsync(string name, Model model, CancellationToken cancellationToken = default);

        UniTask<ModelUpdateResult> UpdateModelAsync(ModelDescriptor descriptor, Model model, CancellationToken cancellationToken = default);

        UniTask<ModelDeleteResult> DeleteModelAsync(ModelDescriptor descriptor, CancellationToken cancellationToken = default);
    }
}