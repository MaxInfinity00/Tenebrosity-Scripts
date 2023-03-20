using UnityEngine;

namespace Team11.Health
{
    public class LightChecker : MonoBehaviour
    {
        private int _safetyCount;

        protected bool IsInSafety => _safetyCount > 0;

        private void OnTriggerEnter(Collider other)
        {
            var safeArea = other.gameObject.GetComponent<SafeArea>();
            if (safeArea == null) return;

            _safetyCount++;
        }

        private void OnTriggerExit(Collider other)
        {
            var safeArea = other.gameObject.GetComponent<SafeArea>();
            if (safeArea == null) return;

            _safetyCount--;
        }
    }
}