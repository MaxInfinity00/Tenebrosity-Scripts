using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class TriggerSound : MonoBehaviour
{

    [SerializeField] private string soundPath;
    
    public void PlaySound()
    {
        RuntimeManager.PlayOneShot(soundPath);
    }
}
