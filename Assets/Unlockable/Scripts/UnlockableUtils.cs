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
}
