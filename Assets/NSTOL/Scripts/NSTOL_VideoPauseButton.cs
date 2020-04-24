using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class NSTOL_VideoPauseButton : XRBaseInteractable
{

    protected override void OnSelectEnter(XRBaseInteractor controller)
    {
        var tv = GameObject.Find("TVScreen");
        var syncVidComponent = tv.GetComponent<NSTOL_SynchronousVideoTest>();
        DebugHelpers.Log("Pause button pushed");
        Debug.LogError("Pause button pushed");
        syncVidComponent._playState = 2;
    }

}
