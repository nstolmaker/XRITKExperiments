using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections.Generic;
using UnityEngine.UI;

namespace UnityEngine.XR.Interaction.Toolkit
{
    public class NSTOL_DebugHelpers : LocomotionProvider
    {


        public static void Log(string logStatement)
        {
            var prevText = GameObject.FindGameObjectWithTag("DebugPanel").GetComponent<Text>().text;
            if (prevText.Length > 800)
            {
                prevText = prevText.Substring(0, 800);
            }
            GameObject.FindGameObjectWithTag("DebugPanel").GetComponent<Text>().text = logStatement + "\n" + prevText;
        }

        /* 
         * There's a bunch of locomotion stuff here that just makes it so you can move the avatar around with ASDW/Space controls, for testing purposes.
         * */
        void Update()
        {

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Debug.Log("Escape pressed");
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log("space pressed");
                float m_JumpAmount = 0.5f;

                if (BeginLocomotion())
                {
                    // the below code works for smooth movement, but doesn't seem to update the avatar position properly.
                    var xrRig = system.xrRig;
                    var camera = xrRig.transform.Find("Camera Offset").transform.Find("Main Camera");
                    Quaternion headRotationFlat = camera.transform.rotation;
                    var move = (headRotationFlat * (new Vector3(0f, m_JumpAmount, 0f)));
                    xrRig.transform.Translate(move, Space.World);

                    m_JumpAmount = 0.5f;
                    EndLocomotion();
                }
                //GameObject.Find("RedButton").GetComponentInChildren<PoolTableButtons>().ButtonGoesIn();
            }
            if (Input.GetKeyDown(KeyCode.W))
            {
                Debug.Log("w pressed");
                Vector2 m_CurrentMovementAmount = new Vector2(0f, 0.5f);

                if (BeginLocomotion())
                {
                    var xrRig = system.xrRig;
                    var camera = xrRig.transform.Find("Camera Offset").transform.Find("Main Camera");
                    Quaternion headRotationFlat = Quaternion.Euler(0, camera.transform.eulerAngles.y, 0);
                    var move = (headRotationFlat * (new Vector3(m_CurrentMovementAmount.x, 0f, m_CurrentMovementAmount.y)));
                    xrRig.transform.Translate(move, Space.World);

                    m_CurrentMovementAmount = new Vector2(0f, 0.5f);
                    EndLocomotion();
                }
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                if (BeginLocomotion())
                {
                    Vector2 m_CurrentMovementAmount = new Vector2(0f, -0.5f);

                    var xrRig = system.xrRig;
                    var camera = xrRig.transform.Find("Camera Offset").transform.Find("Main Camera");
                    Quaternion headRotationFlat = Quaternion.Euler(0, camera.transform.eulerAngles.y, 0);
                    var move = (headRotationFlat * (new Vector3(m_CurrentMovementAmount.x, 0f, m_CurrentMovementAmount.y)));
                    xrRig.transform.Translate(move, Space.World);

                    m_CurrentMovementAmount = new Vector2(0f, -0.5f);
                    EndLocomotion();
                }
            }
            if (Input.GetKeyDown(KeyCode.A))
            {
                if (BeginLocomotion())
                {
                    var xrRig = system.xrRig;
                    xrRig.transform.Rotate(Vector3.up, -45f);

                    EndLocomotion();
                }
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                if (BeginLocomotion())
                {
                    var xrRig = system.xrRig;
                    xrRig.transform.Rotate(Vector3.up, 45f);

                    EndLocomotion();
                }
            }

        }

    }

}