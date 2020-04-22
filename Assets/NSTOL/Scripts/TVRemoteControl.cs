using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class TVRemoteControl : MonoBehaviour
{
    private XRController controller;    // name it "RightHand Controller"
    private XRRayInteractor interactor = null;

    private GameObject tv;  // name it "TVScreen"

    void Start()
    {
        DebugHelpers.Log("starting tv remote control");
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

        SetupTV();
        //        StartCoroutine(GetMovieTexture());

    }

    /*IEnumerator GetMovieTexture()
    {
        using (var uwr = UnityWebRequestMultimedia.GetMovieTexture("http://myserver.com/mymovie.ogv"))
        {
            yield return uwr.SendWebRequest();
            if (uwr.isNetworkError || uwr.isHttpError)
            {
                Debug.LogError(uwr.error);
                yield break;
            }

            MovieTexture movie = DownloadHandlerMovieTexture.GetContent(uwr);

            GetComponent<Renderer>().material.mainTexture = movie;
            movie.loop = true;
            movie.Play();
        }
    }
    */
    // Update is called once per frame
    void Update()
    {
        CheckForTVCommands();
    }

    public void SetupTV()
    {
        var videoPlayer = tv.AddComponent<UnityEngine.Video.VideoPlayer>();
        var audioSource = gameObject.AddComponent<AudioSource>();

        //videoPlayer.clip = videoClip;
        videoPlayer.renderMode = UnityEngine.Video.VideoRenderMode.MaterialOverride;
        videoPlayer.targetMaterialRenderer = GetComponent<Renderer>();
        videoPlayer.targetMaterialProperty = "_MainTex";
        videoPlayer.audioOutputMode = UnityEngine.Video.VideoAudioOutputMode.AudioSource;
        videoPlayer.SetTargetAudioSource(0, audioSource);
        videoPlayer.isLooping = true;

        videoPlayer.url = "https://file-examples.com/wp-content/uploads/2017/04/file_example_MP4_1280_10MG.mp4";
    }

    public void CheckForTVCommands()
    {

        // trigger button pressed
        if (controller.inputDevice.TryGetFeatureValue(CommonUsages.triggerButton, out bool trigger))
        {
            if (trigger)
            {
                DebugHelpers.Log("Primary A Button Capacitive touch: " + trigger);
                if (tv.GetComponent<UnityEngine.Video.VideoPlayer>().isPlaying == true)
                {
                    tv.GetComponent<UnityEngine.Video.VideoPlayer>().Pause();
                } else
                {
                    tv.GetComponent<UnityEngine.Video.VideoPlayer>().Play();
                }
            }
        }
    }

    public void SwapVideo()
    {
        // change aspect ratio of texture VideoTexture
        // change video clip in VideoPlayer component
        // make sure VideoMaterial's Rendering Maps are set to the VideoTexture still
        // make sure the VideoMaterial is still assigned to the material of MeshRenderer component
    }

    //
}
