using System;
using System.Collections.Generic;
using System.Text;

namespace Hal.CookieGetterSharp
{

	/// <summary>
	/// GoogleChromeからクッキーを取得する
	/// </summary>
	class GoogleChrome3CookieGetter : SqlCookieGetter
	{

		const string SELECT_QUERY = "SELECT value, name, host_key, path, expires_utc FROM cookies";

		public GoogleChrome3CookieGetter(ICookieStatus status) : base(status)
		{
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
			} catch (Exception ex) {
				throw new CookieGetterException("googlechromeのexpires変換に失敗しました", ex);
			}

			return cookie;
		}

		private string makeWhere(Uri url)
		{
			Stack<string> hostStack = new Stack<string>(url.Host.Split('.'));
			StringBuilder hostBuilder = new StringBuilder('.' + hostStack.Pop());
			string[] pathes = url.Segments;

			StringBuilder sb = new StringBuilder();
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

		protected override string MakeQuery()
		{
			return SELECT_QUERY + " ORDER BY creation_utc DESC";
		}

		protected override string MakeQuery(Uri url)
		{
			return string.Format("{0} {1} ORDER BY creation_utc DESC", SELECT_QUERY, makeWhere(url));
		}

		protected override string MakeQuery(Uri url, string key)
		{
			return string.Format("{0} {1} AND name = \"{2}\" ORDER BY creation_utc DESC", SELECT_QUERY, makeWhere(url), key);
		}
	}
}
