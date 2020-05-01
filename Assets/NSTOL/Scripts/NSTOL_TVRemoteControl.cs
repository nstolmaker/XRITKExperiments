using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.Video;

public class NSTOL_TVRemoteControl : MonoBehaviour
{
    [SerializeField]
    private XRController controller;    // name it "RightHand Controller"
    [SerializeField]
    private XRRayInteractor interactor = null;

    [SerializeField]
    private GameObject tv;  // name it "TVScreen"
    [SerializeField]
    private VideoPlayer videoPlayer;

    [SerializeField]
    private string previousPlaybackURL;
    private NSTOL_SynchronousVideo _synchronousVideo;

    [SerializeField]
    public Texture m_MainTexture, m_Normal, m_Albedo;
    [SerializeField]
    private Renderer m_Renderer;

    [SerializeField]
    public int playState = 0;
    [SerializeField]
    public int _previousPlayState;
    public string videoURL = "https://movietrailers.apple.com/movies/independent/abe/abe-trailer-1_i320.m4v";  

    public bool tvReady = false;


    void Start()
    {
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
        _previousPlayState = playState;

        CheckDimensions(videoURL);
    }

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
        //Debug.Log("CheckDimensions(" + url + ")");

        GameObject tempVideo = new GameObject("Temp video for " + url);
        VideoPlayer tmpvideoPlayer = tempVideo.AddComponent<VideoPlayer>();
        tmpvideoPlayer.renderMode = VideoRenderMode.MaterialOverride;
        tmpvideoPlayer.targetTexture = new RenderTexture(1, 1, 0);
        tmpvideoPlayer.source = VideoSource.Url;
        tmpvideoPlayer.url = url;
        tmpvideoPlayer.playOnAwake = false;

        tmpvideoPlayer.prepareCompleted += (VideoPlayer source) =>
        {
            NSTOL_DebugHelpers.Log("dimensions " + source.texture.width + " x " + source.texture.height);
            Debug.Log("CheckDimensions complete. dimensions " + source.texture.width + " x " + source.texture.height);
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

        NSTOL_DebugHelpers.Log("SetupTV(" + remoteVidWidth + "c" + remoteVidHeight+")");

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
        if (videoURL != previousPlaybackURL)
        {
            // Step 1. the video player URL changed, so update the model. 
            Debug.Log("Step 1 TVRemoteControl.cs detect vid url change to " + videoURL);
            NSTOL_DebugHelpers.Log("Step 1 TVRemoteControl.cs detect vid url change to " + videoURL);
            previousPlaybackURL = videoURL;
            // if the new videoURL is not already set as the VideoPlayer's URL, then propogate this out to the synchronous component.
            // NOTE: It might be that we can just use the video property itself, instead of this videoURL variable, but I think this additional layer of abstraction will be a good thing.
            if (videoURL != GetComponent<VideoPlayer>().url)
            {
                Debug.Log("Step 1b TVRemoteControl.cs calling SetVideoURL" + videoURL);
                NSTOL_DebugHelpers.Log("Step 1b TVRemoteControl.cs calling SetVideoURL" + videoURL);
                _synchronousVideo.SetVideoURL(videoURL);
            }

        }
        if (playState != _previousPlayState)
        {
            Debug.Log("TVRemoteControl Playstate changed to: " + playState);
            NSTOL_DebugHelpers.Log("TVRemoteControl Playstate changed to: " + playState);
            _previousPlayState = playState;
            _synchronousVideo.SetPlayState(playState);

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
