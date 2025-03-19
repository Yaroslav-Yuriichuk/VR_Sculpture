using UnityEngine;

namespace _VRSculpture.Runtime.Mechanics
{
    internal sealed class RotateForever : MonoBehaviour
    {
        [SerializeField] private Vector3 _rotationSpeed;

        private void Update()
        {
            transform.Rotate(_rotationSpeed * Time.deltaTime);
        }
    }
}