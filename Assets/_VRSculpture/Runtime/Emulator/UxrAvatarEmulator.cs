using UltimateXR.Avatar;
using UltimateXR.Core;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _VRSculpture.Runtime.Emulator
{
    internal sealed class UxrAvatarEmulator : MonoBehaviour
    {
        [SerializeField] private float _cameraElevateHeight = 1.75f;
        [SerializeField] private Vector2 _moveSpeed = Vector2.one;
        [SerializeField] private Vector2 _headRotationSpeed = Vector2.one;

        [Space]
        [SerializeField] private Vector2 _headVerticalAngleLimit = Vector2.zero;

        [Space]
        [SerializeField] private Vector3 _leftHandCameraOffset;
        [SerializeField] private Vector3 _rightHandCameraOffset;

        [Space]
        [SerializeField] private Vector3 _leftHandLocalRotation;
        [SerializeField] private Vector3 _rightHandLocalRotation;

        [Space]
        [SerializeField] private InputActionReference _moveAction;
        [SerializeField] private InputActionReference _rotateHeadAction;
        [SerializeField] private InputActionReference _lockAction;

#if UNITY_EDITOR
        private UxrAvatar _avatar;

        private Transform _cameraTransform;
        private Transform _leftHandTransform;
        private Transform _rightHandTransform;

        private void Awake()
        {
            _avatar = GetComponent<UxrAvatar>();
        }

        private void OnEnable()
        {
            _cameraTransform = _avatar.CameraComponent.transform;
            _leftHandTransform = _avatar.LeftHandBone;
            _rightHandTransform  = _avatar.RightHandBone;

            _moveAction.action.Enable();
            _rotateHeadAction.action.Enable();
            _lockAction.action.Enable();

            UxrManager.StageUpdated += UxrManager_StageUpdated;
        }

        private void OnDisable()
        {
            _moveAction.action.Disable();
            _rotateHeadAction.action.Disable();
            _lockAction.action.Disable();

            UxrManager.StageUpdated -= UxrManager_StageUpdated;
        }

        private void UxrManager_StageUpdated(UxrUpdateStage stage)
        {
            if (_cameraTransform == null || _leftHandTransform == null || _rightHandTransform == null)
            {
                return;
            }

            if (stage == UxrUpdateStage.AvatarUsingTracking)
            {
                if (_lockAction.action.IsPressed())
                {
                    Vector2 headRotationDelta = _rotateHeadAction.action.ReadValue<Vector2>();
                    Vector3 headRotationEuler = _cameraTransform.localRotation.eulerAngles;

                    headRotationEuler.x -= headRotationDelta.y * _headRotationSpeed.y;
                    headRotationEuler.x = headRotationEuler.x <= 180 ? headRotationEuler.x : headRotationEuler.x - 360;

                    headRotationEuler.x = Mathf.Clamp(headRotationEuler.x, _headVerticalAngleLimit.x, _headVerticalAngleLimit.y);

                    _cameraTransform.localRotation = Quaternion.Euler(headRotationEuler);

                    Vector3 avatarRotationEuler = _avatar.transform.rotation.eulerAngles;

                    avatarRotationEuler.y += headRotationDelta.x * _headRotationSpeed.x;
                    _avatar.transform.rotation = Quaternion.Euler(avatarRotationEuler);

                    Vector2 movementDelta = _moveAction.action.ReadValue<Vector2>().normalized;
                    Vector3 cameraDirection = Vector3.ProjectOnPlane(_cameraTransform.forward, Vector3.up).normalized;

                    Vector3 forwardMovement = movementDelta.y * _moveSpeed.y * Time.deltaTime * cameraDirection;
                    Vector3 sideMovement = movementDelta.x * _moveSpeed.x * Time.deltaTime * new Vector3(cameraDirection.z, 0, -cameraDirection.x);

                    Vector3 movement = forwardMovement + sideMovement;
                    _avatar.transform.position += movement;

                    Quaternion headRotation = _cameraTransform.rotation;
                    Quaternion inverseAvatarRotation = Quaternion.Inverse(headRotation);

                    _cameraTransform.localPosition = new Vector3(0, _cameraElevateHeight, 0);

                    Vector3 cameraPositionLS = inverseAvatarRotation * _cameraTransform.position;
                    Vector3 leftHandPositionLS = cameraPositionLS + _leftHandCameraOffset;
                    Vector3 rightHandPositionLS = cameraPositionLS + _rightHandCameraOffset;

                    Vector3 leftHandPosition = headRotation * leftHandPositionLS;
                    Vector3 rightHandPosition = headRotation * rightHandPositionLS;

                    _leftHandTransform.position = leftHandPosition;
                    _rightHandTransform.position = rightHandPosition;

                    _leftHandTransform.localRotation = Quaternion.Euler(_leftHandLocalRotation);
                    _rightHandTransform.localRotation = Quaternion.Euler(_rightHandLocalRotation);

                    Cursor.visible = false;
                    Cursor.lockState = CursorLockMode.Locked;
                }
                else
                {
                    Quaternion headRotation = _cameraTransform.rotation;
                    Quaternion inverseAvatarRotation = Quaternion.Inverse(headRotation);

                    _cameraTransform.localPosition = new Vector3(0, _cameraElevateHeight, 0);

                    Vector3 cameraPositionLS = inverseAvatarRotation * _cameraTransform.position;
                    Vector3 leftHandPositionLS = cameraPositionLS + _leftHandCameraOffset;
                    Vector3 rightHandPositionLS = cameraPositionLS + _rightHandCameraOffset;

                    Vector3 leftHandPosition = headRotation * leftHandPositionLS;
                    Vector3 rightHandPosition = headRotation * rightHandPositionLS;

                    _leftHandTransform.position = leftHandPosition;
                    _rightHandTransform.position = rightHandPosition;

                    _leftHandTransform.localRotation = Quaternion.Euler(_leftHandLocalRotation);
                    _rightHandTransform.localRotation = Quaternion.Euler(_rightHandLocalRotation);

                    Cursor.visible = true;
                    Cursor.lockState = CursorLockMode.None;
                }
            }
        }
#endif
    }
}