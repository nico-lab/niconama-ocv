using System;
using System.Collections.Generic;
using System.Text;

namespace Hal.CookieGetterSharp
{
	class Utility
	{
		/// <summary>
		/// UnixŠÔ‚ğDateTime‚É•ÏŠ·‚·‚é
		/// </summary>
		/// <param name="UnixTime"></param>
		/// <returns></returns>
		public static DateTime UnixTimeToDateTime(int UnixTime)
		{
			return new DateTime(1970, 1, 1, 9, 0, 0).AddSeconds(UnixTime);
		}

		/// <summary>
		/// DateTime‚ğUnixŠÔ‚É•ÏŠ·‚·‚é
		/// </summary>
		/// <param name="time"></param>
		/// <returns></returns>
		public static int DateTimeToUnixTime(DateTime time)
		{
			TimeSpan t = time.Subtract(new DateTime(1970, 1, 1, 9, 0, 0));
			return (int)t.TotalSeconds;
		}

		/// <summary>
		/// %APPDATA%‚È‚Ç‚ğÀÛ‚ÌƒpƒX‚É•ÏŠ·‚·‚é
		/// </summary>
		/// <param name="path"></param>
		/// <returns></returns>
		public static string ReplacePathSymbols(string path)
		{
			path = path.Replace("%APPDATA%", Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData));
			path = path.Replace("%LOCALAPPDATA%", Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData));
			path = path.Replace("%COOKIES%", Environment.GetFolderPath(Environment.SpecialFolder.Cookies));
			return path;
		}

		
	}
}
