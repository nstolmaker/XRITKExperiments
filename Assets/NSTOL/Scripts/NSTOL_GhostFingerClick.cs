using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NSTOL_GhostFingerClick : MonoBehaviour
{
    public delegate void EventHandler(GameObject collider);
    public delegate void EndEventHandler();
    public event EventHandler CollideWithPlayer;
    public event EndEventHandler EndCollideWithPlayer;

    // Checking a reference to a collider is better than using strings.
    [SerializeField] GameObject collidingWith;


    void OnTriggerEnter(Collider collider)
    {
        collidingWith = collider.gameObject;
        CollideWithPlayer(collidingWith);
    }

    private void OnTriggerExit(Collider collider)
    {
        collidingWith = collider.gameObject;
        EndCollideWithPlayer();
    }
}
