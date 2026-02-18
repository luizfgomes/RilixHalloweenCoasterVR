using Dreamteck.Splines;
using UnityEngine;

namespace RilixHalloweenChallenge.Player.Movement
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

        public static RideController Instance { get; private set; }

        private void Awake()
        {
            _currentSpeed = _baseSpeed;

            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }

        private void Update()
        {
            ApplySlopePhysics();
        }

        private void LateUpdate()
        {
            _splineFollower.followSpeed = _currentSpeed;
        }

        #region Public Ride

        public float GetProgress()
        {
            return (float)_splineFollower.result.percent;
        }

        public Vector3 GetPoint(float percent)
        {
            SplineSample sample = new SplineSample();
            _splineFollower.spline.Evaluate(percent, ref sample);
            return sample.position;
        }

        public Vector3 GetDirection(float percent)
        {
            SplineSample sample = new SplineSample();
            _splineFollower.spline.Evaluate(percent, ref sample);
            return sample.forward;
        }

        public float GetSpeed()
        {
            return _currentSpeed;
        }

        public Transform GetRiderTransform()
        {
            return _splineFollower.transform;
        }

        #endregion

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