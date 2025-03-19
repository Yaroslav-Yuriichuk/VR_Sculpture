namespace _Sculpture.Runtime.Content
{
    public sealed class ModelDeleteResult
    {
        public bool IsSuccessful { get; }

        public ModelDeleteResult(bool isSuccessful)
        {
            IsSuccessful = isSuccessful;
        }
    }
}