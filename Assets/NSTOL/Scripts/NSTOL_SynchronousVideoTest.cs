using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
public class NSTOL_SynchronousVideoTest : MonoBehaviour
{

    // TODO: Move all this to the TVRemoteControl script.
    [SerializeField]
    public int _playState = 0;
    [SerializeField]
    public int _previousPlayState;

    private NSTOL_SynchronousVideo _synchronousVideo;

    void Start()
    {
        // Get a reference to the color sync component.
        _synchronousVideo = GetComponent<NSTOL_SynchronousVideo>();
    }


    void Update()
    {
        // If the playstate has changed, call SetPlayState on the _synchronousVideo component.
        if (_playState != _previousPlayState)
        {
            NSTOL_DebugHelpers.Log("_playState changed. saving that, and calling SetPlayState");
            Debug.LogError("Playstate Chagned");
            _synchronousVideo.SetPlayState(_playState);
            _previousPlayState = _playState;
        }
    }

}
