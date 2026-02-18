using UnityEngine;
using UnityEngine.Events;
using RilixHalloweenChallenge.Player.Movement;
using RilixHalloweenChallenge.Ride.Core;

namespace RilixHalloweenChallenge.Ride.Track
{
    public class TrackMarker : MonoBehaviour
    {
        [SerializeField, Range(0f, 1f)]
        private float _percent;
        [SerializeField] 
        private UnityEvent _onPassed;
        [SerializeField]
        private RideEvent[] _rideEvent;

        private bool _triggered = false;

        public float Percent => _percent;

        private void Start()
        {
            _percent = FindClosestT(transform.position);
        }

        private void Update()
        {
            if (_triggered == true)
                return;

            float playerPercent = RideController.Instance.GetProgress();

            if (playerPercent >= _percent)
            {
                _triggered = true;
                _onPassed?.Invoke();
                foreach (var ride in _rideEvent)
                {
                    ride.TryExecute();
                }
            }
        }

        private float FindClosestT(Vector3 worldPos)
        {
            const int coarseSteps = 100;
            float bestT = 0f;
            float bestDist = float.MaxValue;

            for (int i = 0; i <= coarseSteps; i++)
            {
                float t = i / (float)coarseSteps;
                Vector3 p = RideController.Instance.GetPoint(t);

                float d = (p - worldPos).sqrMagnitude;
                if (d < bestDist)
                {
                    bestDist = d;
                    bestT = t;
                }
            }

            float range = 1f / coarseSteps;

            for (int i = 0; i < 5; i++)
            {
                float left = Mathf.Clamp01(bestT - range);
                float right = Mathf.Clamp01(bestT + range);

                Vector3 pL = RideController.Instance.GetPoint(left);
                Vector3 pR = RideController.Instance.GetPoint(right);

                if ((pL - worldPos).sqrMagnitude < (pR - worldPos).sqrMagnitude)
                    bestT = left;
                else
                    bestT = right;

                range *= 0.5f;
            }

            return bestT;
        }
    }
}
