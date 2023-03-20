using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using STOP_MODE = FMOD.Studio.STOP_MODE;

public class ObjectDragSound : MonoBehaviour
{
    public Rigidbody rb; // The Rigidbody component to check
    private FMOD.Studio.EventInstance dragSound;

    void Start()
    {
        dragSound = RuntimeManager.CreateInstance("event:/Interactables/Object Drag");
        if (rb == null)
        {
            rb = GetComponent<Rigidbody>(); // Get the Rigidbody component if it's not already set
        }
    }

    private void Update()
    {
        TriggerDragSound();
    }

    bool HasNonZeroVelocity()
    {
        return rb.velocity.magnitude > 0.1f; // Check if the Rigidbody's velocity is greater than zero
    }

    public void TriggerDragSound()
    {
        var hasNonZeroVelocity = HasNonZeroVelocity();
        if (hasNonZeroVelocity)
        {   
            if(!IsPlaying(dragSound))
            {
                RuntimeManager.AttachInstanceToGameObject(dragSound, GetComponent<Transform>(), rb);
                dragSound.set3DAttributes((RuntimeUtils.To3DAttributes(gameObject)));
                dragSound.start();
                dragSound.release();
                print(hasNonZeroVelocity);
            }
            return;
        }
        
        if (IsPlaying(dragSound))
        {
            print(hasNonZeroVelocity);
            dragSound.stop(STOP_MODE.ALLOWFADEOUT);
            dragSound.release();
        }
    }

    private bool IsPlaying(FMOD.Studio.EventInstance instance)
    {
        FMOD.Studio.PLAYBACK_STATE state;
        instance.getPlaybackState(out state);
        return state != FMOD.Studio.PLAYBACK_STATE.STOPPED;
    }
}
