using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Hal.NicoApiSharp.Live
{
	public class NgClient : INgClient
	{

		#region Static Methods

		/// <summary>
		/// NGを追加する
		/// </summary>
		/// <param name="liveId"></param>
		/// <param name="type"></param>
		/// <param name="source"></param>
		/// <param name="cookies"></param>
		/// <returns></returns>
		public static bool AddNg(string liveId, NGType type, string source, System.Net.CookieContainer cookies)
		{
			string res = SendNgCommand(liveId, "add", type.ToString(), source, cookies);
			return (res != null && res.Contains("status=\"ok\""));
		}

		/// <summary>
		/// NGを削除する
		/// </summary>
		/// <param name="liveId"></param>
		/// <param name="type"></param>
		/// <param name="source"></param>
		/// <param name="cookies"></param>
		/// <returns></returns>
		public static bool DeleteNg(string liveId, NGType type, string source, System.Net.CookieContainer cookies)
		{
			string res = SendNgCommand(liveId, "del", type.ToString(), source, cookies);
			return (res != null && res.Contains("status=\"ok\""));
		}

		/// <summary>
		/// NG一覧を取得する。失敗したときはnullを返す
		/// </summary>
		/// <param name="liveId"></param>
		/// <param name="cookies"></param>
		/// <returns>NG一覧　失敗時はnull</returns>
		public static INgClient[] GetNgClients(string liveId, System.Net.CookieContainer cookies)
		{
			string res = SendNgCommand(liveId, "get", "", "", cookies);

			if (res == null) {
				Logger.Default.LogMessage("NG一覧を取得できませんでした。");
				return null;
			}

			XmlDocument xdoc = new XmlDocument();

			try {
				xdoc.LoadXml(res);
			} catch (Exception) {
				Logger.Default.LogMessage("XMLの解析に失敗しました。");
				return null;
			}

			XmlNode root = xdoc["response_ngword"];

			if (root == null || root.Attributes["status"].Value != "ok") {
				Logger.Default.LogMessage("サーバーの問題によりNG一覧を取得できませんでした。");
				return null;
			}

			int count = 0;
			int.TryParse(xdoc.SelectSingleNode("response_ngword/count").InnerText, out count);
			List<NgClient> clients = new List<NgClient>(count);
			foreach (XmlNode node in xdoc.SelectNodes("response_ngword/ngclient")) {
				clients.Add(new NgClient(node));
			}

			return clients.ToArray();

		}

		private static string SendNgCommand(string liveId, string mode, string type, string source, System.Net.CookieContainer cookies)
		{

			source = System.Web.HttpUtility.UrlEncode(source);

			string url = string.Format(ApiSettings.Default.NgcommandUrlFormat, liveId, mode, source, type);
			return Utility.GetResponseText(url, cookies, ApiSettings.Default.DefaultApiTimeout);
		}


		#endregion

		[Flags]
		enum NGOption
		{
			None = 0,
			Readonly = 1,
			UseCaseUnify = 2,
			IsRegex = 4
		}

		NGType _type;
		string _source;
		DateTime _regTime;
		NGOption _options;

		private NgClient(XmlNode node)
		{
			this.Parse(node);
		}

		public NgClient(NGType type, string source, DateTime regTime)
		{
			_type = type;
			_source = source;
			_regTime = regTime;
		}

		private void Parse(XmlNode node)
		{
			_source = Utility.Unsanitizing(node["source"].InnerText);
			_regTime = Utility.UnixTimeToDateTime(int.Parse(node["register_time"].InnerText));
			switch (node["type"].InnerText) {
				case "word":
					_type = NGType.Word;
					break;
				case "id":
					_type = NGType.Id;
					break;
				case "command":
					_type = NGType.Command;
					break;
			}

			if (node.Attributes["readonly"] != null && node.Attributes["readonly"].Value == "true") {
				_options |= NGOption.Readonly;
			}

			if (node.Attributes["use_case_unify"] != null && node.Attributes["use_case_unify"].Value == "true") {
				_options |= NGOption.UseCaseUnify;
			}

			if (node.Attributes["is_regex"] != null && node.Attributes["is_regex"].Value == "true") {
				_options |= NGOption.IsRegex;
			}


		}

		#region INgClients メンバ

		public NGType Type
		{
			get { return _type; }
		}

		public string Source
		{
			get { return _source; }
		}

		public DateTime RegisterTime
		{
			get { return _regTime; }
		}

		public bool ReadOnly
		{
			get { return (_options & NGOption.Readonly) != 0; }
		}

		public bool UseCaseUnify
		{
			get { return (_options & NGOption.UseCaseUnify) != 0; }
		}

		public bool IsRegex
		{
			get { return (_options & NGOption.IsRegex) != 0; }
		}


		#endregion

		public override bool Equals(object obj)
		{
			if (obj == null) {
				return false;
			}

			NgClient that = obj as NgClient;
			if (that == null) {
				return false;
			}

			return this._type == that._type && this._source.Equals(that._source);
		}

		public override int GetHashCode()
		{
			return this._source.GetHashCode() ^ (int)this._type;
		}
	}
}
