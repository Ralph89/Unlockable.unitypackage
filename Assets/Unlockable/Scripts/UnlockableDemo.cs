using UnityEngine;
using System;
using System.Collections.Generic;
using System.Text;
using SimpleJSON;

public class UnlockableDemo : MonoBehaviour
{
	//public UniWebView webview;
	public string secretKey			= "super_secret_key";
	public string public_key 		= "gDL5oEyEwjZ5so29Hy1ZZp82ZG51uW43";
	public string opt_out_tracking 	= "false";
	public string idfa	 			= "rAL1wxh00xtmQV41hn0Y91t12v2g7st1";
	public string prize				= "example_prize";
	public string source 			= "example_source";
	public string age_13_or_over	= "true";
	public string country_code		= "US";
	public string timestamp 		= "2014-09-21T20:50:54.12876Z";
	public string sig_token 		= "013523fab7950bcf67db762f6a2baef397a62408ca97a995e33aeea71245bc950f22ccc83f55c0cf05c79de3484dd81734c56a5d21adc217a3ae904ff0aca19c";
	public string fsession_id 		= "abc123";

	string result = "";
	bool receivedValidResponse, showingWebView;
	string unlockableInventoryURL = "";

	void Start()
	{
		//Listen for events from Unlockable
		Unlockable.onResult += OnResult;
		Unlockable.onError += (x,y) => result += string.Format ("Status Code: {0}. Status Description: {1}{2}", x, y, System.Environment.NewLine);

		Unlockable.Init();

		//Uncomment for UniWebView support
		//if (webview != null) {
		//		webview.OnLoadComplete 		+= HandleOnLoadComplete;
		//		webview.OnReceivedMessage 	+= HandleOnReceivedMessage;
		//}

		//You can get your IDFA from IPhone.advertisingIdentifier; Empty when you run it in the editor
		//NOTE: Android user must use google play to get their IDFA
		#if !UNITY_EDITOR && UNITY_IPHONE
		idfa = iPhone.advertisingIdentifier;
		#endif
	}

	void OnGUI()
	{
		//Check to see if the webview is showing, helps with performance due to OnGUI being horrible
		if( !showingWebView )
		{
			GUILayout.BeginHorizontal ();
			GUILayout.BeginVertical ();

			GUILayout.Label ( "secretKey" );
			secretKey 			= GUILayout.TextArea ( secretKey, 	GUILayout.Width(300) );
			GUILayout.Label ( "public_key" );
			public_key 			= GUILayout.TextArea ( public_key, 	GUILayout.Width(300) );
			GUILayout.Label ( "opt_out_tracking" );
			opt_out_tracking 	= GUILayout.TextArea ( opt_out_tracking, GUILayout.Width(300) );
			GUILayout.Label ( "idfa_ios" );
			idfa 				= GUILayout.TextArea ( idfa, 	GUILayout.Width(300) );
			GUILayout.Label ( "prize" );
			prize 				= GUILayout.TextArea ( prize, 		GUILayout.Width(300) );
			GUILayout.Label ( "source" );
			source 				= GUILayout.TextArea ( source, 		GUILayout.Width(300) );
			GUILayout.Label ( "age_13_or_over" );
			age_13_or_over 		= GUILayout.TextArea ( age_13_or_over, 	GUILayout.Width(300) );
			GUILayout.Label ( "country_code" );
			country_code 		= GUILayout.TextArea ( country_code, 	GUILayout.Width(300) );
			GUILayout.Label ( "timestamp" );
			timestamp 			= GUILayout.TextArea ( timestamp, 	GUILayout.Width(300) );
			GUILayout.Label ( "sig_token" );
			sig_token 			= GUILayout.TextArea ( sig_token, 	GUILayout.Width(300) );
			GUILayout.Label ( "fsession_id" );
			fsession_id 		= GUILayout.TextArea ( fsession_id, GUILayout.Width(300) );

			//Always request inventory at the start of your game session
			if( GUILayout.Button( "Request Inventory" ) )
			{
				//hash our secret key
				string hashString = Unlockable.GetHashString( secretKey, fsession_id, timestamp );	//Hex formatted
				//Make sure the hash is correct
				if( !Unlockable.CheckHash( secretKey, fsession_id, timestamp, hashString ) )
					Debug.LogError( "Hash does not match" );

				//Request Inventory
				Unlockable.RequestInventory(public_key,
																		idfa,
																		prize,
																		source,
																		age_13_or_over,
																		country_code,
																		timestamp,
																		hashString,
																		fsession_id,
																		UnlockableUserAgent.ANDROID );
			}

			//If we've received a valid respons we will enable
			if( receivedValidResponse && GUILayout.Button( "Launch Webview" ) )
				LaunchWebView();

			GUILayout.EndVertical ();
			GUILayout.BeginVertical ();
			result = GUILayout.TextArea (result, GUILayout.Width (600), GUILayout.Height (400));
			GUILayout.EndVertical ();
			GUILayout.EndHorizontal ();
		}
	}

	void LaunchWebView()
	{
		//Uncomment for UniWebView support
		//webview.Load( unlockableInventoryURL );
	}

	/// <summary>
	/// Request event was a succes
	/// </summary>
	/// <param name="serverResponse">Server response in JSON a formatted string.</param>
	void OnResult( string serverResponse )
	{
		result += serverResponse + Environment.NewLine;
		//parse the JSON string for the url
		JSONNode resp = JSON.Parse (serverResponse);
		unlockableInventoryURL = resp["data"]["game_url"];

		receivedValidResponse = true;
	}

	//Uncomment for UniWebView support
	/*
	//Succesfully loaded the url, we can now show the webview
	void HandleOnLoadComplete (UniWebView webView, bool success, string errorMessage)
	{
		if (success)
		{
			webview.Show ();
			showingWebView = true;
		}
		else
			result += errorMessage + Environment.NewLine;
	}

	//We received a message from the webview that we are done
	void HandleOnReceivedMessage (UniWebView webView, UniWebViewMessage message)
	{
		result += "Message received from webview---------------" + Environment.NewLine;
		foreach( KeyValuePair<string, string> kvp in message.args )
		{
			result += string.Format( "Received arg: {0} for: {1}", kvp.Value, kvp.Key  ) + Environment.NewLine;
		}

		if (message.path.Contains( "quit" ) || message.path.Contains( "finished" ) )
		{
			webview.Hide ();
			showingWebView = false;
		}
	}
	*/
}
