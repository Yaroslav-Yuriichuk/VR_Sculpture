namespace _Sculpture.Runtime.Content
{
    public sealed class ModelAddResult
    {
        public bool IsSuccessful { get; }

        public ModelDescriptor Descriptor { get; }

        public ModelAddResult(bool isSuccessful, ModelDescriptor descriptor = null)
        {
            IsSuccessful = isSuccessful;
            Descriptor = descriptor;
        }
    }
}