using System.Threading;
using Cysharp.Threading.Tasks;
using VoxelArt.Runtime;

namespace _Sculpture.Runtime.Content.Services
{
    public interface IContentService
    {
        UniTask<ModelAddResult> AddModelAsync(string name, Model model, CancellationToken cancellationToken = default);

        UniTask<ModelsGetResult> GetModelsAsync(CancellationToken cancellationToken = default);

        UniTask<ModelGetResult> GetModelAsync(ModelDescriptor descriptor, CancellationToken cancellationToken = default);

        UniTask<ModelUpdateResult> UpdateModelAsync(ModelDescriptor descriptor, Model model, CancellationToken cancellationToken = default);
    }
}