using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightHandler : MonoBehaviour
{
    [SerializeField] private List<Light> lights;

    public void ToggleLights(bool lightState)
    {
        foreach (Light light in lights)
        {
            light.enabled = lightState;
        }
    }
}