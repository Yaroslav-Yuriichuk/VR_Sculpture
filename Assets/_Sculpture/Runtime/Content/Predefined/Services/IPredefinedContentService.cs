using System.Threading;
using Cysharp.Threading.Tasks;

namespace _Sculpture.Runtime.Content.Predefined.Services
{
    public interface IPredefinedContentService
    {
        UniTask<ModelsGetResult> GetModelsAsync(CancellationToken cancellationToken = default);

        UniTask<ModelGetResult> GetModelAsync(ModelDescriptor descriptor, CancellationToken cancellationToken = default);
    }
}