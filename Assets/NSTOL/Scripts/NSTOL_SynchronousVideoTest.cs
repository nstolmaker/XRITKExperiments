using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
public class NSTOL_SynchronousVideoTest : MonoBehaviour
{
    [SerializeField]
    private string _playbackURL;
    [SerializeField]
    private string _previousPlaybackURL;
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
        // If the playback URL has changed (via the inspector), call SetVideoURL on the _synchronousVideo component.
        if (_playbackURL != _previousPlaybackURL)
        {
            DebugHelpers.Log("playback URL changed. saving that, and calling setvideoURL");
            Debug.LogError("Playback URL Chagned");
            _synchronousVideo.SetVideoURL(_playbackURL);
            _previousPlaybackURL = _playbackURL;
        }
        // If the playstate has changed, call SetPlayState on the _synchronousVideo component.
        if (_playState != _previousPlayState)
        {
            DebugHelpers.Log("_playState changed. saving that, and calling SetPlayState");
            Debug.LogError("Playstate Chagned");
            _synchronousVideo.SetPlayState(_playState);
            _previousPlayState = _playState;
        }
    }

}
