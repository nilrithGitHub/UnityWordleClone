using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public void TestShare()
    {
        Jslib_Share.CopyToClipboardAndShare("Test Share");
    }
    public void ShareOnFacebook()
    {
        Jslib_Share.FacebookShare("Facebook Share");
    }
    public void ShareOnTwitter()
    {
        Jslib_Share.TwitterShare("Twitter Share");
    }
}
