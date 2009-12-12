using System;
using System.Collections.Generic;
using System.Text;

namespace Hal.CookieGetterSharp
{
	class Safari4CookieGetter : CookieGetter
	{

		public Safari4CookieGetter(ICookieStatus status) : base(status)
		{
		}

		public override System.Net.CookieContainer GetAllCookies()
		{

			System.Net.CookieContainer container = new System.Net.CookieContainer();

			if (!System.IO.File.Exists(base.CookiePath)) return container;

			try {
				System.Xml.XmlReaderSettings settings = new System.Xml.XmlReaderSettings();

				// DTDを取得するためにウェブアクセスするのを抑制する
				// (通信遅延や、アクセスエラーを排除するため)
				settings.XmlResolver = null;
				settings.ProhibitDtd = false;
				settings.CheckCharacters = false;

				using (System.Xml.XmlReader xtr = System.Xml.XmlTextReader.Create(base.CookiePath, settings)) {
					while (xtr.Read()) {
						switch (xtr.NodeType) {
							case System.Xml.XmlNodeType.Element:
								if (xtr.Name.ToLower().Equals("dict")) {
									System.Net.Cookie cookie = getCookie(xtr);
									try {
										Utility.AddCookieToContainer(container, cookie);
									} catch {
										Console.WriteLine(string.Format("Invalid Format! domain:{0},key:{1},value:{2}", cookie.Domain, cookie.Name, cookie.Value));
									}
								}
								break;
						}
					}
				}

			} catch (Exception ex) {
				throw new CookieGetterException("Safariのクッキー取得中にエラーが発生しました。", ex);
			}

			return container;
		}

		private System.Net.Cookie getCookie(System.Xml.XmlReader xtr)
		{
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
										cookie.Name = xtr.Value;
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
