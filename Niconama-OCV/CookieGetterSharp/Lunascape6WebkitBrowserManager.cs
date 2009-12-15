using System;
using System.Collections.Generic;
using System.Text;

namespace Hal.CookieGetterSharp
{
	class Lunascape6WebkitBrowserManager : IBrowserManager
	{
		const string LUNASCAPE_PLUGIN_FOLDER = "%APPDATA%\\Lunascape\\Lunascape6\\plugins";
		const string COOKIEPATH = "data\\cookies.ini";

		#region IBrowserManager �����o

		public BrowserType BrowserType
		{
			get { return BrowserType.Lunascape6Webkit; }
		}

		public ICookieGetter CreateDefaultCookieGetter()
		{
			string path = SearchDirectory();

			CookieStatus status = new CookieStatus("Lunascape6 Webkit", path, this.BrowserType, PathType.File);
			return new WebkitCookieGetter(status);
		}

		public ICookieGetter[] CreateCookieGetters()
		{
			return new ICookieGetter[] { CreateDefaultCookieGetter() };
		}

		#endregion

		/// <summary>
		/// Lunascape6�̃v���O�C���t�H���_����Firefox�̃N�b�L�[���ۑ�����Ă���p�X����������
		/// </summary>
		/// <returns></returns>
		private string SearchDirectory()
		{
			string dir = Utility.ReplacePathSymbols(LUNASCAPE_PLUGIN_FOLDER);
			if (System.IO.Directory.Exists(dir)) {
				foreach (string folder in System.IO.Directory.GetDirectories(dir)) {
					string path = System.IO.Path.Combine(folder, COOKIEPATH);
					if (System.IO.File.Exists(path)) {
						return path;
					}
				}
			}
			return null;
		}

	}
}
