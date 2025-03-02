using System;

using UnityEngine;

namespace _Sculpture.Runtime.UI
{
    [Serializable]
    public struct PageLinkData
    {
        [field: SerializeField] public string EnumTypeName { get; private set; }
        [field: SerializeField] public string EnumAssemblyName { get; private set; }
        [field: SerializeField] public string EnumValueName { get; private set; }

        [field: SerializeField] public MonoBehaviourPage Page { get; private set; }

        [field: SerializeField] public bool OpenOnStart { get; private set; }
        [field: SerializeField] public bool CloseOnDestroy { get; private set; }

        public readonly bool TryGetEnumValue(out Enum value)
        {
            value = default;

            try
			{
                Type type = Type.GetType($"{EnumTypeName}, {EnumAssemblyName}");

                value = (Enum)Enum.Parse(type!, EnumValueName);
                return true;
            }
            catch (Exception)
			{
                return false;
            }
        }
    }

    public abstract class PagesLinker : MonoBehaviour
    {
        [SerializeField] private PageLinkData[] _links;

        protected abstract IUIService UIService { get; }

        private void Awake()
        {
            foreach (PageLinkData data in _links)
            {
                if (!data.TryGetEnumValue(out Enum value))
                {
                    Debug.LogError($"Failed to retrieve enum value for data with type {data.EnumTypeName} and value {data.EnumValueName}.");
                    continue;
                }

                UIService.Link(data.Page, value);
            }
        }

        private void Start()
        {
            foreach (PageLinkData data in _links)
            {
                if (!data.TryGetEnumValue(out Enum value))
                {
                    Debug.LogError($"Failed to retrieve enum value for data with type {data.EnumTypeName} and value {data.EnumValueName}.");
                    continue;
                }

                if (data.OpenOnStart)
                {
                    UIService.Open(value);
                }
            }
        }

        private void OnDestroy()
        {
            foreach (PageLinkData data in _links)
            {
                if (!data.TryGetEnumValue(out Enum value))
                {
                    Debug.LogError($"Failed to retrieve enum value for data with type {data.EnumTypeName} and value {data.EnumValueName}.");
                    continue;
                }

                if (data.CloseOnDestroy)
                {
                    UIService.Close(value);
                }

                UIService.Unlink(value);
            }
        }
    }
}