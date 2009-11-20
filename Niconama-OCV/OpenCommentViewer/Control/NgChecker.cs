using System;
using System.Collections.Generic;
using System.Text;

using Hal.NicoApiSharp.Live;

using Regex = System.Text.RegularExpressions.Regex;
using Match = System.Text.RegularExpressions.Match;


namespace Hal.OpenCommentViewer.Control
{

	/// <summary>
	/// NGチェックを担当するクラス
	/// 
	/// すべて正規表現で処理しているが、CommandとIDについてはStringCollectionあたりを使ってやったほうがいいかもしれない
	/// </summary>
	class NgChecker
	{

		List<NgClient> _clients = new List<NgClient>();
		StringBuilder _normalFilterPattern = null;
		StringBuilder _unityFilterPattern = null;
		Regex _normalFilter = null;
		Regex _unifyFilter = null;
		System.Collections.Specialized.StringCollection _idCollections = null;
		System.Collections.Specialized.StringCollection _commandCollections = null;

		/// <summary>
		/// 初期化
		/// </summary>
		/// <param name="core"></param>
		public void Initialize(ICore core)
		{
			this.ClearFilter();
			if (!string.IsNullOrEmpty(core.LiveId)) {
				NgClient[] clients = NgClient.GetNgClients(core.LiveId);
				if (clients != null && clients.Length != 0) {

					BuildRegex(clients);

				}
			}
		}

		/// <summary>
		/// フィルターを初期化する
		/// </summary>
		private void ClearFilter()
		{
			_normalFilterPattern = new StringBuilder();
			_unityFilterPattern = new StringBuilder();
			_normalFilter = null;
			_unifyFilter = null;
			_idCollections = new System.Collections.Specialized.StringCollection();
			_commandCollections = new System.Collections.Specialized.StringCollection();
			_clients.Clear();
		}

		/// <summary>
		/// ChatのNGチェックを行う
		/// 結果はchatオブジェクトのIFilterdChatインターフェースの実装に格納される
		/// </summary>
		/// <param name="chat"></param>
		public void Check(OcvChat chat)
		{

			if (chat.IsOwnerComment && chat.Message.StartsWith("/ng")) {

				OperateNgCommand(chat);

			} else if (!chat.IsOwnerComment) {
				lock (this) {

					if (_normalFilter != null) {
						Match m = _normalFilter.Match(chat.Message);

						if (m.Success) {
							chat.NgType = Hal.NCSPlugin.NGType.Word;
							chat.NgSource = m.Value;
							return;
						}
					}

					if (_unifyFilter != null) {
						string com = normaliza(chat.Message);
						Match m = _unifyFilter.Match(com);
						if (m.Success) {
							chat.NgType = Hal.NCSPlugin.NGType.Word;
							chat.NgSource = m.Value;
							return;
						}
					}

					if (_idCollections.Count != 0 && !string.IsNullOrEmpty(chat.UserId)) {
						if (_idCollections.Contains(chat.UserId)) {
							chat.NgType = Hal.NCSPlugin.NGType.Id;
							chat.NgSource = chat.UserId;
							return;
						}
					}

					if (_commandCollections.Count != 0 && !string.IsNullOrEmpty(chat.Mail)) {
						if (!string.IsNullOrEmpty(chat.Mail) && chat.Mail != "184") {
							foreach (string cc in chat.Mail.Split(' ')) {
								if (_commandCollections.Contains(cc)) {
									chat.NgType = Hal.NCSPlugin.NGType.Command;
									chat.NgSource = cc;
									return;
								}
							}

						}
					}
				}
			}

		}

		public void Check(ICollection<OcvChat> chats)
		{
			foreach (OcvChat chat in chats) {
				this.Check(chat);
			}
		}

		/// <summary>
		/// 主米によるNG操作を実行する
		/// </summary>
		/// <param name="chat"></param>
		private void OperateNgCommand(IChat chat)
		{

			Match mat = Regex.Match(chat.Message, "^/ng(?<op>add|del) (?<type>[\\S]+) \"(?<src>[^\"]+)\"");

			if (mat.Success) {
				string types = mat.Groups["type"].Value;
				string src = mat.Groups["src"].Value;

				NGType type = (types.Equals("ID") ? NGType.Id : types.Equals("COMMAND") ? NGType.Command : NGType.Word);

				if (mat.Groups["op"].Value.Equals("add")) {
					AddNg(type, src);
				} else {
					DelNg(type, src);
				}

			}

		}

		/// <summary>
		/// NGを追加する
		/// </summary>
		/// <param name="type"></param>
		/// <param name="src"></param>
		private void AddNg(NGType type, string src)
		{

			AddNg(new NicoApiSharp.Live.NgClient(type, src, DateTime.Now));

		}

		/// <summary>
		/// NGを追加する
		/// </summary>
		/// <param name="ng"></param>
		private void AddNg(Hal.NicoApiSharp.Live.NgClient client)
		{
			_clients.Add(client);

			switch (client.Type) {
				case NGType.Word:

					// NGの属性に沿って該当する正規表現を生成する
					if (client.UseCaseUnify) {
						AddPattern(_unityFilterPattern, normaliza(client.Source));
						_unifyFilter = new Regex(_unityFilterPattern.ToString());
					} else{
						if (client.IsRegex) {
							AddPattern(_normalFilterPattern, string.Format("({0})", client.Source));
						} else {
							AddPattern(_normalFilterPattern, Regex.Escape(client.Source));
						}
						_normalFilter = new Regex(_normalFilterPattern.ToString());
					}

					break;
				case NGType.Id:
					_idCollections.Add(client.Source);
					break;
				case NGType.Command:
					_commandCollections.Add(client.Source);
					break;
			}

		}

		/// <summary>
		/// NGを削除する
		/// </summary>
		/// <param name="type"></param>
		/// <param name="src"></param>
		private void DelNg(NGType type, string src)
		{
			DelNg(new NicoApiSharp.Live.NgClient(type, src, DateTime.Now));
		}

		/// <summary>
		/// NGを削除する
		/// </summary>
		/// <param name="ng"></param>
		private void DelNg(Hal.NicoApiSharp.Live.NgClient ng)
		{
			if (_clients.Contains(ng)) {
				_clients.Remove(ng);
			}

			BuildRegex(_clients.ToArray());

		}

		/// <summary>
		/// NGフィルターを構築する
		/// </summary>
		private void BuildRegex(Hal.NicoApiSharp.Live.NgClient[] clients)
		{
			ClearFilter();
			_clients.AddRange(clients);
			foreach (NgClient client in _clients) {
				switch (client.Type) {
					case NGType.Word:

						// NGの属性に沿って該当する正規表現を生成する
						if (client.UseCaseUnify) {
							AddPattern(_unityFilterPattern, normaliza(client.Source));
						} else if (client.IsRegex) {
							AddPattern(_normalFilterPattern, string.Format("({0})", client.Source));
						} else {
							AddPattern(_normalFilterPattern, Regex.Escape(client.Source));
						}

						break;
					case NGType.Id:
						_idCollections.Add(client.Source);
						break;
					case NGType.Command:
						_commandCollections.Add(client.Source);
						break;
				}

			}

			if (_normalFilterPattern.Length != 0) {
				_normalFilter = new Regex(_normalFilterPattern.ToString(), System.Text.RegularExpressions.RegexOptions.Compiled);
			} else {
				_normalFilter = null;
			}

			if (_unityFilterPattern.Length != 0) {
				_unifyFilter = new Regex(_unityFilterPattern.ToString(), System.Text.RegularExpressions.RegexOptions.Compiled);
			} else {
				_unifyFilter = null;
			}

		}

		/// <summary>
		/// ストリングビルダーに正規表現パターンを追加する
		/// </summary>
		/// <param name="sb"></param>
		/// <param name="pattern"></param>
		private void AddPattern(StringBuilder sb, string pattern)
		{
			if (sb.Length != 0) {
				sb.Append('|');
			}

			sb.Append(pattern);
		}

		private string normaliza(string text)
		{
			text = System.Text.RegularExpressions.Regex.Replace(text, @"[ 　\t\r\n]", "").ToUpper();
			text = text.Normalize(NormalizationForm.FormKC);
			text = Regex.Escape(text);
			StringBuilder sb = new StringBuilder(text);
			for (int i = 0; i < sb.Length; i++) {
				if (Utility.IsZenkakuJapanese(sb[i])) {
					sb[i] = Utility.ToKatakana(sb[i]);
				}
			}
			return sb.ToString();
		}


		#region IDisposable メンバ

		public void Dispose()
		{
			this.ClearFilter();
			_clients = null;
		}

		#endregion
	}
}
