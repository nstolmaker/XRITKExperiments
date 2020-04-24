using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class NSTOL_GhostFingerSelect : MonoBehaviour
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
    private bool beingHeld = false;
    [SerializeField]
    private Vector3 thumbPosition;
    [SerializeField]
    private GameObject ghostFinger;
    [SerializeField]
    private bool clicking = false;
    [SerializeField]
    private GameObject collidingWith;


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
            
            //onCollisionEnter = (collider collider) => { }
            //ghostFinger.transform.parent = thingManager.transform;
        }
        ghostFinger.GetComponent<NSTOL_GhostFingerClick>().CollideWithPlayer += (GameObject collideWithName) => collidingWith = collideWithName;
        ghostFinger.GetComponent<NSTOL_GhostFingerClick>().EndCollideWithPlayer += () => collidingWith = null; 

        remoteGrabInteractable.onSelectEnter.RemoveAllListeners();
        remoteGrabInteractable.onSelectEnter.AddListener(SetHoldingRemote);
        remoteGrabInteractable.onSelectExit.RemoveAllListeners();
        remoteGrabInteractable.onSelectExit.AddListener(SetNotHoldingRemote);

    }

    void Awake()
    {

    }

    private void SetHoldingRemote(XRBaseInteractor arg0)
    {
        DebugHelpers.Log("SetHoldingRemote");
        //interactor = controller.GetComponent<XRRayInteractor>();
        controller = arg0.GetComponent<XRController>();
        XRBaseInteractable remote = arg0.selectTarget;
        //interactor.onHoverEnter.AddListener(handIsHoldingBall);
        //interactor.onSelectEnter.AddListener(PickUpPaddle);
        //interactor.onSelectExit.AddListener(DropPaddle);
        beingHeld = true;
    }

    private void SetNotHoldingRemote(XRBaseInteractor arg0)
    {
        DebugHelpers.Log("SetNotHoldingRemote");
        beingHeld = false;
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
                    Debug.Log("thumbstick: " + thumbstick.ToString());

                    // map scalar to the width and height of the remote
                    thumbPosition = Scale(thumbstick.x, thumbstick.y, thingManager.transform.localScale.x, thingManager.transform.localScale.z);
                    ghostFinger.transform.localPosition = thumbPosition;

                }
            }

            // Joystick Click
            if (controller.inputDevice.TryGetFeatureValue(CommonUsages.trigger, out float trigger))
            {
                if (trigger > 0.02)
                {
                    if (collidingWith != null)
                    {
                        //Debug.Log("thumbstickClick: " + trigger.ToString());
                        // see if there's a collision and trigger an event if there is. (OnCollisionStay should be triggered, just set clicking=true and it'll run.)
                        DebugHelpers.Log("Clicking on: " + collidingWith.name);
                        Debug.LogError("Clicking on: " + collidingWith.name);
                        var tv = GameObject.Find("TVScreen");
                        var syncVidComponent = tv.GetComponent<NSTOL_SynchronousVideoTest>();
                        switch (collidingWith.name)
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
                                DebugHelpers.Log("DEFAULT ON OnCollisionStay" + collidingWith.name);
                                Debug.LogError("DEFAULT ON OnCollisionStay" + collidingWith.name);
                                break;
                        }
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

    /*
    void OnCollisionEnter(Collision collision)
    {
        collidingWith = collision.gameObject;
        //if (collision.gameObject.name == "PrototypeRemoteButton1")
        //{
        if (clicking)
        {

            clicking = false;
        }
        //}
    }

    void OnCollisionExit(Collision collision)
    {
        //collidingWith = null;
 
    }
    */
}
