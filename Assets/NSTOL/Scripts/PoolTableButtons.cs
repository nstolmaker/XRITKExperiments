using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEngine.XR.Interaction.Toolkit
{
    public class PoolTableButtons : XRBaseInteractable
    {

        public string buttonFunction = "Reset";
        private Vector3 originalBlueButtonTransform;
        private Vector3 originalRedButtonTransform;
        public bool buttonDown;


        /*
        void Start()
        {


            this.originalBlueButtonTransform = GameObject.Find("BlueButton").transform.localPosition;
            this.originalRedButtonTransform = GameObject.Find("RedButton").transform.localPosition;
        }
        
        // Update is called once per frame
        void Update()
        {
            //Theres no start, so we just do this here?
            if (!simpleInteractable)
            {
                simpleInteractable = GetComponent<XRSimpleInteractable>();
                simpleInteractable.onSelectEnter.AddListener(OnButtonSelect);
                simpleInteractable.onActivate.AddListener(OnButtonSelect);
            }

        }
        */



        protected override void OnSelectEnter(XRBaseInteractor controller)
        {
            //Debug.Log("collided with " + other.name);
            DebugHelpers.Log("Button Push Triggered on " + controller.name);
            //all hands have an VORGrabber object on them. So this way we know it was their hand and not their face or player collider.
            ButtonGoesIn();
            switch (buttonFunction)
            {
                case "Reset":
                    gameObject.GetComponentInParent<HockeyController>().ResetPuck();
                    break;
                case "ResetAll":
                    gameObject.GetComponentInParent<HockeyController>().ResetScore();
                    break;

            }
            ButtonGoesOut();
        }

        public void ButtonGoesIn()
        {
            if (!this.buttonDown)
            {
                this.buttonDown = true;
                this.transform.Translate(0, -.0014f, 0);
            }
        }

        public void ButtonGoesOut()
        {
            if (this.buttonDown)
            {
                this.buttonDown = false;
                this.transform.Translate(0, .0014f, 0);
            }
        }
    }
}