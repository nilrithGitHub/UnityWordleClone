using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class Jslib_LockScreenOrientation : MonoBehaviour
{
	[DllImport("__Internal")]
	public static extern void TryLockOrientation();
	[DllImport("__Internal")]
	public static extern void GoFullscreen();
	[DllImport("__Internal")]
	public static extern bool IsMobile();
	[DllImport("__Internal")]
	public static extern bool CheckOrientation();
}
