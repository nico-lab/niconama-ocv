using System;
using System.Collections.Generic;
using System.Text;

using Hal.NCSPlugin;

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

		List<INgClient> _clients = new List<INgClient>();
		//StringBuilder _normalFilterPattern = null;
		//StringBuilder _unityFilterPattern = null;
		//Regex _normalFilter = null;
		//Regex _unifyFilter = null;
		List<Regex> _regs = new List<Regex>();
		System.Collections.Specialized.StringCollection _idCollections = null;
		System.Collections.Specialized.StringCollection _commandCollections = null;

		/// <summary>
		/// 初期化
		/// </summary>
		/// <param name="core"></param>
		public void Initialize(ICore core)
		{
			this.ClearFilter();

			Hal.NCSPlugin.INgClient[] clients = core.GetNgClients();
			if (clients != null && clients.Length != 0) {
				
				BuildRegex(clients);

			}
		}

		/// <summary>
		/// フィルターを初期化する
		/// </summary>
		private void ClearFilter()
		{
			//_normalFilterPattern = new StringBuilder();
			//_unityFilterPattern = new StringBuilder();
			//_normalFilter = null;
			//_unifyFilter = null;
			_regs.Clear();
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

					foreach (Regex reg in _regs) {
						string com = chat.Message.ToUpper();
						Match m = reg.Match(com);

						if (m.Success) {
							chat.NgType = NGType.Word;
							chat.NgSource = m.Value;
							return;
						}
					}

					if (_idCollections.Count != 0 && !string.IsNullOrEmpty(chat.UserId)) {
						if (_idCollections.Contains(chat.UserId)) {
							chat.NgType = NGType.Id;
							chat.NgSource = chat.UserId;
							return;
						}
					}

					if (_commandCollections.Count != 0 && !string.IsNullOrEmpty(chat.Mail)) {
						if (!string.IsNullOrEmpty(chat.Mail) && chat.Mail != "184") {
							foreach (string cc in chat.Mail.Split(' ')) {
								if (_commandCollections.Contains(cc)) {
									chat.NgType = NGType.Command;
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
		private void AddNg(INgClient client)
		{
			_clients.Add(client);

			switch (client.Type) {
				case NGType.Word:

					// NGの属性に沿って該当する正規表現を生成する
					if (client.UseCaseUnify) {
						_regs.Add(new Regex(MakeUnifyPattern(client.Source.ToUpper())));
					} else if (client.IsRegex) {
						_regs.Add(new Regex(string.Format("({0})", client.Source)));
					} else {
						_regs.Add(new Regex(Regex.Escape(client.Source.ToUpper())));
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
		private void DelNg(INgClient ng)
		{
			if (_clients.Contains(ng)) {
				_clients.Remove(ng);
			}

			BuildRegex(_clients.ToArray());

		}

		/// <summary>
		/// NGフィルターを構築する
		/// </summary>
		private void BuildRegex(Hal.NCSPlugin.INgClient[] clients)
		{
			ClearFilter();
			_clients.AddRange(clients);
			foreach (INgClient client in _clients) {
				switch (client.Type) {
					case NGType.Word:

						// NGの属性に沿って該当する正規表現を生成する
						if (client.UseCaseUnify) {
							_regs.Add(new Regex(MakeUnifyPattern(client.Source.ToUpper())));
						} else if (client.IsRegex) {
							_regs.Add(new Regex(string.Format("({0})", client.Source)));
						} else {
							_regs.Add(new Regex(Regex.Escape(client.Source.ToUpper())));
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
					string n = Utility.ToHankaku(k);
					if (n.Length == 1) {
						sb.AppendFormat("[{0}{1}{2}]\\s*", h, k, n);
					} else {
						sb.AppendFormat("([{0}{1}]|{2})\\s", h, k, n);
					}

				} else if (Utility.IsZenkakuCase(c)) {
					// ひらがなカタカナ以外の全角文字（数字や全角英字）から半角文字でも引っかかるパターンを生成する
					string n = Utility.ToHankaku(c).ToUpper();
					sb.AppendFormat("[{0}{1}]\\s", c, n);
				} else if (Utility.IsHankakuCase(c)) {
					// 半角文字から全角文字でも引っかかるパターンを生成する
					string n = Utility.ToZenkaku(c).ToString().ToUpper();
					sb.AppendFormat("[{0}{1}]\\s", c, n);
				} else {
					sb.Append(c + "\\s");
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
