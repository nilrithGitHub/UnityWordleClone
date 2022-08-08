using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public string audioUrl;
    public string uiRawImgUrl;
    public UIRawImageStreamingLoader uIRawImageStreamingLoader;
    public StreamingAudioLoader streamingAudioLoader;
    public void TestShare()
    {
        Jslib_Share.CopyToClipboardAndShare("Test Share");
    }
    public void ShareOnFacebook()
    {
        //Jslib_Share.FacebookShare("Facebook Share");
        streamingAudioLoader.Play(audioUrl);
        uIRawImageStreamingLoader.Load(uiRawImgUrl);
    }
    public void ShareOnTwitter()
    {
        Jslib_Share.TwitterShare("Twitter Share");
    }
}
