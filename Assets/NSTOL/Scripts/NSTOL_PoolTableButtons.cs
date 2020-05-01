using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEngine.XR.Interaction.Toolkit
{
    public class NSTOL_PoolTableButtons : XRBaseInteractable
    {

        public string buttonFunction = "Reset";
        public bool buttonDown;


        protected override void OnSelectEnter(XRBaseInteractor controller)
        {
            Debug.Log("Button Push Triggered on " + controller.name + "with function: " + buttonFunction);
            //NSTOL_DebugHelpers.Log("Button Push Triggered on " + controller.name + "with function: "+ buttonFunction);
            ButtonGoesIn();
            switch (buttonFunction)
            {
                case "Reset":
                    gameObject.GetComponentInParent<NSTOL_HockeyController>().ResetPuck();
                    break;
                case "ResetAll":
                    gameObject.GetComponentInParent<NSTOL_HockeyController>().ResetScore();
                    break;

            }
        }

        protected override void OnSelectExit(XRBaseInteractor controller)
        {
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