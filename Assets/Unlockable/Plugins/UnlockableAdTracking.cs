using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;

public class UnlockableAdTracking
{
	[DllImport("__Internal")]
	private static extern bool adTrackingEnabled();

	public static bool AdTrackingEnabled()
	{
		if (Application.platform == RuntimePlatform.IPhonePlayer)
			return adTrackingEnabled();

		return true;
	}
}
