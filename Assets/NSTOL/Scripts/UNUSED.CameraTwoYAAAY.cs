using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Place this script on a GameObject to switch between the main Camera and your own second Camera on the press of the "L" key
//Place a second Camera in your Scene and assign it as the "Camera Two" in the Inspector.

public class CameraTwoYAAAY : MonoBehaviour
{
    //This is Main Camera in the Scene
    public OVRCameraRig m_OBR_Bool;
    //This is the second Camera and is assigned in inspector
    public Camera m_CameraTwo;

    void Start()
    {
        //This gets the Main Camera from the Scene
        //This enables Main Camera
        m_OBR_Bool.disableEyeAnchorCameras = false;
        //Use this to disable secondary Camera
        m_CameraTwo.enabled = false;
    }

    void Update()
    {
        //Press the L Button to switch cameras
        if (Input.GetKeyDown(KeyCode.L))
        {
            //Check that the Main Camera is enabled in the Scene, then switch to the other Camera on a key press
            if (m_CameraTwo.enabled)
            {
                //Enable the second Camera
                m_CameraTwo.enabled = false;

                //The Main first Camera is disabled
                m_OBR_Bool.disableEyeAnchorCameras = false;
            }
            //Otherwise, if the Main Camera is not enabled, switch back to the Main Camera on a key press
            else if (!m_CameraTwo.enabled)
            {
                //Disable the second camera
                m_CameraTwo.enabled = true;

                //Enable the Main Camera
                m_OBR_Bool.disableEyeAnchorCameras = true;
            }
        }
    }
}