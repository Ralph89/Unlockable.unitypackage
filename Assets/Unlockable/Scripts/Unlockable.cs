using UnityEngine;
using System;
using System.Net;
using System.IO;
using System.Text;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using SimpleJSON;

/// <summary>
/// Unlockable.
/// </summary>
public static class Unlockable
{
	const string ENDPOINT_URL = "http://api.unlockable.com/v1/initiate/";

	public static event Action<string> onResult;					//Returns the result as a JSON String
	public static event Action<string, string> onError;		//Returns Status code and description

	public static void Init()
	{
		//Accept all certificates
		ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
	}

	/// <summary>
	/// Requests the inventory.
	/// </summary>
	/// <param name="public_key">Public_key.</param>
	/// <param name="idfa_ios">Idfa_ios.</param>
	/// <param name="prize">Prize.</param>
	/// <param name="source">Source.</param>
	/// <param name="age_13_or_over">Age_13_or_over.</param>
	/// <param name="country_code">Country_code.</param>
	/// <param name="timestamp">Timestamp.</param>
	/// <param name="sig_token">Sig_token.</param>
	/// <param name="fsession_id">Fsession_id.</param>
	public static void RequestInventory( string public_key, string idfa, string prize, string source, string age_13_or_over,
	                                    string country_code, string timestamp, string sig_token, string fsession_id, UnlockableUserAgent userAgent )
	{
		string reqString = "public_key=" + public_key;
		reqString += "&opt_out_tracking=" + UnlockableAdTracking.AdTrackingEnabled().ToString();
		reqString += ( userAgent == UnlockableUserAgent.IOS ) ? "&idfa_ios=" : "&idfa_android=" + idfa;
		reqString += "&prize=" + prize;
		reqString += "&source=" + source;
		reqString += "&age_13_or_over=" + age_13_or_over;
		reqString += "&country_code=" + country_code;
		reqString += "&timestamp=" + timestamp;
		reqString += "&sig_token=" + sig_token;
		reqString += "&fsession_id=" + fsession_id;

		byte[] reqData 	= Encoding.UTF8.GetBytes (reqString);

		HttpWebRequest req 	= WebRequest.Create( ENDPOINT_URL ) as HttpWebRequest;
		req.Credentials 		= CredentialCache.DefaultCredentials;
		req.Method 					= "POST";
		req.ContentLength 	= reqData.Length;
		req.ContentType 		= "application/x-www-form-urlencoded";
		req.UserAgent 			= UnlockableUtils.GetDescription( userAgent );

		using(Stream requestStream = req.GetRequestStream() )
			requestStream.Write (reqData, 0, reqData.Length);

		string result = "";
		try
		{
			using(HttpWebResponse resp = req.GetResponse() as HttpWebResponse)
			{
				StreamReader reader = new StreamReader(resp.GetResponseStream());
				result = reader.ReadToEnd();

				if( onResult != null )
					onResult( result );
			}
		}
		catch(WebException ex)
		{
			if(ex.Response == null || ex.Status != WebExceptionStatus.ProtocolError)
				throw;

			if( onError != null )
			{
				string resp = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
				JSONNode response = JSON.Parse( resp )  ;

				onError( response["status_code"], response["status_msg"] );
			}
		}
	}

	/// <summary>
	/// Get's the hash string.
	/// </summary>
	/// <returns>The hash string.</returns>
	/// <param name="secret_key">Secret_key.</param>
	/// <param name="fsession">Fsession.</param>
	/// <param name="timeStamp">Time stamp.</param>
	public static string GetHashString( string secret_key, string fsession, string timeStamp )
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
	public static bool CheckHash(string secret_key, string fsession, string timeStamp, string hashString )
	{
		string originalHash = GetHashString( secret_key, fsession,  timeStamp );
		return (originalHash == hashString);
	}
}
