using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.IO;
using System.Text;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

/// <summary>
/// Unlockable.
/// </summary>
public class UnlockableSSL : MonoBehaviour 
{
	const string ENPOINT_URL = "http://api.unlockable.com/beta/initiate/";

	public event Action<string> onResult;					//Returns the result as a JSON String
	public event Action<string> onError;		//Returns Status code and description

	void Start()
	{
		//Accept all certificates
		ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
	}

	/// <summary>
	/// Requests the inventory.
	/// </summary>
	/// <param name="public_key">Public_key.</param>
	/// <param name="opt_out_tracking">Opt_out_tracking.</param>
	/// <param name="idfa_ios">Idfa_ios.</param>
	/// <param name="prize">Prize.</param>
	/// <param name="source">Source.</param>
	/// <param name="age_13_or_over">Age_13_or_over.</param>
	/// <param name="country_code">Country_code.</param>
	/// <param name="timestamp">Timestamp.</param>
	/// <param name="sig_token">Sig_token.</param>
	/// <param name="fsession_id">Fsession_id.</param>
	public void RequestInventory( string public_key, string opt_out_tracking, string idfa, string prize, string source, string age_13_or_over, 
	                             string country_code, string timestamp, string sig_token, string fsession_id, UnlockableUserAgent userAgent )
	{
		WWWForm form = new WWWForm ();
		form.AddField( "public_key", 	public_key);
		form.AddField( "opt_out_tracking", opt_out_tracking);
		form.AddField( (userAgent == UnlockableUserAgent.IOS) ? "idfa_ios" : "idfa_android", idfa);
		form.AddField( "prize", 		prize);
		form.AddField( "source", 		source);
		form.AddField( "age_13_or_over",age_13_or_over);
		form.AddField( "country_code", 	country_code);
		form.AddField( "timestamp", 	timestamp);
		form.AddField( "sig_token", 	sig_token);
		form.AddField( "fsession_id", 	fsession_id);
		form.AddField( "User-Agent", 	UnlockableUtils.GetDescription( userAgent ));

		StartCoroutine( StartRequest( form ) );
	}



	IEnumerator StartRequest( WWWForm form )
	{
		WWW req = new WWW (ENPOINT_URL, form);

		yield return req;

		if( !string.IsNullOrEmpty(req.error) )
			onError( req.error );
		else if( onResult != null )
			onResult( req.text );
	}



	/// <summary>
	/// Get's the hash string.
	/// </summary>
	/// <returns>The hash string.</returns>
	/// <param name="secret_key">Secret_key.</param>
	/// <param name="fsession">Fsession.</param>
	/// <param name="timeStamp">Time stamp.</param>
	public string GetHashString( string secret_key, string fsession, string timeStamp )
	{
		byte[] hashValue;
		byte[] message = Encoding.UTF8.GetBytes( secret_key + fsession + timeStamp );   

		SHA512Managed hashString = new SHA512Managed();
		using (SHA512 shaM = new SHA512Managed())
			hashValue = shaM.ComputeHash(message);

		string hex = "";
		foreach (byte x in hashValue)
			hex += String.Format("{0:x2}", x);

		return hex;
	}

	/// <summary>
	/// Checks the hash.
	/// </summary>
	/// <returns><c>true</c>, if hash was checked, <c>false</c> otherwise.</returns>
	/// <param name="secret_key">Secret_key.</param>
	/// <param name="fsession">Fsession.</param>
	/// <param name="timeStamp">Time stamp.</param>
	/// <param name="hashString">Hash string.</param>
	public bool CheckHash(string secret_key, string fsession, string timeStamp, string hashString )
	{
		string originalHash = GetHashString( secret_key, fsession,  timeStamp );
		return (originalHash == hashString);
	}
}
