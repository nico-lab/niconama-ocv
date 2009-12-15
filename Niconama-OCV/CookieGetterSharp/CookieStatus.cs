using System;
using System.Collections.Generic;
using System.Text;

namespace Hal.CookieGetterSharp
{
	class CookieStatus : ICookieStatus
	{
		readonly string _name;
		readonly BrowserType _browserType;
		readonly PathType _pathType;
		string _path;
		string _displayName;

		internal CookieStatus(string name, string path, BrowserType browserType, PathType pathType)
		{
			_name = name;
			_path = path;
			_browserType = browserType;
			_pathType = pathType;
			_displayName = null;
		}

		#region IBrowserStatus メンバ

		public BrowserType BrowserType
		{
			get { return _browserType; }
		}

		public bool IsAvailable
		{
			get {
				if (string.IsNullOrEmpty(this.CookiePath)) return false;

				if (_pathType == PathType.File) {
					return System.IO.File.Exists(this.CookiePath);
				} else {
					return System.IO.Directory.Exists(this.CookiePath);
				}
			}
		}

		public string Name
		{
			get { return _name; }
		}

		public string CookiePath
		{
			get { return _path; }
			set { _path = value; }
		}

		public PathType PathType {
			get { return _pathType; }
		}

		public string DisplayName {
			get {
				if (string.IsNullOrEmpty(_displayName)) {
					return this.Name;
				} else {
					return _displayName;
				}
			}
			set {
				_displayName = value;
			}
		}

		#endregion

		#region Objectのオーバーライド

		public override string ToString()
		{
			return this.DisplayName;
		}

		public override bool Equals(object obj)
		{
			if (this.Name == null || this.CookiePath == null || obj == null || !(obj is CookieStatus)) {
				return false;
			}
			CookieStatus bi = (CookieStatus)obj;

			return this.Name.Equals(bi.Name) && this.CookiePath.Equals(bi.CookiePath);
		}

		public override int GetHashCode()
		{
			string x = this.Name + this.CookiePath;
			return x.GetHashCode();
		}

		#endregion


		#region IBrowserStatus メンバ

		

		#endregion
	}
}
