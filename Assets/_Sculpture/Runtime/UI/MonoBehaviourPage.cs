using System.Threading;
using UnityEngine;

namespace _Sculpture.Runtime.UI
{
    public class MonoBehaviourPage : MonoBehaviour, IPage
    {
        [SerializeField] private MonoBehaviourPageElement[] _elements;

        private CancellationTokenSource _openCts;

        protected CancellationToken OpenCancellationToken => _openCts?.Token ?? new CancellationToken(true);

        public bool IsOpen { get; private set; }

        public virtual void Open(IOpenArguments arguments)
        {
            _openCts?.Cancel();
            _openCts = CancellationTokenSource.CreateLinkedTokenSource(destroyCancellationToken);

            IsOpen = true;

            foreach (MonoBehaviourPageElement element in _elements)
            {
                element.HandleOpen(arguments);
            }
        }

        public virtual void Reload(IReloadArguments arguments)
        {
            _openCts?.Cancel();
            _openCts = CancellationTokenSource.CreateLinkedTokenSource(destroyCancellationToken);

            foreach (MonoBehaviourPageElement element in _elements)
            {
                element.HandleReload(arguments);
            }
        }

        public virtual void Close(ICloseArguments arguments)
        {
            _openCts?.Cancel();
            IsOpen = false;

            foreach (MonoBehaviourPageElement element in _elements)
            {
                element.HandleClose(arguments);
            }
        }

#if UNITY_EDITOR
        [ContextMenu("Find Elements")]
        private void FindElements()
        {
            _elements = GetComponentsInChildren<MonoBehaviourPageElement>();
        }
#endif
    }
}