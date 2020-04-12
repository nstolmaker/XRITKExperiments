using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NSTOLHandAnimate : MonoBehaviour
{
    // stores the current animation for the hand
    [Tooltip("Set to the animator you want to use")]
    public Animator handAnimation;

    // these are the names of the layers we've assigned to the geometry for the hand. We use this too do a search of geometry to find the layer we want to affect.
    public const string ANIM_LAYER_NAME_POINT = "Point Layer";
    public const string ANIM_LAYER_NAME_THUMB = "Thumb Layer";
    public const string ANIM_PARAM_NAME_FLEX = "Flex";
    public const string ANIM_PARAM_NAME_POSE = "Pose";

    // these store the ID of the layer, for easy access later. We could just use the names and translate each time, but this is probably faster and cleaner and it's what oculus is doing, so let's go with this for now.
    private int m_animLayerIndexPoint = -1;
    private int m_animLayerIndexThumb = -1;
    private int m_animParamIndexFlex = -1;
    private int m_animParamIndexPose = -1;

    // Start is called before the first frame update
    void Start()
    {
        // Get animator layer indices by name, for later use switching between hand visuals
        m_animLayerIndexPoint = handAnimation.GetLayerIndex(ANIM_LAYER_NAME_POINT);
        m_animLayerIndexThumb = handAnimation.GetLayerIndex(ANIM_LAYER_NAME_THUMB);
        m_animParamIndexFlex = Animator.StringToHash(ANIM_PARAM_NAME_FLEX);
        m_animParamIndexPose = Animator.StringToHash(ANIM_PARAM_NAME_POSE);
    }

    // Just checking the state of the index and thumb cap touch sensors, but with a little bit of
    // debouncing.
    private void UpdateCapTouchStates()
    {

        //m_isPointing = !OVRInput.Get(OVRInput.NearTouch.PrimaryIndexTrigger, m_controller);
        //m_isGivingThumbsUp = !OVRInput.Get(OVRInput.NearTouch.PrimaryThumbButtons, m_controller);
    }


    private void Update()
    {
        // Step 1. Update our local class's state to match what the user is doing. In other words: grab the current state and stick into local variables for us to use.
        UpdateCapTouchStates();

       // m_pointBlend = InputValueRateChange(m_isPointing, m_pointBlend);
        //m_thumbsUpBlend = InputValueRateChange(m_isGivingThumbsUp, m_thumbsUpBlend);

        //float flex = OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, m_controller);

        //bool collisionEnabled = m_grabber.grabbedObject == null && flex >= THRESH_COLLISION_FLEX;
        //CollisionEnable(collisionEnabled);

        UpdateAnimStates();
    }


    private void UpdateAnimStates()
    {
        /*
        bool grabbing = m_grabber.grabbedObject != null;
        HandPose grabPose = m_defaultGrabPose;
        if (grabbing)
        {
            HandPose customPose = m_grabber.grabbedObject.GetComponent<HandPose>();
            if (customPose != null) grabPose = customPose;
        }
        // Pose
        HandPoseId handPoseId = grabPose.PoseId;
        m_animator.SetInteger(m_animParamIndexPose, (int)handPoseId);

        // Flex
        // blend between open hand and fully closed fist
        float flex = OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, m_controller);
        m_animator.SetFloat(m_animParamIndexFlex, flex);

        // Point
        bool canPoint = !grabbing || grabPose.AllowPointing;
        float point = canPoint ? m_pointBlend : 0.0f;
        m_animator.SetLayerWeight(m_animLayerIndexPoint, point);

        // Thumbs up
        bool canThumbsUp = !grabbing || grabPose.AllowThumbsUp;
        float thumbsUp = canThumbsUp ? m_thumbsUpBlend : 0.0f;
        m_animator.SetLayerWeight(m_animLayerIndexThumb, thumbsUp);

        float pinch = OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, m_controller);
        m_animator.SetFloat("Pinch", pinch);
        */


    }
}
