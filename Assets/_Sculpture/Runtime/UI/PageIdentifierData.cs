using System;
using UnityEngine;

namespace _Sculpture.Runtime.UI
{
    [Serializable]
    public struct PageIdentifierData
    {
        [field: SerializeField] public string EnumTypeName { get; private set; }
        [field: SerializeField] public string EnumAssemblyName { get; private set; }
        [field: SerializeField] public string EnumValueName { get; private set; }

        public readonly bool TryGetIdentifier(out Enum identifier)
        {
            identifier = default;

            try
            {
                Type type = Type.GetType($"{EnumTypeName}, {EnumAssemblyName}");

                identifier = (Enum)Enum.Parse(type!, EnumValueName);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}