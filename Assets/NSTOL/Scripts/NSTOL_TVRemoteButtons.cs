using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class NSTOL_TVRemoteButtons : XRBaseInteractable
{
    [SerializeField]
    private string buttonFunction = "Pause";

    protected override void OnSelectEnter(XRBaseInteractor controller)
    //protected override void OnSelectEnter(XRBaseInteractor controller)
    {
        DebugHelpers.Log("Button Push Triggered on " + controller.name + "with function: " + this.buttonFunction);
        //all hands have an VORGrabber object on them. So this way we know it was their hand and not their face or player collider.
        var tv = GameObject.Find("TVScreen");
        var syncVidComponent = tv.GetComponent<NSTOL_SynchronousVideoTest>();
        switch (this.buttonFunction)
        {
            case "Play":
                DebugHelpers.Log("Play button pushed");
                Debug.LogError("Play button pushed");
                syncVidComponent._playState = 1;
                break;
            case "Pause":
                DebugHelpers.Log("Pause button pushed");
                Debug.LogError("Pause button pushed");
                syncVidComponent._playState = 2;
                break;
            case "Stop":
                DebugHelpers.Log("Stop button pushed");
                Debug.LogError("Stop button pushed");
                syncVidComponent._playState = 0;
                break;
            case "wtf":
                DebugHelpers.Log("wtf");
                Debug.LogError("wtf");
                syncVidComponent._playState = 1;
                break;
            default:
                DebugHelpers.Log("DEFAULT ON protected override void OnActivate" + this.buttonFunction);
                Debug.LogError("DEFAULT ON protected override void OnActivate" + this.buttonFunction);
                break;

        }
    }

}
