using System;
using System.Collections.Generic;
using System.Text;

namespace Hal.NicoApiSharp.Cookie
{

	/// <summary>
	/// GoogleChrome3.0からクッキーを取得する
	/// </summary>
	class GoogleChromeCookieGetter : SqlCookieGetter
	{

		const string SELECT_QUERY = "SELECT value, name, host_key, path, expires_utc FROM cookies";

		public static new ICookieGetter GetInstance(Cookie.CookieGetter.BROWSER_TYPE type)
		{
			switch (type) {
				case CookieGetter.BROWSER_TYPE.Chrome3:
					return new GoogleChromeCookieGetter();
			}

			return null;
		}

		private GoogleChromeCookieGetter() {
		}

		
		protected override System.Net.Cookie DataToCookie(object[] data)
		{
			System.Net.Cookie cookie = new System.Net.Cookie();
			cookie.Value = data[0] as string;
			cookie.Name = data[1] as string;
			cookie.Domain = data[2] as string;
			cookie.Path = data[3] as string;

			try {
				long exp = (long)data[4];
				cookie.Expires = new DateTime(exp);
			} catch {
				Logger.Default.LogMessage("googlechromeのexpires変換に失敗しました");					
			}

			return cookie;
		}
		

		protected override string MakeQuery(Uri url) 
		{ 
			Stack<string> hostStack = new Stack<string>(url.Host.Split('.'));
			StringBuilder hostBuilder = new StringBuilder('.' + hostStack.Pop());
			string[] pathes = url.Segments;

			StringBuilder sb = new StringBuilder();
			sb.Append(SELECT_QUERY);
			sb.Append(" WHERE (");

			bool needOr = false;
			while (hostStack.Count != 0) {
				if (needOr) {
					sb.Append(" OR");
				}

				if (hostStack.Count != 1) {
					hostBuilder.Insert(0, '.' + hostStack.Pop());
					sb.AppendFormat(" host_key = \"{0}\"", hostBuilder.ToString());
				} else {
					hostBuilder.Insert(0, '%' + hostStack.Pop());
					sb.AppendFormat(" host_key LIKE \"{0}\"", hostBuilder.ToString());
				}

				needOr = true;
			}

			sb.Append(')');
			return sb.ToString();
		
		}

		protected override string MakeQuery(Uri url, string key)
		{
			string baseQuery = MakeQuery(url);
			return string.Format("{0} AND name = \"{1}\"", baseQuery, key);

		}
	}
}
