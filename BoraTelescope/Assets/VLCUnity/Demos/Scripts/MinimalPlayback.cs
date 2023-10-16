using UnityEngine;
using System;
using LibVLCSharp;
using UnityEngine.UI;

/// this class serves as an example on how to configure playback in Unity with VLC for Unity using LibVLCSharp.
/// for libvlcsharp usage documentation, please visit https://code.videolan.org/videolan/LibVLCSharp/-/blob/master/docs/home.md
public class MinimalPlayback : MonoBehaviour
{
    public GameManager gamemanager;
    public RawImage Streaming;
    public GameObject ReadyImg;

    LibVLC _libVLC;
    MediaPlayer _mediaPlayer;
    const int seekTimeDelta = 5000;
    Texture2D tex = null;
    bool playing;
    public static string path;
    public string path_1;

    public void Start()
    {
        Core.Initialize(Application.dataPath);

        _libVLC = new LibVLC(enableDebugLogs: true, "--no-osd");

        Application.SetStackTraceLogType(LogType.Log, StackTraceLogType.None);
        //_libVLC.Log += (s, e) => UnityEngine.Debug.Log(e.FormattedLog); // enable this for logs in the editor
        gamemanager.WriteLog(LogSendServer.NormalLogCode.ChangeMode, "LiveStreaming ReadyStart", GetType().ToString());
        PlayPause();
    }

    public void SeekForward()
    {
        Debug.Log("[VLC] Seeking forward !");
        _mediaPlayer.SetTime(_mediaPlayer.Time + seekTimeDelta);
    }

    public void SeekBackward()
    {
        Debug.Log("[VLC] Seeking backward !");
        _mediaPlayer.SetTime(_mediaPlayer.Time - seekTimeDelta);
    }

    void OnDisable() 
    {
        _mediaPlayer?.Stop();
        _mediaPlayer?.Dispose();
        _mediaPlayer = null;

        _libVLC?.Dispose();
        _libVLC = null;
    }

    public void PlayPause()
    {
        Debug.Log ("[VLC] Toggling Play Pause !");
        if (_mediaPlayer == null)
        {
            _mediaPlayer = new MediaPlayer(_libVLC);
        }
        if (_mediaPlayer.IsPlaying)
        {
            _mediaPlayer.Pause();
        }
        else
        {
            playing = true;
            gamemanager.WriteLog(LogSendServer.NormalLogCode.ChangeMode, "LiveStreaming PlayStart", GetType().ToString());
            if (_mediaPlayer.Media == null)
            {
                // playing remote media
                _mediaPlayer.Media = new Media(_libVLC, new Uri(path));    // url 변경 필요
                //var trimmedPath = path.Trim(new char[] { '"' });//Windows likes to copy paths with quotes but Uri does not like to open them
                //_mediaPlayer.Media = new Media(_libVLC, new Uri(trimmedPath));
            }

            _mediaPlayer.Play();
            //this.GetComponent<AutoStreaming>().gamemanager.WriteLog(GameManager.NormalLogCode.Etc_YoutubeLive, "VideoPlayer Play", GetType().ToString());
        }
    }

    public void Stop ()
    {
        Debug.Log ("[VLC] Stopping Player !");

        playing = false;
        _mediaPlayer?.Stop();

        // there is no need to dispose every time you stop, but you should do so when you're done using the mediaplayer and this is how:
        // _mediaPlayer?.Dispose(); 
        // _mediaPlayer = null;
        //GetComponent<RawImage>().texture = null;
        //GetComponent<Renderer>().material.mainTexture = null;
        //this.gameObject.GetComponent<AutoStreaming>().rawim.GetComponent<RawImage>().texture = null;
        tex = null;
    }

    void Update()
    {
        if(!playing) return;

        if (tex == null)
        {
            ReadyImg.SetActive(true);
            // If received size is not null, it and scale the texture
            uint i_videoHeight = 0;
            uint i_videoWidth = 0;

            _mediaPlayer.Size(0, ref i_videoWidth, ref i_videoHeight);
            var texptr = _mediaPlayer.GetTexture(i_videoWidth, i_videoHeight, out bool updated);
            gamemanager.WriteLog(LogSendServer.NormalLogCode.ChangeMode, "LiveStreaming Ready", GetType().ToString());
            if (i_videoWidth != 0 && i_videoHeight != 0 && updated && texptr != IntPtr.Zero)
            {
                Debug.Log("Creating texture with height " + i_videoHeight + " and width " + i_videoWidth);
                tex = Texture2D.CreateExternalTexture((int)i_videoWidth,
                    (int)i_videoHeight,
                    TextureFormat.RGBA32,
                    false,
                    true,
                    texptr);
                Streaming.texture = tex;
                //GetComponent<Renderer>().material.mainTexture = tex;
                //this.gameObject.GetComponent<AutoStreaming>().rawim.GetComponent<RawImage>().texture = tex;
            }
        }
        else if (tex != null)
        {
            var texptr = _mediaPlayer.GetTexture((uint)tex.width, (uint)tex.height, out bool updated);
            if (updated)
            {
                tex.UpdateExternalTexture(texptr);
                Streaming.texture = tex;
                //GetComponent<Renderer>().material.mainTexture = tex;
                //this.gameObject.GetComponent<AutoStreaming>().rawim.GetComponent<RawImage>().texture = tex;

                if (ReadyImg.activeSelf)
                {
                    Invoke("WaitReady", 0.8f);
                }
            }
        }
    }

    public void WaitReady()
    {
        if (ReadyImg.activeSelf)
        {
            gamemanager.WriteLog(LogSendServer.NormalLogCode.ChangeMode, "LiveStreaming Start", GetType().ToString());
            ReadyImg.SetActive(false);
        }
    }
}
