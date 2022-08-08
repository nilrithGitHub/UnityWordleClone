using System;
using System.Collections;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Networking;
using UnityEngine.Rendering;

///<summary>
/// Load and use an image from Streaming Assets for a Skybox texture.
///</summary>
///<remarks>
/// Works with an equirectangular (with seam), but not a cube map.
/// GOAL: Either modify this to work with a cube map,
/// or with 6 individual side textures (+x, -x, +y, -y, +z, -z)
/// 3rd Option is to figure out how to fix the seem on the equirectangular
/// without having to adjust the import settings
///</remarks>
public class StreamingTextureLoader : MonoBehaviour
{
    public string filePath;
    public Material skyBoxMaterial;
    //public Material testMat;
    public string textureHas = "_Tex";
    string url;
    TextureFormat format = TextureFormat.RGB24;
    int size = 512;
    public GameObject loadingUI;

    void Start()
    {
       // StartCoroutine(TextureLoader());
    }

    public void Load ()
	{
        StartCoroutine(TextureLoader());
    }
    public void LoadTexure(string url, Action<Texture> onLoadingCompleted)
    {
        StartCoroutine(LoadTextureCoroutine(url, onLoadingCompleted));
    }
    public IEnumerator LoadTextureCoroutine(string path, Action<Texture> onLoadingCompleted)
    {
        url = Path.Combine(Application.streamingAssetsPath, path);
        Debug.Log(url);
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);

        yield return www.SendWebRequest();

        Texture tex = DownloadHandlerTexture.GetContent(www);

        if (onLoadingCompleted != null)
            onLoadingCompleted(tex);

        Debug.Log("Texture Loaded.");

        //AudioSource.PlayClipAtPoint(audioClip, Camera.main.transform.position);

        //www.Dispose();
    }
    //public async Task<Texture> GetStreamingTexture(string path)
    //{
    //    url = Path.Combine(Application.streamingAssetsPath, path);
    //    Debug.Log(url);
    //    Texture tex = null;
    //    using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(url, true))
    //    {
    //        uwr.SendWebRequest();

    //        // wrap tasks in try/catch, otherwise it'll fail silently
    //        try
    //        {
    //            while (!uwr.isDone) await Task.Delay(5);

    //            if (uwr.result == UnityWebRequest.Result.ConnectionError || uwr.result == UnityWebRequest.Result.ProtocolError) Debug.Log($"{uwr.error}");
    //            else
    //            {
    //                tex = DownloadHandlerTexture.GetContent(uwr);
    //                Debug.Log("Texture Loaded.");
    //            }
    //        }
    //        catch (Exception err)
    //        {
    //            Debug.Log($"{err.Message}, {err.StackTrace}");
    //        }
    //    }

    //    return tex;
    //}
    IEnumerator TextureLoader()
    {
        // StreamingAssets/ + file path
        loadingUI?.SetActive(true);
        url = Path.Combine(Application.streamingAssetsPath, filePath);

        // create byte array
        byte[] imgData;

        // create new texture
        Texture2D tex = new Texture2D(size, size, textureFormat: format, false);
		// I thought this might work for cubemap, but its not compatible with LoadImage ...
		// Cubemap tex = new Cubemap (2048, TextureFormat.RGBA32, false);
		//accomodate for UnityWebRequest or File.ReadAllBytes
		if (url.Contains("://") || url.Contains(":///"))
		{
			UnityWebRequest www = UnityWebRequest.Get(url);
            yield return www.SendWebRequest();
            imgData = www.downloadHandler.data;
		}
		else
		{
			imgData = File.ReadAllBytes(url);
		}
		//Check if data loaded
		Debug.Log(imgData.Length);

        //Load raw Data into Texture2D
        tex.LoadImage(imgData, false);

		// Convert to cubemap
		// texRef is your Texture2D
		// You can also reduice your texture 2D that way
		//RenderTexture rt = new RenderTexture(size, size, 24);
		//RenderTexture.active = rt;
		// Copy your texture ref to the render texture
		//Graphics.Blit(tex, rt);

		//// Now you can read it back to a Texture2D if you care
		//if (tex == null)
		//    tex = new Texture2D(rt.width, rt.height, TextureFormat.RGB24, true);
		//tex.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0, false);

		//int zBufferDepth = 24;
		//RenderTexture target = new RenderTexture(size, size, zBufferDepth);
		//      Graphics.Blit(tex, target);
		//Cubemap cubemap = new Cubemap(size, UnityEngine.Experimental.Rendering.DefaultFormat.HDR, 0);

		//Cubemap cubemap = PanoramicToCubemapRuntimeConverter.ConvertPanoramaTextureToCubemap(tex, size, generateMipMaps: false);


        CheckFormat (RenderTextureFormat.RInt);
        CheckFormat(RenderTextureFormat.RGInt);
        CheckFormat(RenderTextureFormat.RGFloat);
        CheckFormat(RenderTextureFormat.ARGBInt);
        CheckFormat(RenderTextureFormat.ARGB64);
        CheckFormat(RenderTextureFormat.ARGB1555);
        CheckFormat(RenderTextureFormat.RGHalf);
        CheckFormat(RenderTextureFormat.ARGBHalf);
        CheckFormat(RenderTextureFormat.ARGB4444);
        CheckFormat(RenderTextureFormat.ARGB32);
        CheckFormat(RenderTextureFormat.ARGB2101010);
        CheckFormat(RenderTextureFormat.BGR101010_XR);
        CheckFormat(RenderTextureFormat.BGRA10101010_XR);
        CheckFormat(RenderTextureFormat.BGRA32);
        CheckFormat(RenderTextureFormat.Shadowmap);
        CheckFormat(RenderTextureFormat.RHalf);
        CheckFormat(RenderTextureFormat.RGBAUShort);
        CheckFormat(RenderTextureFormat.RGB565);


        //Cubemap cubemap = PanoramicToCubemapRuntimeConverter.ConvertPanoramaTextureToCubemap(tex, size, 2, format, false);


        // WITH SKYBOX MATERIAL
        Texture oldText = skyBoxMaterial.GetTexture(textureHas);
        if (oldText != null)
            DestroyImmediate(oldText, false);

        skyBoxMaterial.SetTexture(textureHas, tex);
        //testMat.SetTexture(textureHas, tex);
        loadingUI?.SetActive(false);
    }
    void CheckFormat (RenderTextureFormat format)
	{
        if (SystemInfo.SupportsRandomWriteOnRenderTextureFormat(format))
		{
            Debug.Log("Support: " + format.ToString());
		}
	}
    public void Load_RGB24 ()
	{
        this.format = TextureFormat.RGB24;
        Load();
	}
    public void Load_RGBA32()
	{
        format = TextureFormat.RGBA32;
        Load();
	}
    public void Load_DXT1()
	{
        format = TextureFormat.DXT1;
        Load();
	}
    public void Load_DXT5()
	{
        format = TextureFormat.DXT5;
        Load();
    }
    public void Load_ETC2RGB()
	{
        format = TextureFormat.ETC2_RGB;
        Load();
    }
    public void Load_RGB48()
	{
        format = TextureFormat.RGB48;
        Load();
    }
    public void Load_ATC4x4()
	{
        format = TextureFormat.ASTC_4x4;
        Load();
    }
    public void SetSize512Multi(int value)
	{
        size = 512 * value;
	}
}