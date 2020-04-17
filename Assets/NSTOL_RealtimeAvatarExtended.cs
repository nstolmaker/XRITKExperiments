using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;

public class NSTOL_RealtimeAvatarExtended : MonoBehaviour
{

    private RealtimeView realtimeView;  // keep a reference to the RealtimeView component.
    private RealtimeAvatar realtimeAvatar;  // keep a reference to the RealtimeView component.
    public List<GameObject> appendages = new List<GameObject>();
    public bool hideHands;
    void Start()
    {
        this.realtimeView = GetComponent<RealtimeView>();
        this.realtimeAvatar = GetComponent<RealtimeAvatar>();
        GameObject leftHand = this.gameObject.transform.Find("Left Hand").gameObject;
        GameObject rightHand = this.gameObject.transform.Find("Right Hand").gameObject;
        this.appendages.Add(leftHand);
        this.appendages.Add(rightHand);

        // for now, hand movements are only supported for the local player. So let's hide those rigid hand objects when its our own avatar
        if (this.realtimeView.isOwnedLocally)
        {
            this.hideHands = true;
            this.realtimeAvatar.model.activeStateChanged += AvatarActiveStateDidChange;
        }
    }

    private void AvatarActiveStateDidChange(RealtimeAvatarModel model, uint activeState) 
    {
        if (this.hideHands)
        {
            //iterate through known appendages and feed them invisibility serum
            foreach (GameObject appendage in this.appendages)
            {
               appendage.SetActive(false);
            }
        }

    }
}
