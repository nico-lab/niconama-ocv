using System;
using System.Collections.Generic;
using System.Text;

using NCSPlugin;

using Regex = System.Text.RegularExpressions.Regex;
using Match = System.Text.RegularExpressions.Match;


namespace OpenCommentViewer.Control
{

	/// <summary>
	/// NGチェックを担当するクラス
	/// 
	/// すべて正規表現で処理しているが、CommandとIDについてはStringCollectionあたりを使ってやったほうがいいかもしれない
	/// </summary>
	class NgChecker
	{

		List<INgClient> _clients = new List<INgClient>();
		Regex _normalFilter = null;
		Regex _unifyFilter = null;
		Regex _idFilter = null;
		Regex _commandFilter = null;
		
		/// <summary>
		/// 初期化
		/// </summary>
		/// <param name="core"></param>
		public void Initialize(ICore core)
		{
			this.ClearFilter();

			NCSPlugin.INgClient[] clients = core.GetNgClients();
			if (clients != null && clients.Length != 0) {
				_clients.AddRange(clients);
				BuildRegex();

			}
		}

		/// <summary>
		/// フィルターを初期化する
		/// </summary>
		private void ClearFilter() {
			_normalFilter = null;
			_unifyFilter = null;
			_idFilter = null;
			_commandFilter = null;
			_clients.Clear();
		}

		/// <summary>
		/// ChatのNGチェックを行う
		/// 結果はchatオブジェクトのIFilterdChatインターフェースの実装に格納される
		/// </summary>
		/// <param name="chat"></param>
		public void Check(NicoAPI.Chat chat)
		{
			
			if (chat.IsOwnerComment && chat.Message.StartsWith("/ng")) {
				
				OperateNgCommand(chat);

			} else if (!chat.IsOwnerComment) {
				lock (this) {

					if (_normalFilter != null) {
						string com = chat.Message.ToUpper();
						Match m = _normalFilter.Match(com);

						if (m.Success) {
							chat.NgType = NGType.Word;
							chat.NgSource = m.Value;
							return;
						}
					}

					if (_unifyFilter != null) {
						string com = System.Text.RegularExpressions.Regex.Replace(chat.Message, @"[ 　\t\r\n]", "").ToUpper(); 
						Match m = _unifyFilter.Match(com);
						if (m.Success) {
							chat.NgType = NGType.Word;
							chat.NgSource = m.Value;
							return;
						}
					}

					if (_idFilter != null && !string.IsNullOrEmpty(chat.UserId)) {
						Match m = _idFilter.Match(chat.UserId);
						if (m.Success) {
							chat.NgType = NGType.Id;
							chat.NgSource = m.Value;
							return;
						}
					}

					if (_commandFilter != null && !string.IsNullOrEmpty(chat.Mail)) {
						Match m = _commandFilter.Match(chat.Mail);
						if (m.Success) {
							chat.NgType = NGType.Command;
							chat.NgSource = m.Value;
							return;
						}
					}
				}
			}

		}

		public void Check(ICollection<NicoAPI.Chat> chats) {
			foreach (NicoAPI.Chat chat in chats) {
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
				BuildRegex();
			}

		}

		/// <summary>
		/// NGを追加する
		/// </summary>
		/// <param name="type"></param>
		/// <param name="src"></param>
		private void AddNg(NGType type, string src)
		{

			AddNg(new NicoAPI.NgClient(type, src, DateTime.Now));

		}

		/// <summary>
		/// NGを追加する
		/// </summary>
		/// <param name="ng"></param>
		private void AddNg(INgClient ng)
		{
			_clients.Add(ng);
			
		}

		/// <summary>
		/// NGを削除する
		/// </summary>
		/// <param name="type"></param>
		/// <param name="src"></param>
		private void DelNg(NGType type, string src)
		{
			DelNg(new NicoAPI.NgClient(type, src, DateTime.Now));
		}

		/// <summary>
		/// NGを削除する
		/// </summary>
		/// <param name="ng"></param>
		private void DelNg(INgClient ng)
		{
			if (_clients.Contains(ng)) {
				_clients.Remove(ng);
			}

		}

		/// <summary>
		/// NGフィルターを構築する
		/// </summary>
		private void BuildRegex()
		{
			StringBuilder wordPattern = new StringBuilder();
			StringBuilder unifyPattern = new StringBuilder();
			StringBuilder idPattern = new StringBuilder();
			StringBuilder commandPattern = new StringBuilder();

			foreach (INgClient client in _clients) {
				switch (client.Type) {
					case NGType.Word:

						// NGの属性に沿って該当する正規表現を生成する
						if (client.UseCaseUnify) {
							AddPattern(unifyPattern, MakeUnifyPattern(client.Source.ToUpper()));
						} else if (client.IsRegex) {
							AddPattern(wordPattern, string.Format("({0})", client.Source));
						} else {
							AddPattern(wordPattern, Regex.Escape(client.Source.ToUpper()));
						}

						break;
					case NGType.Id:
						AddPattern(idPattern, string.Format("^{0}$", Regex.Escape(client.Source)));
						break;
					case NGType.Command:
						AddPattern(commandPattern, Regex.Escape(client.Source));
						break;
				}

			}

			if (wordPattern.Length != 0) {
				_normalFilter = new Regex(wordPattern.ToString(), System.Text.RegularExpressions.RegexOptions.Compiled);
			} else {
				_normalFilter = null;
			}

			if (unifyPattern.Length != 0) {
				_unifyFilter = new Regex(unifyPattern.ToString(), System.Text.RegularExpressions.RegexOptions.Compiled);
			} else {
				_unifyFilter = null;
			}

			if (idPattern.Length != 0) {
				_idFilter = new Regex(idPattern.ToString(), System.Text.RegularExpressions.RegexOptions.Compiled);
			} else {
				_idFilter = null;
			}

			if (commandPattern.Length != 0) {
				_commandFilter = new Regex(commandPattern.ToString(), System.Text.RegularExpressions.RegexOptions.Compiled);
			} else {
				_commandFilter = null;
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

		/// <summary>
		/// UseCaseUnity属性が付いたNGの正規表現パターンを生成する
		/// </summary>
		/// <param name="str"></param>
		/// <returns></returns>
		private string MakeUnifyPattern(string str)
		{
			StringBuilder sb = new StringBuilder();
			str = str.Replace(" ", "");
			str = System.Text.RegularExpressions.Regex.Escape(str);

			for (int i = 0; i < str.Length; i++) {

				char c = str[i];
				if (Utility.IsZenkakuJapanese(c)) { 
					// ひらがな、カタカナ、半角カタカナのどれでも引っかかるパターンを作る
					// ただし、濁点がある場合はパフォーマンスを優先して一部仕様外の動作を許容している
					// (（）を使ってグループを作るとパフォーマンスが悪化する気がしたので)
					char h = Utility.ToHiragana(c);
					char k = Utility.ToKatakana(c);
					string n = Utility.ToHankaku(k.ToString());
					if (n.Length == 1) {
						sb.AppendFormat("[{0}{1}{2}]", h, k, n);
					} else {
						sb.AppendFormat("[{0}{1}{2}]{3}?", h, k, n[0], n[1]);
					}

				} else if (Utility.IsZenkakuCase(c)) {
					// ひらがなカタカナ以外の全角文字（数字や全角英字）から半角文字でも引っかかるパターンを生成する
					string n = Utility.ToHankaku(c.ToString()).ToUpper();
					sb.AppendFormat("[{0}{1}]", c, n);
				} else if (Utility.IsHankakuCase(c)) {
					// 半角文字から全角文字でも引っかかるパターンを生成する
					string n = Utility.ToZenkaku(c).ToString().ToUpper();
					sb.AppendFormat("[{0}{1}]", c, n);
				} else {
					sb.Append(c);
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
