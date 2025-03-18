using VoxelArt.Runtime;

namespace _Sculpture.Runtime.Content
{
    public sealed class ModelGetResult
    {
        public bool IsSuccessful { get; }

        public Model Model { get; }

        public ModelGetResult(bool isSuccessful, Model model = null)
        {
            IsSuccessful = isSuccessful;
            Model = model;
        }
    }
}