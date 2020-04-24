using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;

#if LIH_PRESENT
using UnityEngine.Experimental.XR.Interaction;
#endif

namespace UnityEngine.XR.Interaction.Toolkit
{

    public class NSTOL_AirHockey_Controller : MonoBehaviour
    {

        private RealtimeTransform rtPaddle;
        private XRController controller;
        private XRRayInteractor interactor = null;

        void Start()
        {
            //DebugLog = new NSTOLLog(debugTextObject);
            if (!controller)
            {
                controller = GetComponent<XRController>();
            }

            interactor = controller.GetComponent<XRRayInteractor>();
            //interactor.onHoverEnter.AddListener(handIsHoldingBall);
            interactor.onSelectEnter.AddListener(PickUpPaddle);
            interactor.onSelectExit.AddListener(DropPaddle);
        }

        private void PickUpPaddle(XRBaseInteractable paddle)
        {
            // get and save a reference to the RealtimeTransform on the target object
            rtPaddle = paddle.GetComponent<RealtimeTransform>();
            if (rtPaddle != null)
            {
                rtPaddle.RequestOwnership();
                Debug.Log("PickUpPaddle Requesting ownership for object:" + paddle.name);
            } else
            {
                Debug.Log("PickUpPaddle Failed to find a RT transform. Not owning obj:" + paddle.name);
            }
        }

        private void DropPaddle(XRBaseInteractable paddle)
        {
            /*
            // get and save a reference to the RealtimeTransform on the target object
            rtPaddle = paddle.GetComponent<RealtimeTransform>();

            if (rtPaddle != null)
            {
                //rtPaddle.ClearOwnership();
                DebugHelpers.Log("CLEARING ownership for object:" + paddle.name);
            }
            else
            {
                DebugHelpers.Log("DropPaddle Failed to find a RT transform. Not owning obj:" + paddle.name);
            }
            */

        }

    }

}