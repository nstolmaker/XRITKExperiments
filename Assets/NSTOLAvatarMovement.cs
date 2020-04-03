using System;
using System.Collections.Generic;
using UnityEngine;


namespace UnityEngine.XR.Interaction.Toolkit {
    /// <summary>
    /// The NSTOLAvatarMovement is a locomotion provider that allows the user to move their rig using a specified 2d axis input.
    /// the provider can take input from two different devices (eg: L & R hands). It is a ripoff of the SnapTurnProvider.cs XRToolkit provider.
    /// </summary>
    public class NSTOLAvatarMovement : LocomotionProvider
    {

        [Tooltip("The axis on the controller you're gonna use for moving around. Look at the ProjectSettings panel's Input for this")]
        public Input inputDevice;
        public Transform avatar;
        public float rotationSpeed = 100.0f;
        [Tooltip("Valid options are left and right. Case sensitive.")]
        public string hand;
        private Transform forwardDirection;
        float yRotation;

        /// <summary>
        /// This is the list of possible valid "InputAxes" that we allow users to read from.
        /// </summary>
        public enum InputAxes
        {
            Primary2DAxis = 0,
            Secondary2DAxis = 1,
        };

        // Mapping of the above InputAxes to actual common usage values
        static readonly InputFeatureUsage<Vector2>[] m_Vec2UsageList = new InputFeatureUsage<Vector2>[] {
            CommonUsages.primary2DAxis,
            CommonUsages.secondary2DAxis,
        };

        [SerializeField]
        [Tooltip("The 2D Input Axis on the primary devices that will be used to trigger smooth movement.")]
        InputAxes m_MoveUsage = InputAxes.Primary2DAxis;
        /// <summary>
        /// The 2D Input Axis on the primary device that will be used to trigger a snap turn.
        /// </summary>
        public InputAxes turnUsage { get { return m_MoveUsage; } set { m_MoveUsage = value; } }

        [SerializeField]
        [Tooltip("A list of controllers that allow Snap Turn.  If an XRController is not enabled, or does not have input actions enabled.  Snap Turn will not work.")]
        List<XRController> m_Controllers = new List<XRController>();
        /// <summary>
        /// The XRControllers that allow SnapTurn.  An XRController must be enabled in order to Snap Turn.
        /// </summary>
        public List<XRController> controllers { get { return m_Controllers; } set { m_Controllers = value; } }

        [SerializeField]
        [Tooltip("The speed at which movement will happen.")]
        float m_Speed = 10.0f;
        /// <summary>
        /// The speed at which movement will happen.
        /// </summary>
        public float moveSpeed { get { return m_Speed; } set { m_Speed = value; } }

        [SerializeField]
        [Tooltip("The amount of time that the system will wait before starting another snap turn.")]
        float m_DebounceTime = 0.25f;
        /// <summary>
        /// The amount of time that the system will wait before starting another snap turn.
        /// </summary>
        public float debounceTime { get { return m_DebounceTime; } set { m_DebounceTime = value; } }

        [SerializeField]
        [Tooltip("The deadzone that the controller movement will have to be above to trigger a snap turn.")]
        float m_DeadZone = 0.5f;
        /// <summary>
        /// The deadzone that the controller movement will have to be above to trigger a snap turn.
        /// </summary>
        public float deadZone { get { return m_DeadZone; } set { m_DeadZone = value; } }

        void EnsureControllerDataListSize()
        {
            if (m_Controllers.Count != m_ControllersWereActive.Count)
            {
                while (m_ControllersWereActive.Count < m_Controllers.Count)
                {
                    m_ControllersWereActive.Add(false);
                }

                while (m_ControllersWereActive.Count < m_Controllers.Count)
                {
                    m_ControllersWereActive.RemoveAt(m_ControllersWereActive.Count - 1);
                }
            }
        }

        // state data
        Vector3 m_CurrentMovementAmount = new Vector3();
        float m_TimeStarted = 0.0f;

        List<bool> m_ControllersWereActive = new List<bool>();

        private void Update()
        {
            // wait for a certain amount of time before allowing another turn.
            if (m_TimeStarted > 0.0f && (m_TimeStarted + m_DebounceTime < Time.time))
            {
                m_TimeStarted = 0.0f;
                return;
            }

            if (m_Controllers.Count > 0)
            {
                EnsureControllerDataListSize();

                InputFeatureUsage<Vector2> feature = m_Vec2UsageList[(int)m_MoveUsage];
                for (int i = 0; i < m_Controllers.Count; i++)
                {
                    XRController controller = m_Controllers[i];
                    if (controller != null)
                    {
                        if (controller.enableInputActions && m_ControllersWereActive[i])
                        {
                            InputDevice device = controller.inputDevice;

                            Vector2 currentState;
                            if (device.TryGetFeatureValue(feature, out currentState))
                            {
                                //if (currentState.y > deadZone)
                                //{
                                    StartMove(currentState);
                                //}
                                //else if (currentState.y < -deadZone)
                                //{
                                //    StartMove(-currentState.y);
                                //}
                            }
                        }
                        else //This adds a 1 frame delay when enabling input actions, so that the frame it's enabled doesn't trigger a snap turn.
                        {
                            m_ControllersWereActive[i] = controller.enableInputActions;
                        }
                    }
                }
            }

            if (Math.Abs(m_CurrentMovementAmount.x) > 0.0f || Math.Abs(m_CurrentMovementAmount.y) > 0.0f)
            {

                if (BeginLocomotion())
                {
                    var xrRig = system.xrRig;
                    print("xrRig:" + xrRig.rigInCameraSpacePos);
                    var camera = xrRig.transform.Find("Camera Offset").transform.Find("Main Camera");
                    Quaternion headRotationFlat = Quaternion.Euler(0, camera.transform.eulerAngles.y, 0);
                    //ovrCameraRig.transform.Translate((headRotationFlat * Vector3.forward) * speed * Time.deltaTime, Space.World);
                    //Vector3 heightAdjustment = xrRig.rig.transform.up * xrRig.cameraInRigSpaceHeight;
                    var move = (headRotationFlat * new Vector3(m_CurrentMovementAmount.x, 0f, m_CurrentMovementAmount.y)) * m_Speed * Time.deltaTime;
                    Vector3 cameraDestination = xrRig.rigInCameraSpacePos + move; // heightAdjustment + move; //  + move;   // rigInCameraSpacePos doesn't make sense. Should be just xrRig.transform.position i think. make sure all numbers are using same relative or absolute space point

                    //xrRig.MoveCameraToWorldLocation(cameraDestination);


                    //xrRig.MoveCameraToWorldLocation((headRotationFlat * Vector3.forward) * m_Speed * Time.deltaTime);
                    xrRig.transform.Translate((headRotationFlat * new Vector3(m_CurrentMovementAmount.x, 0f, m_CurrentMovementAmount.y))  * m_Speed * Time.deltaTime, Space.World);

                    m_CurrentMovementAmount = new Vector2();

                    EndLocomotion();
                }
            }

                /*
                if (BeginLocomotion())
                {
                    var xrRig = system.xrRig;

                    float moveHorizontal = m_CurrentMovementAmount.x;
                    float moveVertical = m_CurrentMovementAmount.y;
                    Vector3 position = xrRig.transform.position;
                    position.x += moveHorizontal * m_Speed; //* Time.deltaTime;
                    position.z += moveVertical * m_Speed; //* Time.deltaTime;
                    //transform.position = position;

                    Vector3 cameraDestination = position; // + heightAdjustment;// + move; //  + move;

                    xrRig.MatchRigUp(transform.up);
                    xrRig.MoveCameraToWorldLocation(cameraDestination);

                    m_CurrentMovementAmount = new Vector2();
                    EndLocomotion();
                }
            }
                /*
                var camera = xrRig.transform.Find("Camera Offset").transform.Find("Main Camera");
                //var oldPos = camera.position; //new Vector3(camera.position.x, camera.position.y, camera.position.z);
                Vector2 direction = m_CurrentMovementAmount.normalized;
                Vector2 velocity = direction * m_Speed;
                Vector2 moveAmount = velocity * Time.deltaTime; // new Vector3(moveAmount.x, 0f, moveAmount.y);
                //Vector3 move = new Vector3(moveAmount.x, 0f, moveAmount.y);
                Vector3 move = new Vector3(0.1f, 0f, 0.1f);
                if (xrRig != null)
                {
                    // xrRig.transform.Translate(new Vector3(camera.position.x, 0f, camera.position.z + m_CurrentMovementAmount));
                    //camera.Translate(oldPos.x, oldPos.y, oldPos.z); // Translate(new Vector3(camera.position.x, 0f, camera.position.z + m_CurrentMovementAmount));
                    //camera.transform.Translate(move); // Translate(new Vector3(camera.position.x, 0f, camera.position.z + m_CurrentMovementAmount));
                    //xrRig.RotateAroundCameraUsingRigUp(m_CurrentMovementAmount);
                }

                // switch (m_CurrentRequest.matchOrientation)
                // {
                //     case MatchOrientation.None:
                         xrRig.MatchRigUp(camera.up);
                //         break;
                //     case MatchOrientation.Camera:
                //I changed this and it'd work if it needed to i think.         
                // xrRig.MatchRigUpCameraForward(xrRig.transform.up, xrRig.transform.forward);
                //         break;
                //case MatchOrientation.Rig:
                //    xrRig.MatchRigUpRigForward(m_CurrentRequest.destinationUpVector, m_CurrentRequest.destinationForwardVector);
                //    break;
                //}

                Vector3 heightAdjustment = xrRig.rig.transform.up * xrRig.cameraInRigSpaceHeight;

                Vector3 cameraDestination = xrRig.rig.transform.position + heightAdjustment;// + move; //  + move;

                        xrRig.MoveCameraToWorldLocation(cameraDestination);
                    
                    m_CurrentMovementAmount = new Vector2();
                    EndLocomotion();
            }
            */
            }


        private void StartMove(Vector2 currentState)
        {
            //if (m_TimeStarted != 0.0f)
            //    return;

            if (!CanBeginLocomotion())
                return;

            m_TimeStarted = Time.time;
            m_CurrentMovementAmount = currentState;
        }        
                /*
        void Start()
        {
            forwardDirection = avatar.transform.Find("Main Camera").transform;
        }

        // Update is called once per frame
        void Update()
        {
            Vector2 ovrinput = new Vector2(); // = new Vector2(0,0);
                                              // assume it's an oculus controller
                                              //float translation = Input.GetAxis("Oculus_CrossPlatform_PrimaryThumbstickHorizontal") * speed;
            switch (hand)
            {
                case "right":
                    //   ovrinput = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, OVRInput.Controller.RTouch);
                    break;
                case "left":
                    //   ovrinput = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, OVRInput.Controller.LTouch);
                    break;
            }



            // Make it move 10 meters per second instead of 10 meters per frame...
            Vector2 scaledOvrinput = ovrinput * speed * Time.deltaTime;
            float xAxis = scaledOvrinput.x;
            float yAxis = scaledOvrinput.y;

            //rotation *= Time.deltaTime;
            //float rotation = Input.GetAxis("Oculus_CrossPlatform_PrimaryThumbstickVertical") * rotationSpeed;
            //if (ovrinput != new Vector2() && ovrinput != new Vector2())
            //{
            //    Debug.Log("got x and y axis for " + hand + " hand controller: " + new Vector3(0f, xAxis, yAxis).ToString());
            //}

            // Move translation along the object's z-axis
            yRotation = Time.deltaTime * forwardDirection.transform.rotation.eulerAngles.y;
            //avatar.rotation.eulerAngles.y = Quaternion.Euler(0, yRotation, 0);
            //avatar.SetPositionAndRotation(new Vector3(xAxis, 0f, yAxis), forwardDirection.transform.rotation);
            //yAxis += Time.deltaTime * 10;
            //xAxis += Time.deltaTime * 10;
            avatar.transform.Translate(xAxis, 0f, yAxis);

            ovrinput = new Vector2();
        }

        void test()
        {
            if (Input CanBeginLocomotion)
        {

            }
            BeginLocomotion
            EndLocomotion
        }
    }
    */
    }
    }