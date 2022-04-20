using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class GlobalVolumeHandler : MonoBehaviour
{
    public Volume globalVolume;
    private VolumeProfile globalVolProfile;

    private void Awake()
    {
        globalVolProfile = globalVolume.profile;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (globalVolProfile.TryGet(out ChromaticAberration chromAberr))
            {
                chromAberr.active = !chromAberr.active;
            }
        }
    }
}
