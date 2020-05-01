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
    void Awake()
    {
        if (!tv)
        {
            tv = GameObject.Find("TVScreen");
        }
    }

    // As i understand it, this implements the NSTOL_SynchronousVideoModel partial.
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
        // the plan is to calculate the time variance and then modify the playback speed paramater for like 0.5s or something to get it back into sync.
        NSTOL_DebugHelpers.Log("Smoothing to new frame " + frameNumber);

    }

    public void SetFrame(int frameNumber)
    {
        _model.frameNumber = frameNumber;
    }


    public void VideoURLDidChange(NSTOL_SynchronousVideoModel model, string videoURL)
    {
        // update local video url to new url
        //DebugHelpers.Log("Step 3 Loading new video URL: " + videoURL);
        tv.GetComponent<VideoPlayer>().url = videoURL;  // this updates the actual video player url. It might make more sense to trigger CheckDimensions or Setup() with an argument and let it handle the change.
        tv.GetComponent<NSTOL_TVRemoteControl>().videoURL = videoURL; // this updates the TVRemoteControl component, which is what we read from mostly. That might be kinda confusing.
    }

    public void SetVideoURL(string videoURL)
    {
        //DebugHelpers.Log("Step 2.");
        _model.playbackURL = videoURL;
    }

    private void PlayStateDidChange(NSTOL_SynchronousVideoModel model, int playState)
    {
        Debug.Log("PlayStateDidChange event triggered, updating to: " + playState);
        // !!! TODO: convert to ENUMs.
        tv.GetComponent<NSTOL_TVRemoteControl>().playState = playState;
        switch (playState)
        {
            case 0:
                tv.GetComponent<UnityEngine.Video.VideoPlayer>().Stop();
                break;
            case 1:
                tv.GetComponent<UnityEngine.Video.VideoPlayer>().Play();
                break;
            case 2:
                tv.GetComponent<UnityEngine.Video.VideoPlayer>().Pause();
                break;
            case 3:
                break;
            case 10:
                tv.GetComponent<NSTOL_TVRemoteControl>().JumpToFrame(_model.frameNumber);
                break;
        }
    }

    public void SetPlayState(int playState)
    {
        Debug.Log("SetPlayState called, updating model to reflect new playstate of: " + playState);
        _model.playState = playState;
    }

    public void Stop()
    {
        _model.playState = 0;
    }
    public void Pause()
    {
        _model.playState = 2;
    }

    public void Play()
    {
        _model.playState = 1;
    }

    public void JumpToFrame(int frameNum)
    {
        NSTOL_DebugHelpers.Log("skipToFrame: " + frameNum);
        _model.frameNumber = frameNum;
    }
}
