using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections.Generic;


namespace UnityEngine.XR.Interaction.Toolkit
{
    public class DebugHelpers : LocomotionProvider
    {
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Debug.Log("Primary thumbstick pressed");
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                Debug.Log("tab pressed");
                Vector2 m_CurrentMovementAmount = new Vector2(0.5f, 0.5f);

                if (BeginLocomotion())
                {
                    // the below code works for smooth movement, but doesn't seem to update the avatar position properly.
                    var xrRig = system.xrRig;
                    var camera = xrRig.transform.Find("Camera Offset").transform.Find("Main Camera");
                    Quaternion headRotationFlat = Quaternion.Euler(0, camera.transform.eulerAngles.y, 0);
                    var move = (headRotationFlat * (new Vector3(m_CurrentMovementAmount.x, 0f, m_CurrentMovementAmount.y)));
                    xrRig.transform.Translate(move, Space.World);

                    m_CurrentMovementAmount = new Vector2(0.5f, 0.5f);
                    EndLocomotion();
                }
                //GameObject.Find("RedButton").GetComponentInChildren<PoolTableButtons>().ButtonGoesIn();
            }

        }
    }

}