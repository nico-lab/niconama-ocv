using System;
using System.Collections.Generic;
using System.Text;

namespace Hal.CookieGetterSharp
{
	class BrowserStatus : IBrowserStatus
	{
		string _name;
		string _cookieFilePath;
		ICookieGetter _cookieGetter;

		internal BrowserStatus() {
		}

		#region IBrowserStatus メンバ

		public string Name
		{
			get { return _name; }
			internal set { _name = value; }
		}

		public string CookiePath
		{
			get { return _cookieFilePath; }
			internal set { _cookieFilePath = value; }
		}

		public ICookieGetter CookieGetter
		{
			get { return _cookieGetter; }
			internal set { _cookieGetter = value; }
		}

		#endregion

		#region Objectのオーバーライド

		public override string ToString()
		{
			return _name;
		}

		public override bool Equals(object obj)
		{
			if (this._name == null || this._cookieFilePath == null || obj == null || !(obj is BrowserStatus)) {
				return false;
			}
			BrowserStatus bi = (BrowserStatus)obj;

			return this._name.Equals(bi.Name) && this._cookieFilePath.Equals(bi.CookiePath);
		}

		public override int GetHashCode()
		{
			string x = this._name + this._cookieFilePath;
			return x.GetHashCode();
		}

		#endregion

	}
}
