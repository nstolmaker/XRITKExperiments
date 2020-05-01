using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class NSTOL_GhostFingerSelect : LocomotionProvider   // extending LocomotionProvider so that I can lock snap-turn movement while using the remote. This might not be the preferred pattern, i should ask someone.
{
    [SerializeField]
    private GameObject thingManager;
    [SerializeField]
    private XRGrabInteractable remoteGrabInteractable;
    [SerializeField]
    private XRController controller;
    [SerializeField]
    private XRRayInteractor interactor = null;
    [SerializeField]
    public bool beingHeld = false;
    [SerializeField]
    private Vector3 thumbPosition;
    [SerializeField]
    private GameObject ghostFinger;
    [SerializeField]
    private bool clicking = false;
    [SerializeField]
    private GameObject collidingWith;
    [SerializeField][Tooltip("Auto-populated. looks for a gameobject called 'TVScreen'")]
    GameObject tv;

    [SerializeField]
    private float hapticFingerHoverIntensity = 0.2f;


    InputDevice device;
    HapticCapabilities capabilities; // for storing the actual device. reduces lines of code for haptics.

    void Start()
    {
        if (!thingManager)
        {
            thingManager = GameObject.Find("PrototypeRemote");
        }

        if (!remoteGrabInteractable)
        {
            remoteGrabInteractable = thingManager.GetComponent<XRGrabInteractable>();
        }

        if (!ghostFinger)
        {
            ghostFinger = GameObject.Find("GhostFinger");
        }

        if (!tv)
        {
            tv = GameObject.Find("TVScreen");
        }

        ghostFinger.GetComponent<NSTOL_GhostFingerClick>().CollideWithPlayer += (GameObject collideWithName) => collidingWith = collideWithName;
        ghostFinger.GetComponent<NSTOL_GhostFingerClick>().EndCollideWithPlayer += () => collidingWith = null; 

        remoteGrabInteractable.onSelectEnter.RemoveAllListeners();
        remoteGrabInteractable.onSelectEnter.AddListener(SetHoldingRemote);
        remoteGrabInteractable.onSelectExit.RemoveAllListeners();
        remoteGrabInteractable.onSelectExit.AddListener(SetNotHoldingRemote);

    }

    private void SetHoldingRemote(XRBaseInteractor arg0)
    {
        //DebugHelpers.Log("SetHoldingRemote");
        controller = arg0.GetComponent<XRController>();
        XRBaseInteractable remote = arg0.selectTarget;
        // TODO: i might need this stuff still. i think i had some inteligent stuff for picking up the balls and paddles that was getting run redundantly while i was playing with this ghost finger remote thing. Or maybe i just copy-pasted this stuff in here from there and never used it. Test and clean up.
        //interactor = controller.GetComponent<XRRayInteractor>();
        //interactor.onHoverEnter.AddListener(handIsHoldingBall);
        //interactor.onSelectEnter.AddListener(PickUpPaddle);
        //interactor.onSelectExit.AddListener(DropPaddle);
        beingHeld = true;

        if (BeginLocomotion())
        {
            controller.enableInputActions = false;
            device = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
            if (device.TryGetHapticCapabilities(out capabilities))
            {
                if (!capabilities.supportsImpulse)
                {
                    Debug.LogError("capabilities.supportsImpluse is false. Is this not a haptic-capable XR controller?");
                }
            }
            EndLocomotion();
            controller.enableInputActions = true;
        }
    }

    private void SetNotHoldingRemote(XRBaseInteractor arg0)
    {
        //DebugHelpers.Log("SetNotHoldingRemote");
        beingHeld = false;
    }

    private void SendHapticPulse()
    {
        uint channel = 0;
        float duration = 0.05f;
        device.SendHapticImpulse(channel, hapticFingerHoverIntensity, duration);
    }

    void Update()
    {
        if (beingHeld)
        {
            // Joystick 
            if (controller.inputDevice.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 thumbstick))
            {
                if (thumbstick != null)
                {
                    //Debug.Log("thumbstick: " + thumbstick.ToString());

                    // map scalar to the width and height of the remote
                    thumbPosition = Scale(thumbstick.x, thumbstick.y, thingManager.transform.localScale.x, thingManager.transform.localScale.z);
                    ghostFinger.transform.localPosition = thumbPosition;

                }
            }

            // trigger activated
            if (controller.inputDevice.TryGetFeatureValue(CommonUsages.trigger, out float trigger))
            {
                // collidingWith is handled by the event system. It's instantiated in this file in the Start() method.
                if (collidingWith != null)
                {

                    var syncVidComponent = tv.GetComponent<NSTOL_SynchronousVideo>();
                    switch (collidingWith.name)
                    {
                        case "Play":
                            //DebugHelpers.Log("Play button mouseover");
                            SendHapticPulse();
                            if (trigger > 0.02) {
                                NSTOL_DebugHelpers.Log("Play button pushed");
                                syncVidComponent.Play();
                            }
                            break;
                        case "Pause":
                            //DebugHelpers.Log("Pause button pushed");
                            SendHapticPulse();
                            if (trigger > 0.02)
                                syncVidComponent.Pause();
                            break;
                        case "Stop":
                            //DebugHelpers.Log("Stop button pushed");
                            SendHapticPulse();
                            if (trigger > 0.02)
                                syncVidComponent.Stop();
                            break;
                        case "Video1":
                            //DebugHelpers.Log("Video1 button pushed");
                            SendHapticPulse();
                            if (trigger > 0.02)
                                tv.GetComponent<TVRemoteControl>().videoURL = "https://movietrailers.apple.com/movies/independent/blood-and-money/blood-and-money-trailer-1_i320.m4v";
                            break;
                        case "Video2":
                            //DebugHelpers.Log("Video2 button pushed");
                            SendHapticPulse();
                            if (trigger > 0.02)
                                tv.GetComponent<TVRemoteControl>().videoURL = "https://movietrailers.apple.com/movies/lionsgate/the-quarry/the-quarry-trailer-1_i320.m4v";
                            break;
                        default:
                            //DebugHelpers.Log("DEFAULT ON OnCollisionStay" + collidingWith.name);
                            break;
                    }
                }
            }
        }
    }

    public Vector3 Scale(float width, float height, float scaleWidth, float scaleHeight)
    {

        var widthMultiplier = 1f / scaleWidth;

        var heightMultiplier = 1f / scaleHeight;
        Vector3 returnVector = new Vector3((width / widthMultiplier) * 2, 0, (height / heightMultiplier)*2);
        //Debug.Log("Scale called with: width=" + width + "; height=" + height + "; scaleWidth=" + scaleWidth + "; scaleHeight=" + scaleHeight);
        //Debug.Log("Returning Scale values: " + returnVector.ToString());
        return returnVector;
    }


}
