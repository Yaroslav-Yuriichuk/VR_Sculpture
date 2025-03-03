using System;
using _Sculpture.Runtime.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VoxelArt.Runtime;
using VoxelArt.Runtime.Modification;
using VoxelArt.Runtime.Modification.Components;

namespace _VRSculpture.Runtime.UI
{
    internal sealed class AxisSlider : MonoBehaviourPageElement
    {
        private enum Axis
        {
            X,
            Y,
            Z,
        }

        [SerializeField] private ModificationOperation _operation;
        [SerializeField] private ModificationOption _option;

        [Space]
        [SerializeField] private Axis _axis;

        [Space]
        [SerializeField] private TMP_Text _text;
        [SerializeField] private Slider _slider;

        public override void HandleOpen(IOpenArguments arguments)
        {
            base.HandleOpen(arguments);

            if (TryGetParameter(out float parameter))
            {
                _text.text = $"{parameter:F2}";
                _slider.SetValueWithoutNotify(parameter);
            }

            _slider.onValueChanged.AddListener(UpdateParameter);
        }

        public override void HandleClose(ICloseArguments arguments)
        {
            base.HandleClose(arguments);
            _slider.onValueChanged.RemoveListener(UpdateParameter);
        }

        private void UpdateParameter(float value)
        {
            SetParameter(value);
            _text.text = $"{value:F2}";
        }

        private bool TryGetParameter(out float parameter)
        {
            parameter = default;

            if (VoxelArtSystems.Components.TryGetModifierComponent(c => c.ModificationSettings.Operation == _operation,
                    out GeneralTriggerModifierComponent modifierComponent))
            {
                Vector3DFloat size = _option switch
                {
                    ModificationOption.Parallelepiped => modifierComponent.ModificationSettings.ParallelepipedHalfBounds,
                    ModificationOption.Ellipsoid => modifierComponent.ModificationSettings.EllipsoidRadius,
                    _ => throw new ArgumentOutOfRangeException()
                };

                parameter = _axis switch
                {
                    Axis.X => size.X,
                    Axis.Y => size.Y,
                    Axis.Z => size.Z,
                    _ => throw new ArgumentOutOfRangeException()
                };

                return true;
            }

            return false;
        }

        private void SetParameter(float parameter)
        {
            if (VoxelArtSystems.Components.TryGetModifierComponent(c => c.ModificationSettings.Operation == _operation,
                    out GeneralTriggerModifierComponent modifierComponent))
            {
                Vector3DFloat size = _option switch
                {
                    ModificationOption.Parallelepiped => modifierComponent.ModificationSettings.ParallelepipedHalfBounds,
                    ModificationOption.Ellipsoid => modifierComponent.ModificationSettings.EllipsoidRadius,
                    _ => throw new ArgumentOutOfRangeException()
                };

                size = _axis switch
                {
                    Axis.X => size.WithX(parameter),
                    Axis.Y => size.WithY(parameter),
                    Axis.Z => size.WithZ(parameter),
                    _ => throw new ArgumentOutOfRangeException()
                };

                switch (_option)
                {
                    case ModificationOption.Parallelepiped:
                        modifierComponent.ModificationSettings.ParallelepipedHalfBounds = size;
                        break;
                    case ModificationOption.Ellipsoid:
                        modifierComponent.ModificationSettings.EllipsoidRadius = size;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
    }
}