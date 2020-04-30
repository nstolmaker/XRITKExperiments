﻿using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.Video;

public class TVRemoteControl : MonoBehaviour
{
    [SerializeField]
    private XRController controller;    // name it "RightHand Controller"
    [SerializeField]
    private XRRayInteractor interactor = null;

    [SerializeField]
    private GameObject tv;  // name it "TVScreen"
    [SerializeField]
    private VideoPlayer videoPlayer;

    //[SerializeField]
    //private string _playbackURL;
    [SerializeField]
    private string previousPlaybackURL;
    private NSTOL_SynchronousVideo _synchronousVideo;

    [SerializeField]
    public Texture m_MainTexture, m_Normal, m_Albedo;
    [SerializeField]
    private Renderer m_Renderer;

    public string videoURL = "https://movietrailers.apple.com/movies/independent/abe/abe-trailer-1_i320.m4v"; //"https://docs.google.com/uc?export=download&id=1LG7un7gOiMhdueo6G88h0YkAmba20ZQC";

    public bool tvReady = false;


    void Start()
    {
        //DebugHelpers.Log("starting tv remote control");
        if (!controller)
        {
            controller = GameObject.Find("RightHand Controller").GetComponent<XRController>();
        }

        interactor = controller.GetComponent<XRRayInteractor>();

        // get a reference for the tv
        if (!tv)
        {
            tv = GameObject.Find("TVScreen");
        }

        if (!tv || !controller)
        {
            throw new System.Exception("TV or Controller not found. TVRemoteControl failed to start. Check the TVRemoteControl.cs file and confirm you didn't rename the game objects");
        }

        // Get a reference to the NSTOL_SynchronousVideo component.
        _synchronousVideo = GetComponent<NSTOL_SynchronousVideo>();
        previousPlaybackURL = videoURL;

        CheckDimensions(videoURL);
    }

    // Update is called once per frame
    void Update()
    {

        if (tvReady)
            CheckForTVCommands();



    }

    /* 
     * I stole this code from the internet and only modified it slightly. i'm suspicious of whether it loads the entire video or not in order to do this test.
     * */
    public void CheckDimensions(string url)
    {
        Debug.LogError("CheckDimensions(" + url + ")");

        GameObject tempVideo = new GameObject("Temp video for " + url);
        VideoPlayer tmpvideoPlayer = tempVideo.AddComponent<VideoPlayer>();
        tmpvideoPlayer.renderMode = VideoRenderMode.MaterialOverride;
        tmpvideoPlayer.targetTexture = new RenderTexture(1, 1, 0);
        tmpvideoPlayer.source = VideoSource.Url;
        tmpvideoPlayer.url = url;
        tmpvideoPlayer.playOnAwake = false;

        tmpvideoPlayer.prepareCompleted += (VideoPlayer source) =>
        {
            DebugHelpers.Log("dimensions " + source.texture.width + " x " + source.texture.height);
            Debug.LogError("CheckDimensions complete. dimensions " + source.texture.width + " x " + source.texture.height);
            SetupTV(source.texture.width, source.texture.height);
            Destroy(tempVideo);
        };
        tmpvideoPlayer.Prepare();
    }

    /* 
     * Called by CheckDimensions when its Prepare() step is complete 
     */
    void SetupTV(int remoteVidWidth, int remoteVidHeight)
    {

        //Debug.LogError("SetupTV(" + remoteVidWidth + ")");

        // first MeshRenderer
        if (tv.GetComponent<MeshRenderer>() != null)
        {
            Destroy(tv.GetComponent<MeshRenderer>());
        }
        m_Renderer = tv.AddComponent<MeshRenderer>();

        // then make the video texture
        Texture2D VideoTextureOTF = new Texture2D(remoteVidWidth, remoteVidHeight, TextureFormat.RGBA32, false);   // set to video with and height

        // now make the material
        Material material = new Material(Shader.Find("Standard"));
        //This will save the new object to the project if we want it to: AssetDatabase.CreateAsset(material, "Assets/VideoMaterialOTF.mat");

        // now assign the texture to the material
        material.SetTexture("VideoTextureOTF", m_Albedo);

        // asign the new material to our renderer
        m_Renderer.material = material;

        // now make the videoPlayer
        if (tv.GetComponent<UnityEngine.Video.VideoPlayer>() != null)
        {
            Destroy(tv.GetComponent<UnityEngine.Video.VideoPlayer>());
        }
        videoPlayer = tv.AddComponent<UnityEngine.Video.VideoPlayer>();

        /* commenting this out seemed to fix the double-audio issue.
        if (gameObject.GetComponent<AudioSource>() != null)
        {
            Destroy(gameObject.GetComponent<AudioSource>());
        }
        var audioSource = gameObject.AddComponent<AudioSource>();
        */
        //videoPlayer.clip = videoClip;
        videoPlayer.renderMode = UnityEngine.Video.VideoRenderMode.MaterialOverride;
        videoPlayer.targetMaterialRenderer = m_Renderer;
        videoPlayer.targetMaterialProperty = "_MainTex";
        /* commenting this out seemed to fix the double-audio issue. */
        //videoPlayer.audioOutputMode = UnityEngine.Video.VideoAudioOutputMode.AudioSource;
        //videoPlayer.SetTargetAudioSource(0, audioSource);
        videoPlayer.playOnAwake = false;
        videoPlayer.isLooping = true;
        videoPlayer.url = videoURL;
        //videoPlayer.skipOnDrop = true;

        tvReady = true;
        //Debug.LogError("SetupTV Complete");
    }

    public void CheckForTVCommands()
    {
        /*
        // trigger button pressed
        if (controller.inputDevice.TryGetFeatureValue(CommonUsages.triggerButton, out bool trigger))
        {
            if (trigger)
            {
                DebugHelpers.Log("Height/Width: " + videoPlayer.height + " / " + videoPlayer.width);
                DebugHelpers.Log("Primary A Button Capacitive touch: " + trigger);

                Debug.LogError("CheckForTVCommands() trigger pressed. isPlaying = " + tv.GetComponent<UnityEngine.Video.VideoPlayer>().isPlaying);
                if (tv.GetComponent<UnityEngine.Video.VideoPlayer>().isPlaying == true)
                {
                    tv.GetComponent<UnityEngine.Video.VideoPlayer>().Pause();
                } else
                {
                    tv.GetComponent<UnityEngine.Video.VideoPlayer>().Play();
                }
            }
        }
        */

        if (videoURL != previousPlaybackURL)
        {
            // Step 1. the video player URL changed, so update the model. 
            Debug.LogError("Step 1 TVRemoteControl.cs detect vid url change to " + videoURL);
            DebugHelpers.Log("Step 1 TVRemoteControl.cs detect vid url change to " + videoURL);
            previousPlaybackURL = videoURL;
            // if the new videoURL is not already set as the VideoPlayer's URL, then propogate this out to the synchronous component.
            // NOTE: It might be that we can just use the video property itself, instead of this videoURL variable, but I think this additional layer of abstraction will be a good thing.
            if (videoURL != GetComponent<VideoPlayer>().url)
            {
                Debug.LogError("Step 1b TVRemoteControl.cs calling SetVideoURL" + videoURL);
                DebugHelpers.Log("Step 1b TVRemoteControl.cs calling SetVideoURL" + videoURL);
                _synchronousVideo.SetVideoURL(videoURL);
            }

        }
    }

    public void JumpToFrame(int frameNum)
    {
        Debug.LogError("Skipping to frame " + frameNum);
        tv.GetComponent<VideoPlayer>().frame = frameNum;
    }

    public void Stop()
    {
        Debug.LogError("Stopping VideoPlayer");
        tv.GetComponent<VideoPlayer>().Stop();
    }

    public void Play()
    {
        Debug.LogError("Starting VideoPlayer");
        tv.GetComponent<VideoPlayer>().Play();
    }

    public void Pause()
    {
        Debug.LogError("Pausing VideoPlayer");
        tv.GetComponent<VideoPlayer>().Pause();
    }
}
