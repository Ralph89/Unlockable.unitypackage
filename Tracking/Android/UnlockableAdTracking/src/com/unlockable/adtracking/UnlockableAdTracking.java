package com.unlockable.adtracking;

import android.content.Context;

import com.google.android.gms.ads.identifier.AdvertisingIdClient;
import com.google.android.gms.ads.identifier.AdvertisingIdClient.Info;
import com.google.android.gms.common.GooglePlayServicesNotAvailableException;
import com.google.android.gms.common.GooglePlayServicesRepairableException;

import java.io.IOException;

public class UnlockableAdTracking
{

    private static Context context;
    public static void setContext( Context ctxt )
    {
    	context = ctxt;
    }
	
	public static Boolean adTrackingEnabled() throws IllegalStateException, GooglePlayServicesRepairableException
	{
		Info adInfo = null;
		try 
		{
			adInfo = AdvertisingIdClient.getAdvertisingIdInfo( context );
		} 
		catch (IOException e) 
		{
			//Unrecoverable error connecting to Google Play services (e.g.,
			//the old version of the service doesn't support getting AdvertisingId).
 
		} 
		catch (GooglePlayServicesNotAvailableException e) 
		{
			// Google Play services is not available entirely.
		}
  
		return adInfo.isLimitAdTrackingEnabled();
	}
}