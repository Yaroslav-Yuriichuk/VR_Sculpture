using System;
using _Sculpture.Runtime.UI.Services;
using UnityEngine;

namespace _Sculpture.Runtime.UI
{
    [Serializable]
    public struct PageLinkData
    {
        [field: SerializeField] public PageIdentifierData PageIdentifierData { get; private set; }
        [field: SerializeField] public MonoBehaviourPage Page { get; private set; }

        [field: Space]
        [field: SerializeField] public bool OpenOnStart { get; private set; }
        [field: SerializeField] public bool CloseOnDestroy { get; private set; }
    }

    [DefaultExecutionOrder(-1)]
    public abstract class PagesLinker : MonoBehaviour
    {
        [SerializeField] private PageLinkData[] _links;

        protected abstract IUIService UIService { get; }

        private void Awake()
        {
            foreach (PageLinkData data in _links)
            {
                if (!data.PageIdentifierData.TryGetIdentifier(out Enum identifier))
                {
                    Debug.LogError($"Failed to retrieve enum value for data with type {data.PageIdentifierData.EnumTypeName} and value {data.PageIdentifierData.EnumValueName}.");
                    continue;
                }

                UIService.Link(data.Page, identifier);
            }
        }

        private void Start()
        {
            foreach (PageLinkData data in _links)
            {
                if (!data.PageIdentifierData.TryGetIdentifier(out Enum identifier))
                {
                    Debug.LogError($"Failed to retrieve enum value for data with type {data.PageIdentifierData.EnumTypeName} and value {data.PageIdentifierData.EnumValueName}.");
                    continue;
                }

                if (data.OpenOnStart)
                {
                    UIService.Open(identifier);
                }
            }
        }

        private void OnDestroy()
        {
            foreach (PageLinkData data in _links)
            {
                if (!data.PageIdentifierData.TryGetIdentifier(out Enum identifier))
                {
                    Debug.LogError($"Failed to retrieve enum value for data with type {data.PageIdentifierData.EnumTypeName} and value {data.PageIdentifierData.EnumValueName}.");
                    continue;
                }

                if (data.CloseOnDestroy)
                {
                    UIService.Close(identifier);
                }

                UIService.Unlink(identifier);
            }
        }
    }
}