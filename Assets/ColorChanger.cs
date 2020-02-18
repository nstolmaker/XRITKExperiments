using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ColorChanger : MonoBehaviour
{
    public Material defaultMaterial  = null;
    public Material activeMaterial = null;

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

    private void SetFirst(XRBaseInteractor interactor)
    {
        meshRenderer.material = defaultMaterial;
    }

    private void SetSecond(XRBaseInteractor intereactor)
    {
        meshRenderer.material = activeMaterial;
    }
}
