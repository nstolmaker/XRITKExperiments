using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.XR.Interaction.Toolkit;
using Normal.Realtime;

public class NSTOL_SynchronousVideo : RealtimeComponent
{
    [SerializeField]
    private NSTOL_SynchronousVideoModel _model;
    [SerializeField]
    private GameObject tv;
    void Start()
    {
        if (!tv)
        {
            tv = GameObject.Find("TVScreen");
        }
    }

    private NSTOL_SynchronousVideoModel model
    {
        set
        {
            if (_model != null)
            {
                _model.frameNumberDidChange -= FrameNumberDidChange;
                _model.playbackURLDidChange -= VideoURLDidChange;
            }

            _model = value;
            if (_model != null)
            {
                _model.frameNumberDidChange += FrameNumberDidChange;
                _model.playbackURLDidChange += VideoURLDidChange;
            }
        }
    }


    public void FrameNumberDidChange(NSTOL_SynchronousVideoModel model, int frameNumber)
    {
        SmoothToNewFrame(frameNumber);
    }

    private void SmoothToNewFrame(int frameNumber)
    {
        DebugHelpers.Log("Smoothing to new frame " + frameNumber);

    }

    public void SetFrame(int frameNumber)
    {
        _model.frameNumber = frameNumber;
    }


    public void VideoURLDidChange(NSTOL_SynchronousVideoModel model, string videoURL)
    {
        // update local video url to new url
        DebugHelpers.Log("Loading new video URL: " + videoURL);
        Debug.LogError("Loading new video URL: " + videoURL);
        tv.GetComponent<TVRemoteControl>().videoURL = videoURL; // this updates the TVRemoteControl component, which is what we read from mostly. That might be kinda confusing.
        tv.GetComponent<VideoPlayer>().url = videoURL;  // this updates the actual video player url. It might make more sense to trigger CheckDimensions or Setup() with an argument and let it handle the change.
        //tv.GetComponent<TVRemoteControl>().CheckDimensions(videoURL);
    }

    public void SetVideoURL(string videoURL)
    {
        Debug.LogError("SetVideoURL(" + videoURL+")");
        _model.playbackURL = videoURL;
    }
}
