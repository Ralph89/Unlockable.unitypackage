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
		#if UNITY_IPHONE
		return adTrackingEnabled();
		#elif UNITY_ANDROID
		bool result = false;
		/*using (var unlockableAdTracking = new AndroidJavaObject("com.unlockable.adtracking.UnlockableAdTracking"))
		{
			if (unlockableAdTracking != null)
			{
				result = unlockableAdTracking.Call<bool>("adTrackingEnabled");
			}
		}
		*/
		using(AndroidJavaClass activityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
		{
			AndroidJavaObject activityContext = activityClass.GetStatic<AndroidJavaObject>("currentActivity");
			using(AndroidJavaClass pluginClass = new AndroidJavaClass("com.unlockable.adtracking.UnlockableAdTracking"))
			{
				if(pluginClass != null)
				{
					AndroidJavaObject unlockableAdTracking = pluginClass.CallStatic<AndroidJavaObject>("instance");
					unlockableAdTracking.Call("setContext", activityContext);
					result = unlockableAdTracking.Call<bool>("adTrackingEnabled");
				}
			}
		}

		return result;
		#endif
	}
}
