using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(AudioSource))]
public class StreamingAudioLoader : MonoBehaviour
{
    public string filePath;
    public AudioType audioType;
    public GameObject loadingUI;
    public bool playOnEnable;
    AudioSource audioSource;

	private void OnEnable()
	{
		if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
        if (playOnEnable)
            Play(filePath);
	}
	private void OnDisable()
	{
        UnloadClip();
	}
	public void Play(string filePath)
    {
        UnloadClip();
        string url = Path.Combine(Application.streamingAssetsPath, filePath);
        // Load Clip then assign to audio source and play
        LoadClip(url, (clip) => { audioSource.clip = clip; audioSource.Play(); });
    }
    public void UnloadClip ()
	{
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
        // Unload audio clip
        if (audioSource.clip != null)
        {
            audioSource.Stop();
            AudioClip clip = audioSource.clip;
            audioSource.clip = null;
            clip.UnloadAudioData();
            DestroyImmediate(clip, false); // This is important to avoid memory leak
        }
    }
    public void LoadClip(string url, Action<AudioClip> onLoadingCompleted)
    {
        StartCoroutine(LoadClipCoroutine(url, onLoadingCompleted));
    }

    IEnumerator LoadClipCoroutine(string url, Action<AudioClip> onLoadingCompleted)
    {
        UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(url, audioType);

        yield return www.SendWebRequest();

        AudioClip audioClip = DownloadHandlerAudioClip.GetContent(www);

        if (onLoadingCompleted != null)
            onLoadingCompleted (audioClip);

        Debug.Log("Audio Loaded.");

        //AudioSource.PlayClipAtPoint(audioClip, Camera.main.transform.position);

        //www.Dispose();
    }

    async Task<AudioClip> LoadClip(string path)
    {
        AudioClip clip = null;
        using (UnityWebRequest uwr = UnityWebRequestMultimedia.GetAudioClip(path, audioType))
        {
            uwr.SendWebRequest();

            // wrap tasks in try/catch, otherwise it'll fail silently
            try
            {
                while (!uwr.isDone) await Task.Delay(5);

                if (uwr.result == UnityWebRequest.Result.ConnectionError || uwr.result == UnityWebRequest.Result.ProtocolError) Debug.Log($"{uwr.error}");
                else
                {
                    clip = DownloadHandlerAudioClip.GetContent(uwr);
                }
            }
            catch (Exception err)
            {
                Debug.Log($"{err.Message}, {err.StackTrace}");
            }
        }

        return clip;
    }
}
