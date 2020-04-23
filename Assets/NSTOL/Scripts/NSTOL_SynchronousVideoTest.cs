using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NSTOL_SynchronousVideoTest : MonoBehaviour
{
    [SerializeField]
    private string _playbackURL;
    [SerializeField]
    private string _previousPlaybackURL;

    private NSTOL_SynchronousVideo _synchronousVideo;

    void Start()
    {
        // Get a reference to the color sync component.
        _synchronousVideo = GetComponent<NSTOL_SynchronousVideo>();
    }

    void Update()
    {
        // If the color has changed (via the inspector), call SetColor on the color sync component.
        if (_playbackURL != _previousPlaybackURL)
        {
            _synchronousVideo.SetVideoURL(_playbackURL);
        }
    }
}
