﻿using System;
using System.Collections.Generic;
using System.Text;

namespace OpenCommentViewer.CustomControl
{
  /// <summary>
  /// XMLDocumentの拡張クラス
  /// Cookie情報込みのLoad
  /// </summary>
	class ExXMLDocument : System.Xml.XmlDocument
	{

		public virtual void Load(string url, System.Net.CookieContainer cookies)
		{
			if (url == null || !url.StartsWith("http://")) {
				throw new ArgumentException("urlが正しく設定されていません", "url");
			}
			
			string xml = Utility.GetResponseText(url, cookies, ApplicationSettings.Default.DefaultApiTimeout);
			if (xml == null) {
				throw new System.Xml.XmlException("WEB上からXMLを取得できませんでした。");
			}

			this.LoadXml(xml);

		}

	}
}