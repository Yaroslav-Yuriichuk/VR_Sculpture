using System.Collections.Generic;

namespace _Sculpture.Runtime.Content
{
    public sealed class ModelsGetResult
    {
        public bool IsSuccessful { get; }

        public IEnumerable<ModelDescriptor> Descriptors { get; }

        public ModelsGetResult(bool isSuccessful, IEnumerable<ModelDescriptor> descriptors = null)
        {
            IsSuccessful = isSuccessful;
            Descriptors = descriptors;
        }
    }
}