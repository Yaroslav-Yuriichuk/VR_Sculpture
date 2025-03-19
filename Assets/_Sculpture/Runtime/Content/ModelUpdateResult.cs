namespace _Sculpture.Runtime.Content
{
    public sealed class ModelUpdateResult
    {
        public bool IsSuccessful { get; }

        public ModelDescriptor Descriptor { get; }

        public ModelUpdateResult(bool isSuccessful, ModelDescriptor descriptor = null)
        {
            IsSuccessful = isSuccessful;
            Descriptor = descriptor;
        }
    }
}