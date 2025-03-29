using System.Threading;
using UnityEngine;

namespace _Sculpture.Runtime.UI
{
    public class MonoBehaviourPageElement : MonoBehaviour
    {
        private CancellationTokenSource _openCts;

        protected CancellationToken OpenCancellationToken => _openCts?.Token ?? new CancellationToken(true);

        public virtual void HandleOpen(IOpenArguments arguments)
        {
            _openCts?.Cancel();
            _openCts = CancellationTokenSource.CreateLinkedTokenSource(destroyCancellationToken);
        }

        public virtual void HandleReload(IReloadArguments arguments)
        {
            _openCts?.Cancel();
            _openCts = CancellationTokenSource.CreateLinkedTokenSource(destroyCancellationToken);
        }

        public virtual void HandleClose(ICloseArguments arguments)
        {
            _openCts?.Cancel();
        }
    }
}