using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolTableButtons : MonoBehaviour
{

    public string buttonFunction = "Reset";
    private Vector3 originalBlueButtonTransform;
    private Vector3 originalRedButtonTransform;
    public bool buttonDown;
    // Start is called before the first frame update
    void Start()
    {
        this.originalBlueButtonTransform = GameObject.Find("BlueButton").transform.localPosition;
        this.originalRedButtonTransform = GameObject.Find("RedButton").transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTrggerStay(Collider other)
    {
        Debug.Log("collided with " + other.name);
        //all hands have an VORGrabber object on them. So this way we know it was their hand and not their face or player collider.
        if (other.GetComponentInParent<OVRGrabber>())
        {
            ButtonGoesIn();
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.GetComponentInParent<OVRGrabber>())
        {
            ButtonGoesOut();

            switch (buttonFunction)
            {
                case "Reset":
                    gameObject.GetComponentInParent<HockeyController>().ResetPuck();
                    break;
                case "ResetAll":
                    gameObject.GetComponentInParent<HockeyController>().ResetScore();
                    break;

            }
        }
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
