using System;
using System.Collections.Generic;
using System.Text;

namespace Hal.NicoApiSharp.Cookie
{
	class SafariCookieGetter : ICookieGetter
	{
		#region ICookieGetter ÉÅÉìÉo

		public System.Net.Cookie[] GetCookies(Uri url, string key)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public System.Net.Cookie[] GetCookies(Uri url, string key, string path)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		#endregion
	}
}
