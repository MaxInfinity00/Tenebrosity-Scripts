using UnityEngine;
using Team11.Interactions;

namespace Team11.Health
{
    public class PlayerHealth : LightChecker
    {
        [SerializeField] private float maxHealth = 10;
        [SerializeField] private float damageDelay = 1;
        [SerializeField] private float healthDecreaseSpeed = 0.5f;
        [SerializeField] private float healthIncreaseSpeed = 1f;
        [SerializeField] private bool regenerateHealth;

        private FMOD.Studio.EventInstance _damageDarknessAudio;
        private bool _isHoldingTorch;
        private float _currentHealth;
        private bool _isBeingDamaged;
        private float _damageCountdown;
        private float _regenerationCountdown;
        private float _cachedHealthDecreaseSpeed;

        public float CurrentHealth => _currentHealth;
        public float MaxHealth => maxHealth;
        public bool IsBeingDamaged => _isBeingDamaged;

        private void Start()
        {
            _damageCountdown = damageDelay;
            _currentHealth = maxHealth;
            _damageDarknessAudio = FMODUnity.RuntimeManager.CreateInstance("event:/Player/Damage Darkness");
            GetComponent<LitomancerInteractions>().OnHaveTorch += hasTorch => _isHoldingTorch = hasTorch;
            _cachedHealthDecreaseSpeed = healthDecreaseSpeed;
        }

        private void Update()
        {
            if (IsInSafety || _isHoldingTorch)
            {
                if (IsPlaying(_damageDarknessAudio))
                    _damageDarknessAudio.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                HandleSafety();
                return;
            }

            HandleUnSafety();
            if (!IsPlaying(_damageDarknessAudio))
                _damageDarknessAudio.start();
        }

        private void HandleSafety()
        {
            _damageCountdown = damageDelay;
            _isBeingDamaged = false;

            if (regenerateHealth && _currentHealth < maxHealth) 
                _currentHealth += healthIncreaseSpeed * Time.deltaTime;
        }

        private void HandleUnSafety()
        {
            if (!_isBeingDamaged)
                _damageCountdown -= Time.deltaTime;
            if (_damageCountdown <= 0)
            {
                _isBeingDamaged = true;
                _currentHealth -= healthDecreaseSpeed * Time.deltaTime;
                _currentHealth = Mathf.Max(0, _currentHealth);
            }
        }

        private bool IsPlaying(FMOD.Studio.EventInstance instance)
        {
            FMOD.Studio.PLAYBACK_STATE state;
            instance.getPlaybackState(out state);
            return state != FMOD.Studio.PLAYBACK_STATE.STOPPED;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = IsInSafety ? Color.green : Color.red;
            Gizmos.DrawWireSphere(transform.position + Vector3.up * 0.93f, 1);
        }

        public void Revive()
        {
            _currentHealth = maxHealth;
        }

        public void Invincibility(bool invincible)
        {
            if (invincible)
                healthDecreaseSpeed = 0;
            else
                healthDecreaseSpeed = _cachedHealthDecreaseSpeed;
        }
    }
}