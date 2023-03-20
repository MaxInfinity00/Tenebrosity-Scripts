using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class SpotlightTrigger : MonoBehaviour
{
    [SerializeField] private Light lightSource;
    [SerializeField] private StudioEventEmitter on;
    
    public void LightSwitchCoroutine()
    {
        StopAllCoroutines();
        StartCoroutine(LightSwitchTrigger());
    }

    public IEnumerator LightSwitchTrigger()
    {
        FMOD.Studio.EventInstance spotlightOn = RuntimeManager.CreateInstance("event:/Interactables/SpotlightOn");
        FMOD.Studio.EventInstance spotlightOff = RuntimeManager.CreateInstance("event:/Interactables/SpotlightOff");
        
        lightSource.enabled = true;
        spotlightOn.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
        spotlightOn.start();
        yield return new WaitForSeconds(2);
        lightSource.enabled = false;
        spotlightOff.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
        spotlightOff.start();
    }
}
