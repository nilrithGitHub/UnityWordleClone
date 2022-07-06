using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class Jslib_Share : MonoBehaviour
{
	[DllImport("__Internal")]
	public static extern void CopyToClipboardAndShare(string textToCopy);
	[DllImport("__Internal")]
	public static extern void TwitterShare(string textToCopy);
	[DllImport("__Internal")]
	public static extern void FacebookShare(string textToCopy);
}
