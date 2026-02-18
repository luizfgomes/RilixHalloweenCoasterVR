using UnityEngine;

namespace RilixHalloweenChallenge.Ride.Core
{
    public abstract class RideEvent : MonoBehaviour
    {
        [SerializeField]
        private bool _triggerOnlyOnce = true;

        private bool _hasTriggered;

        public void TryExecute()
        {
            if (_triggerOnlyOnce && _hasTriggered)
            {
                return;
            } 

            _hasTriggered = true;
            Execute();
        }

        protected abstract void Execute();
    }
}
