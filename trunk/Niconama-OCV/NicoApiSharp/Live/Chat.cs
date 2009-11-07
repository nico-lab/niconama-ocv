using System;
using System.Collections.Generic;
using System.Text;

namespace Hal.NicoApiSharp.Live
{

	/// <summary>
	/// サーバーから受け取ったチャットを表すクラス
	/// </summary>
	public class Chat : IErrorData, Hal.NCSPlugin.IChat
	{

		/// <summary>
		/// 投稿者属性
		/// </summary>
		private enum PremiumType
		{
			Normal = 0,
			Premium = 1,
			Caster = 2
		}

		/// <summary>
		/// エラーコード
		/// </summary>
		private enum ERROR_CODE
		{
			None,
			ParseError,
			Undefined
		}

		#region 非公開フィールド

		private string _mail;
		private string _message;
		private string _userId;
		private int _thread;
		private int _no;
		private int _vpos;
		private bool _anonymity;
		private DateTime _date;
		private int _premium;
		
		private ERROR_CODE _errorCode;

		#endregion

		#region IChat メンバ

		/// <summary>
		///  コマンド
		/// </summary>
		public string Mail
		{
			get { return _mail; }
		}

		/// <summary>
		///  コメント
		/// </summary>
		public string Message
		{
			get { return _message; }
		}

		/// <summary>
		/// ユーザーID
		/// </summary>
		public string UserId
		{
			get { return _userId; }
		}

		/// <summary>
		/// 
		/// </summary>
		public int Thread
		{
			get { return _thread; }
		}

		/// <summary>
		/// コメント番号
		/// </summary>
		public int No
		{
			get { return _no; }
		}

		/// <summary>
		///  コメント位置
		/// </summary>
		public int Vpos
		{
			get { return _vpos; }
		}

		/// <summary>
		///  匿名性
		/// </summary>
		public bool Anonymity
		{
			get { return _anonymity; }
		}

		/// <summary>
		/// 投稿時刻
		/// </summary>
		public DateTime Date
		{
			get { return _date; }
		}

		/// <summary>
		/// 投稿者の属性をあらわす数値
		/// </summary>
		public int Premium
		{
			get { return this._premium; }
		}

		/// <summary>
		/// 放送主コメントか
		/// </summary>
		public bool IsOwnerComment
		{
			get { return (this._premium & (int)PremiumType.Caster) != 0; }
		}

		#endregion

		#region IErrorData メンバ

		/// <summary>
		/// エラーコード
		/// </summary>
		public string ErrorCode
		{
			get { return _errorCode.ToString(); }
		}

		/// <summary>
		/// エラーコードの説明
		/// </summary>
		public string ErrorMessage
		{
			get
			{
				switch (_errorCode) {

					case ERROR_CODE.None:
						return "エラーはありません。";
					case ERROR_CODE.ParseError:
						return "解析に失敗しました。";
					case ERROR_CODE.Undefined:
					default:
						return "未定義のエラーが発生しました。";
				}

			}
		}

		/// <summary>
		/// エラーがあるか
		/// </summary>
		public bool HasError
		{
			get { return _errorCode != ERROR_CODE.None; }
		}

		#endregion

		#region コンストラクタ

		/// <summary>
		/// デフォルトコンストラクタ
		/// </summary>
		public Chat()
		{
		}

		/// <summary>
		/// 指定された文字列で初期化します。
		/// </summary>
		/// <param name="xml"></param>
		public Chat(string xml)
			: this()
		{
			this.parse(xml);
		}

		/// <summary>
		/// 指定されたExXMLDocumentで初期化します。
		/// </summary>
		/// <param name="xml"></param>
		public Chat(System.Xml.XmlNode xml)
			: this()
		{
			this.parse(xml);
		}

		/// <summary>
		/// 複製
		/// </summary>
		/// <param name="chat"></param>
		public Chat(NCSPlugin.IChat chat)
			: this()
		{
			this._anonymity = chat.Anonymity;
			this._premium = chat.Premium;
			this._date = chat.Date;
			this._mail = chat.Mail;
			this._message = chat.Message;
			this._no = chat.No;
			this._thread = chat.Thread;
			this._userId = chat.UserId;
			this._vpos = chat.Vpos;
		}

		/// <summary>
		/// 送信用のチャット
		/// </summary>
		/// <param name="message"></param>
		/// <param name="command"></param>
		/// <param name="vpos"></param>
		/// <param name="userid"></param>
		/// <param name="isPremium"></param>
		public Chat(string message, string command, int vpos, string userid, bool isPremium) : this(){
			this._premium = isPremium ? 1 : 0;
			this._mail = command;
			this._message = message;
			this._userId = userid;
			this._vpos = vpos;
		}

		/// <summary>
		/// デバッグ
		/// </summary>
		/// <param name="anonymity"></param>
		/// <param name="premium"></param>
		/// <param name="date"></param>
		/// <param name="mail"></param>
		/// <param name="message"></param>
		/// <param name="no"></param>
		/// <param name="thread"></param>
		/// <param name="userid"></param>
		/// <param name="vpos"></param>
		public Chat(bool anonymity, int premium, DateTime date, string mail, string message, int no, int thread, string userid, int vpos)
			: this()
		{
			this._anonymity = anonymity;
			this._premium = premium;
			this._date = date;
			this._mail = mail;
			this._message = message;
			this._no = no;
			this._thread = thread;
			this._userId = userid;
			this._vpos = vpos;
		}

		#endregion

		/// <summary>
		/// ChatをあらわすXmlを解析する
		/// </summary>
		/// <param name="str"></param>
		void parse(string str)
		{

			if (string.IsNullOrEmpty(str)) {
				throw new ArgumentException();
			}

			System.Xml.XmlDocument xdoc = new System.Xml.XmlDocument();

			try {

				xdoc.LoadXml(str);
				this.parse(xdoc.FirstChild);

			} catch (Exception) {
				this._errorCode = ERROR_CODE.Undefined;
			}


		}

		/// <summary>
		/// ChatをあらわすXmlを解析する
		/// </summary>
		/// <param name="node"></param>
		void parse(System.Xml.XmlNode node)
		{

			if (node == null || !node.Name.Equals("chat")) {
				this._errorCode = ERROR_CODE.ParseError;
				return;
			}

			this._anonymity = (node.Attributes["anonymity"] != null);
			this._message = Utility.Unsanitizing(node.InnerText);
			this._no = Utility.SelectInt(node, "@no", 0);
			this._vpos = Utility.SelectInt(node, "@vpos", 0);
			this._mail = Utility.SelectString(node, "@mail");
			this._userId = Utility.SelectString(node, "@user_id");
			this._thread = Utility.SelectInt(node, "@thread", 0);
			this._date = Utility.SelectDateTime(node, "@date");
			this._premium = Utility.SelectInt(node, "@premium", 0);

			if (this.IsOwnerComment && this._message.StartsWith("/hb")) {
				this._userId = "hb";
			}

			//改行文字を統一
			this._message = this._message.Replace("\r\n", "\n").Replace("\n", "\r\n");
		}

		/// <summary>
		/// スレッド番号とコメント番号が一致しているか調べる
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public override bool Equals(object obj)
		{
			if (obj == null) {
				return false;
			}

			NCSPlugin.IChat that = obj as NCSPlugin.IChat;
			if (that == null) {
				return false;
			}

			return this.Thread == that.Thread && this.No == that.No;
		}

		/// <summary>
		/// コメント番号とスレッド番号からハッシュコードを生成する
		/// </summary>
		/// <returns></returns>
		public override int GetHashCode()
		{
			return this.Thread ^ this.No;
		}
	}
}
