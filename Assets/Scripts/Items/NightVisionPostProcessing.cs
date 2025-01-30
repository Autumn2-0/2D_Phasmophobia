using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class NightVisionPostProcessing : MonoBehaviour
{
    public static NightVisionPostProcessing Instance;
    public bool NightVisionInUse = false;

    public Volume volume;
    public GameObject dimLights;
    public GameObject brightLights;
    void Start()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
        SetVolume(false);
    }

    public void SetVolume(bool NVinUse)
    {
        volume.enabled = NVinUse;
        if (NVinUse)
        {
            dimLights.SetActive(!NVinUse);
            brightLights.SetActive(NVinUse);
        }
        else
        {
            brightLights.SetActive(NVinUse);
            dimLights.SetActive(!NVinUse);
        }
    }
}
