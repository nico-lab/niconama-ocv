using System;
using System.Collections.Generic;
using System.Text;

namespace Hal.NicoApiSharp
{
  /// <summary>
  /// XMLDocumentの拡張クラス
  /// Cookie情報込みのLoadやXpathの拡張
  /// </summary>
	class ExXMLDocument : System.Xml.XmlDocument
	{

		public virtual void Load(string url, System.Net.CookieContainer cookies)
		{
			if (url == null || !url.StartsWith("http://")) {
				throw new ArgumentException("urlが正しく設定されていません", "url");
			}
			try {
				string xml = Utility.GetResponseText(url, cookies, ApiSettings.Default.DefaultApiTimeout);
				if (xml == null) {
					throw new System.Xml.XmlException("WEB上からXMLを取得できませんでした。");
				}
				this.LoadXml(xml);
			} catch (Exception ex) {
				Logger.Default.LogException(ex);
			}

			

		}

	}
}
