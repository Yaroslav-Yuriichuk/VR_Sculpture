using System.Threading;
using _Sculpture.Runtime.Content;
using UnityEngine;

namespace _VRSculpture.Runtime.UI
{
    internal abstract class ModelView : MonoBehaviour
    {
        public ModelDescriptor Descriptor { get; private set; }

        private CancellationTokenSource _activeCts;

        protected CancellationToken ActiveCancellationToken => _activeCts?.Token ?? new CancellationToken(true);

        public virtual void Initialize(ModelDescriptor descriptor)
        {
            Descriptor = descriptor;

            _activeCts?.Cancel();
            _activeCts = CancellationTokenSource.CreateLinkedTokenSource(destroyCancellationToken);
        }

        public virtual void Uninitialize()
        {
            _activeCts?.Cancel();
        }
    }
}