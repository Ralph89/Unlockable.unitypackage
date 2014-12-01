using UnityEngine;
using System.Collections;

public class Tester : MonoBehaviour
{
	bool hasLoaded = false;

	void Update () {
		if( !Application.isLoadingLevel && !hasLoaded )
		{
			hasLoaded = true;
			Debug.Log( UnlockableAdTracking.AdTrackingEnabled() );
		}
	}
}
