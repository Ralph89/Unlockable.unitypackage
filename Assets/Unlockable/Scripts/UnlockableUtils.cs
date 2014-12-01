using System;
using System.ComponentModel;
using System.Reflection;

public enum UnlockableUserAgent
{
	[Description("UniWebView; iOS")]
	IOS,
	[Description("UniWebView; Android")]
	ANDROID
}

public class UnlockableUtils
{
	public static string GetDescription( object enumValue )
	{
		FieldInfo fi = enumValue.GetType().GetField(enumValue.ToString());

		if (null != fi)
		{
			object[] attrs = fi.GetCustomAttributes(typeof(DescriptionAttribute), true);
			if (attrs != null && attrs.Length > 0)
				return ((DescriptionAttribute)attrs[0]).Description;
		}

		return "USERAGENT-UNDEFINED";
	}

	/// <summary>
	/// Checks for internet connection.
	/// Courtesy of http://stackoverflow.com/questions/2031824/what-is-the-best-way-to-check-for-internet-connectivity-using-net
	/// </summary>
	/// <returns><c>true</c>, if for internet connection was checked, <c>false</c> otherwise.</returns>
	public static bool CheckForInternetConnection()
	{
		try
		{
			using (var client = new System.Net.WebClient())
				using (var stream = client.OpenRead("http://www.google.com"))
					return true;
		}
		catch
		{
			return false;
		}
	}
}
