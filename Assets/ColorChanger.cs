using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ColorChanger : MonoBehaviour
{
    public Color defaultColor;
    public Color activeColor;

    private Animator anim;


    private MeshRenderer meshRenderer = null;
    private XRGrabInteractable grabInteractable = null;


    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        grabInteractable = GetComponent<XRGrabInteractable>();

        grabInteractable.onSelectExit.AddListener(SetFirst);
        grabInteractable.onSelectEnter.AddListener(SetSecond);
    }

    private void OnDestroy()
    {
        grabInteractable.onSelectExit.RemoveListener(SetFirst);
        grabInteractable.onSelectEnter.RemoveListener(SetSecond);
    }

    /* 
     * State for default (not holding)
     */
    private void SetFirst(XRBaseInteractor interactor)
    {
        // DebugHelpers.Log("Not Holding");
        meshRenderer.material.color = defaultColor;
    }

    /* 
     * State 2 (for holding )
     */
    private void SetSecond(XRBaseInteractor intereactor)
    {
       // DebugHelpers.Log("Holding");
       
        meshRenderer.material.color = activeColor;
    }
}
