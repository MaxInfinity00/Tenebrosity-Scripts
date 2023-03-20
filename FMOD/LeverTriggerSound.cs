using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class LeverTriggerSound : MonoBehaviour
{
    public void TriggerLeverSound()
    {
        RuntimeManager.PlayOneShot("event:/Interactables/Lever-Crank", transform.position);
    }
}
