using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class TVRemoteControl : MonoBehaviour
{
    [SerializeField]
    private XRController controller;    // name it "RightHand Controller"
    private XRRayInteractor interactor = null;

    private GameObject tv;  // name it "TVScreen"
    
    void Start()
    {
        DebugHelpers.Log("starting tv remote control");
        if (!controller)
        {
            controller = GameObject.Find("RightHand Controller").GetComponent<XRController>();
        }

        interactor = controller.GetComponent<XRRayInteractor>();

        // get a reference for the tv
        if (!tv)
        {
            tv = GameObject.Find("TVScreen");
        }

        if (!tv || !controller)
        {
            throw new System.Exception("TV or Controller not found. TVRemoteControl failed to start. Check the TVRemoteControl.cs file and confirm you didn't rename the game objects");
        }

    }
        // Update is called once per frame
        void Update()
    {
        CheckForTVCommands();
    }

    public void CheckForTVCommands()
    {

        // trigger button pressed
        if (controller.inputDevice.TryGetFeatureValue(CommonUsages.triggerButton, out bool trigger))
        {
            if (trigger)
            {
                DebugHelpers.Log("Primary A Button Capacitive touch: " + trigger);
                if (tv.GetComponent<UnityEngine.Video.VideoPlayer>().isPlaying == true)
                {
                    tv.GetComponent<UnityEngine.Video.VideoPlayer>().Pause();
                } else
                {
                    tv.GetComponent<UnityEngine.Video.VideoPlayer>().Play();
                }
            }
        }
    }

    public void SwapVideo()
    {
        // change aspect ratio of texture VideoTexture
        // change video clip in VideoPlayer component
        // make sure VideoMaterial's Rendering Maps are set to the VideoTexture still
        // make sure the VideoMaterial is still assigned to the material of MeshRenderer component
    }
}
