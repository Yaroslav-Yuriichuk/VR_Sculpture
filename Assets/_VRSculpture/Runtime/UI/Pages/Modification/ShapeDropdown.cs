using System;
using _Sculpture.Runtime.UI;
using TMPro;
using UnityEngine;
using VoxelArt.Runtime;
using VoxelArt.Runtime.Modification;
using VoxelArt.Runtime.Modification.Components;

namespace _VRSculpture.Runtime.UI.Pages.Modification
{
    [RequireComponent(typeof(TMP_Dropdown))]
    internal sealed class ShapeDropdown : MonoBehaviourPageElement
    {
        [SerializeField] private TMP_Dropdown _dropdown;
        [SerializeField] private ModificationOperation _operation;

        public override void HandleOpen(IOpenArguments arguments)
        {
            base.HandleOpen(arguments);

            _dropdown.options.Clear();

            foreach (ModificationOption modificationOption in Enum.GetValues(typeof(ModificationOption)))
            {
                _dropdown.options.Add(new TMP_Dropdown.OptionData(modificationOption.ToString()));
            }

            if (TryGetOption(out ModificationOption option))
            {
                int index = _dropdown.options.FindIndex(o => o.text == option.ToString());

                _dropdown.SetValueWithoutNotify(index);
                _dropdown.RefreshShownValue();
            }

            _dropdown.onValueChanged.AddListener(UpdateOption);
        }

        public override void HandleClose(ICloseArguments arguments)
        {
            base.HandleClose(arguments);
            _dropdown.onValueChanged.RemoveListener(UpdateOption);
        }

        private void UpdateOption(int index)
        {
            ModificationOption option;

            try
            {
                option = (ModificationOption)Enum.Parse(typeof(ModificationOption), _dropdown.options[index].text);
            }
            catch (Exception)
            {
                return;
            }

            SetOption(option);
        }

        private bool TryGetOption(out ModificationOption option)
        {
            option = default;

            if (VoxelArtSystems.Components.TryGetModifierComponent(c => c.ModificationSettings.Operation == _operation,
                    out GeneralTriggerModifierComponent modifierComponent))
            {
                option = modifierComponent.ModificationSettings.Option;
                return true;
            }

            return false;
        }

        private void SetOption(ModificationOption option)
        {
            if (VoxelArtSystems.Components.TryGetModifierComponent(c => c.ModificationSettings.Operation == _operation,
                    out GeneralTriggerModifierComponent modifierComponent))
            {
                modifierComponent.ModificationSettings.Option = option;
            }
        }
    }
}