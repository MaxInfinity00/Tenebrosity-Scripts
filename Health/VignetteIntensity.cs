using Team11.Health;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class VignetteIntensity : MonoBehaviour
{
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private float increaseSpeed = 1;
    [SerializeField] private bool useHealthRegen;

    private Vignette _vignette;

    void Start()
    {
        PostProcessVolume volume = GetComponent<PostProcessVolume>();
        volume.profile.TryGetSettings(out _vignette);
    }

    void Update()
    {
        if (useHealthRegen)
        {
            _vignette.intensity.value = GetIntensity();
            return;
        }

        if (playerHealth.IsBeingDamaged)
            _vignette.intensity.value = GetIntensity();
        else if (_vignette.intensity.value >= 0)
            _vignette.intensity.value -= increaseSpeed * Time.deltaTime;
    }

    float GetIntensity()
    {
        float t = playerHealth.CurrentHealth / playerHealth.MaxHealth;
        t = 1 - t;

        return t;
    }

    public void SetPlayerHealth(PlayerHealth playerHealth)
    {
        this.playerHealth = playerHealth;
        this.enabled = true;
    }
}