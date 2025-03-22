using System.Threading;
using Cysharp.Threading.Tasks;
using VoxelArt.Runtime;

namespace _Sculpture.Runtime.Content.Remote.Services
{
    public interface IRemoteStorageService
    {
        UniTask<ModelGetResult> GetModelAsync(ModelDescriptor descriptor, CancellationToken cancellationToken = default);

        UniTask<ModelAddResult> AddModelAsync(Model model, ModelDescriptor descriptor, CancellationToken cancellationToken = default);

        UniTask<ModelUpdateResult> UpdateModelAsync(Model model, ModelDescriptor descriptor, CancellationToken cancellationToken = default);

        UniTask<ModelDeleteResult> DeleteModelAsync(ModelDescriptor descriptor, CancellationToken cancellationToken = default);
    }
}