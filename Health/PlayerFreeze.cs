using StarterAssets;
using UnityEngine;

namespace Team11.Health
{
    [RequireComponent(typeof(FirstPersonController), typeof(PlayerHealth))]
    public class PlayerFreeze : MonoBehaviour
    {
        [SerializeField] private AnimationCurve effectCurve;
        public float intensity;

        private PlayerHealth _health;
        private FirstPersonController _controller;

        private void Start()
        {
            _health = GetComponent<PlayerHealth>();
            _controller = GetComponent<FirstPersonController>();
        }

        private void Update()
        {
            _controller.speedMultiplier = GetSpeedMultiplier();
            intensity = GetSpeedMultiplier();
        }

        private float GetSpeedMultiplier()
        {
            float t = _health.CurrentHealth / _health.MaxHealth;
            t = Mathf.Clamp01(t);
            t = 1 - t;
            t *= effectCurve.Evaluate(t);
            t = 1 - t;

            return t;
        }
    }
}