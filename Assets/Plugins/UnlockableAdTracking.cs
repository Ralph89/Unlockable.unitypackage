using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;

public class UnlockableAdTracking
{
	#if UNITY_IPHONE
	[DllImport("__Internal")]
	private static extern bool adTrackingEnabled();
	#endif

	public static bool AdTrackingEnabled()
	{
		#if UNITY_IPHONE && !UNITY_EDITOR
		return adTrackingEnabled();
		#elif UNITY_ANDROID && !UNITY_EDITOR
		bool result = false;

		using(AndroidJavaClass activityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
		{
			AndroidJavaObject activityContext = activityClass.GetStatic<AndroidJavaObject>("currentActivity");
			using(AndroidJavaObject pluginClass = new AndroidJavaObject("com.unlockable.adtracking.UnlockableAdTracking"))
			{
				if(pluginClass != null)
				{
					pluginClass.CallStatic("setContext", activityContext);
					result = pluginClass.CallStatic<bool>("adTrackingEnabled");
				}
			}
		}

		return result;
		#else
		return true;
		#endif
	}
}
