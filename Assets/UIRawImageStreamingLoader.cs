using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RawImage))]
public class UIRawImageStreamingLoader : MonoBehaviour
{
	[SerializeField] StreamingTextureLoader streamingTexture;
	RawImage rawImage;
	public void Load(string url)
	{
		Unload();
		streamingTexture.LoadTexure(url, (tex) => { rawImage.texture = tex; });
	}
	public void Unload()
	{
		GetRawImageComp();
		Texture oldText = rawImage.texture;
		if (oldText != null)
		{
			DestroyImmediate(oldText, false);
		}
	}
	void GetRawImageComp()
	{
		if (rawImage == null)
			rawImage = GetComponent<RawImage>();
	}
}
