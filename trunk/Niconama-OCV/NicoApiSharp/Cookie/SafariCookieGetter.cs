using System;
using System.Collections.Generic;
using System.Text;

namespace Hal.NicoApiSharp.Cookie
{
	class SafariCookieGetter : CookieGetter
	{
		public static new ICookieGetter GetInstance(Cookie.CookieGetter.BROWSER_TYPE type)
		{
			switch (type) {
				case CookieGetter.BROWSER_TYPE.Safari4:
					return new SafariCookieGetter();
			}

			return null;
		}

		private SafariCookieGetter()
		{ 
			base._defaultPath = Utility.ReplacePathSymbols(ApiSettings.Default.Opera10CookieFilePath);
		}

		public override System.Net.CookieContainer GetAllCookies(string path)
		{

			System.Net.CookieContainer container = new System.Net.CookieContainer();
			
			if (!System.IO.File.Exists(path)) {
				Logger.Default.LogErrorMessage("クッキー取得：存在しないパス - " + path);
				return container;
			}

			try {
				using (System.Xml.XmlTextReader xtr = new System.Xml.XmlTextReader(path)) {
					while (xtr.Read()) {
						switch (xtr.NodeType) { 
							case System.Xml.XmlNodeType.Element:
								if (xtr.Name.ToLower().Equals("dict")) {
									System.Net.Cookie cookie = getCookie(xtr);
									container.Add(cookie);
								}
								break;
						}
					}
				}

			} catch (Exception ex){
				Logger.Default.LogException(ex);
			}

			return container;
		}

		private System.Net.Cookie getCookie(System.Xml.XmlTextReader xtr) {
			bool isEnd = false;
			System.Net.Cookie cookie = new System.Net.Cookie();
			string tagName = "";
			string kind = "";

			while (xtr.Read() && !isEnd) {
				switch (xtr.NodeType) { 
					case System.Xml.XmlNodeType.Element:
						tagName = xtr.Name.ToLower();
						break;

					case System.Xml.XmlNodeType.Text:
						switch (tagName) { 
							case "key":
								kind = xtr.Value.ToLower();
								break;
							case "real":
							case "string":
							case "date":
								switch (kind) { 
									case "domain":
										cookie.Domain = xtr.Value;
										break;
									case "name":
										cookie.Name = xtr.Name;
										break;
									case "value":
										cookie.Value = xtr.Value;
										break;
									case "expires":
										cookie.Expires = DateTime.Parse(xtr.Value);
										break;
									case "path":
										cookie.Path = xtr.Value;
										break;
								}
								break;
						}
						break;

					case System.Xml.XmlNodeType.EndElement:
						if (xtr.Name.ToLower() == "dict") {
							isEnd = true;
						}
						break;

				}
			}

			return cookie;
		}
	}
}
