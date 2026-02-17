using Dreamteck.Splines;
using UnityEngine;

namespace RilixHalloweenChallenge
{
    public class RideController : MonoBehaviour
    {
        [SerializeField]
        private SplineFollower _splineFollower;

        #region Speed and Acceleration Parameters
        [SerializeField]
        private float _acceleration;
        [SerializeField]
        private float _maxAcceleration = 15f;
        [SerializeField]
        private float _baseSpeed = 6f;
        [SerializeField]
        private float _gravityInfluence = 20f;
        [SerializeField]
        private float _brakeForce = 15f;

        private float _currentSpeed;
        #endregion

        private void Awake()
        {
            _currentSpeed = _baseSpeed;
        }

        private void Update()
        {
            ApplySlopePhysics();
        }

        private void LateUpdate()
        {
            _splineFollower.followSpeed = _currentSpeed;
        }

        void ApplySlopePhysics()
        {
            Vector3 dir = _splineFollower.result.forward;

            float slope = Vector3.Dot(dir, Vector3.down);

            float _targetSpeed = _currentSpeed + slope * _gravityInfluence * Time.deltaTime;

            _acceleration = (_targetSpeed - _currentSpeed) / Time.deltaTime;
            _acceleration = Mathf.Clamp(_acceleration, -_brakeForce, _maxAcceleration);

            _currentSpeed += slope * _gravityInfluence * Time.deltaTime;
            _currentSpeed = Mathf.Max(1f, _currentSpeed);
        }
    }
}