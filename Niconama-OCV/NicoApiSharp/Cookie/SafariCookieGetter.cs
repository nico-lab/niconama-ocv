using System;
using System.Collections.Generic;
using System.Text;

namespace Hal.NicoApiSharp.Cookie
{
	class SafariCookieGetter : ICookieGetter
	{
		#region ICookieGetter メンバ

		public System.Net.Cookie[] GetCookies(Uri url, string key)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public System.Net.Cookie[] GetCookies(Uri url, string key, string path)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		#endregion

		#region ICookieGetter メンバ


		public System.Net.CookieCollection[] GetCookieCollection(Uri url)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public System.Net.CookieCollection[] GetCookieCollection(Uri url, string path)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		#endregion
	}
}
