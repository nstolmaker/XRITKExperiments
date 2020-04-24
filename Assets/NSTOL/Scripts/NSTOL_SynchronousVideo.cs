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
                _model.playStateDidChange -= PlayStateDidChange;
            }

            _model = value;
            if (_model != null)
            {
                _model.frameNumberDidChange += FrameNumberDidChange;
                _model.playbackURLDidChange += VideoURLDidChange;
                _model.playStateDidChange += PlayStateDidChange;
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

    private void PlayStateDidChange(NSTOL_SynchronousVideoModel model, int playState)
    {
        DebugHelpers.Log("Playstate event triggered, updating to: " + playState);
        // !!! TODO: convert to ENUMs.
        switch (playState)
        {
            case 0:
                tv.GetComponent<TVRemoteControl>().Stop();
                break;
            case 1:
                tv.GetComponent<TVRemoteControl>().Play();
                break;
            case 2:
                tv.GetComponent<TVRemoteControl>().Pause();
                break;
            case 3:
                break;
            case 4:
                break;
            case 10:
                tv.GetComponent<TVRemoteControl>().JumpToFrame(_model.frameNumber);
                break;
        }
    }

    public void SetPlayState(int playState)
    {
        DebugHelpers.Log("SetPlayState called, updating model to reflect new playstate of: " + playState);
        _model.playState = playState;
    }

    public void Stop()
    {
        _model.playState = 0;
        //tv.GetComponent<TVRemoteControl>().Stop();
    }
    public void Pause()
    {
        _model.playState = 2;
        //tv.GetComponent<TVRemoteControl>().Pause();
    }

    public void Play()
    {
        Debug.LogError("Play called on synch vid");
        _model.playState = 1;
        //tv.GetComponent<TVRemoteControl>().Play();
    }

    public void JumpToFrame(int frameNum)
    {
        DebugHelpers.Log("skipToFrame: " + frameNum);
        _model.frameNumber = frameNum;
        //tv.GetComponent<TVRemoteControl>().JumpToFrame(frameNum);
    }
}
