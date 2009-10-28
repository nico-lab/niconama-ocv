using System;
using System.Collections.Generic;
using System.Text;

namespace OpenCommentViewer
{
	/// <summary>
	/// アプリケーションが使用する設定を管理するクラス
	/// </summary>
	[Serializable]
	public class ApplicationSettings
	{

		#region NicoAPI URLs

		/// <summary>
		/// API：GetPlayerStatusにアクセスするためのアドレスフォーマット
		/// {0} = 放送ID
		/// </summary>
		public string GetPlayerStatusUrlFormat = "http://watch.live.nicovideo.jp/api/getplayerstatus?v={0}";

		/// <summary>
		/// 放送ページのURLフォーマット
		/// {0} = 放送ID
		/// </summary>
		public string LiveWatchUrlFormat = "http://live.nicovideo.jp/watch/{0}";

		/// <summary>
		/// Ngコマンド送信用URLフォーマット
		/// {0} = 放送ID
		/// {1} = Mode (ADD | DEL | GET)
		/// {2} = Source
		/// {3} = Type (WORD | ID | COMMAD)
		/// </summary>
		public string NgcommandUrlFormat = "http://watch.live.nicovideo.jp/api/configurengword?video={0}&mode={1}&source={2}&type={3}";

		/// <summary>
		/// ウェイバックキー取得用URLフォーマット
		/// {0} = スレッド
		/// </summary>
		public string WaybackkeyUrlFormat = "http://watch.live.nicovideo.jp/api/getwaybackkey?thread={0}";

		/// <summary>
		/// FLV情報を取得するためのURL(sm9限定)
		/// </summary>
		public string GetFlvUrl = "http://www.nicovideo.jp/api/getflv/sm9";

		/// <summary>
		/// HeartBeatを取得するためのURLフォーマット
		/// {0} = 放送ID
		/// </summary>
		public string GetHeartBeatUrlFormat = "http://watch.live.nicovideo.jp/api/heartbeat?v={0}";

		/// <summary>
		/// コメント送信キーを取得するためのURLフォーマット
		/// {0} = スレッド
		/// {1} = ブロック番号（送信したいコメントの番号 / 100）
		/// </summary>
		public string GetPostKeyUrlFormat = "http://watch.live.nicovideo.jp/api/getpostkey?thread={0}&block_no={1}";

		#endregion

		#region Socket

		/// <summary>
		/// コメントサーバーからメッセージを取得する際に書き込む情報のフォーマット
		/// {0} = スレッド番号
		/// {1} = 開始コメント番号
		/// {2} = UserID
		/// </summary>
		public string ThreadStartMessageFormat = "<thread thread=\"{0}\" res_from=\"{1}\" version=\"20061206\" />";

		/// <summary>
		/// コメントサーバーからメッセージを取得する際に書き込む情報のフォーマット
		/// {0} = スレッド番号
		/// {1} = 開始コメント番号
		/// {2} = ?
		/// {3} = ウェイバックキー
		/// {4} = ユーザーID
		/// </summary>
		public string WaybackThreadStartMessageFormat = "<thread thread=\"{0}\" res_from=\"{1}\" version=\"20061206\" when=\"{2}\" waybackkey=\"{3}\" user_id=\"{4}\" />";

		/// <summary>
		/// 一度にサーバーから受け取れるバイト数
		/// </summary>
		public int ReceiveBufferSize = 4096;

		#endregion

		#region Send Commnet

		/// <summary>
		/// 放送主コメント送信用URLフォーマット
		/// {0} = 放送ID
		/// </summary>
		public string PostOwnerCommentUrlFormat = "http://watch.live.nicovideo.jp/api/broadcast/{0}";

		/// <summary>
		/// 放送主コメントデータフォーマット
		/// {0} = コメント
		/// {1} = コマンド
		/// </summary>
		public string PostOwnerCommentDataFormat = "body={0}&mail={1}&is184=true";

		/// <summary>
		/// 放送主コメント送信時のリファラー
		/// </summary>
		public string PostOwnerCommentReferer = "http://live.nicovideo.jp/console.swf?090810";

		/// <summary>
		/// 放送主コメント送信時のユーザーエージェント
		/// </summary>
		public string PostOwnerCommentUserAgent = "SimpleViewer";

		#endregion


		#region RegexPattern

		/// <summary>
		/// 放送ページから放送タイトルを取得するための正規表現パターン
		/// </summary>
		public string LiveTitleRegPattern = "<title>(?\'t\'[^<>]*) - ニコニコ生放送</title>";

		/// <summary>
		/// 放送ページから放送者名を取得するための正規表現パターン
		/// </summary>
		public string LiveCasterRegPattern = "放送者:<strong class=\\\"[^\\\"]*\\\">(?\'t\'[^<>]*)";

		/// <summary>
		/// 放送ページからコミュニティ名を取得するための正規表現パターン
		/// </summary>
		public string LiveCommunityNameRegPattern = "class=\\\"(community|channel)\\\" target=\\\"_blank\\\">(?\'t\'[^<>]*)</a>";

		/// <summary>
		/// 放送ページからコミュニティIDを取得するための正規表現パターン
		/// </summary>
		public string LiveCommunityIdRegPattern = "href=\"http://ch.nicovideo.jp/community/(?\'t\'co\\d+)\"";

		/// <summary>
		/// URLからIDを取得するための正規表現パターン
		/// </summary>
		public string LiveIdRegPattern = "^((http://live.nicovideo.jp/watch/)|(http://live.nicovideo.jp/console.swf?v=))?(?'id'lv\\d+)";

		/// <summary>
		/// FLV情報からアカウントインフォメーションを取得するための正規表現
		/// </summary>
		public string AccountInfomationRegPattern = "user_id=(?'id'\\d+)&is_premium=(?'pre'\\d+)?&nickname=(?'name'[^&]+)";

		#endregion


		/// <summary>
		/// 放送主の座席番号
		/// </summary>
		public int OwnerSheetNo = 777;

		/// <summary>
		/// API通信のタイムアウトの既定値
		/// </summary>
		public int DefaultApiTimeout = 10000;

		/// <summary>
		/// メッセージサーバとのコネクション時のタイムアウトの既定値
		/// </summary>
		public int DefaultConnectionTimeout = 3000;

		public int _StatusMessageColor = System.Drawing.Color.Black.ToArgb();

		public int _FatalMessageColor = System.Drawing.Color.Red.ToArgb();

		/// <summary>
		/// メイン画面のステータスメッセージの色
		/// </summary>
		[System.Xml.Serialization.XmlIgnoreAttribute()]
		public System.Drawing.Color StatusMessageColor {
			get { return System.Drawing.Color.FromArgb(_StatusMessageColor); }
			set { _StatusMessageColor = value.ToArgb(); }
		}

		/// <summary>
		/// メイン画面の警告メッセージの色
		/// </summary>
		[System.Xml.Serialization.XmlIgnoreAttribute()]
		public System.Drawing.Color FatalMessageColor {
			get { return System.Drawing.Color.FromArgb(_FatalMessageColor); }
			set { _FatalMessageColor = value.ToArgb(); }
		}


		/// <summary>
		/// Plugin用DLLが保存されるフォルダ　%APPDATA%はアプリケーションデータフォルダーに置換される(例 %APPDATA%/Plugins)
		/// </summary>
		public string _PluginsFolder = "Plugins";



		[System.Xml.Serialization.XmlIgnoreAttribute()]
		public string PluginsFolder
		{
			get
			{
				string path = _PluginsFolder.Replace("%APPDATA%", this.ApplicationDataFolder);

				if (!System.IO.Directory.Exists(path)) {
					System.IO.Directory.CreateDirectory(path);
				}

				return path;
			}
		}

		/// <summary>
		/// Pluginの設定が保存されるフォルダ　%APPDATA%はアプリケーションデータフォルダーに置換される(例 %APPDATA%/Plugins)
		/// </summary>
		public string _PluginsDataFolder = "Plugins";

		[System.Xml.Serialization.XmlIgnoreAttribute()]
		public string PluginsDataFolder
		{
			get
			{
				string path = _PluginsDataFolder.Replace("%APPDATA%", this.ApplicationDataFolder);

				if (!System.IO.Directory.Exists(path)) {
					System.IO.Directory.CreateDirectory(path);
				}

				return path;
			}
		}

		/// <summary>
		/// Ticket保存用フォルダ
		/// </summary>
		public string _LiveTicketsFolder = "Tickets";

		/// <summary>
		/// Ticket保存用フォルダ
		/// </summary>
		[System.Xml.Serialization.XmlIgnoreAttribute()]
		public string LiveTicketsFolder
		{
			get
			{
				string path = _LiveTicketsFolder.Replace("%APPDATA%", this.ApplicationDataFolder);

				if (!System.IO.Directory.Exists(path)) {
					System.IO.Directory.CreateDirectory(path);
				}

				return path;
			}
		}

		[System.Xml.Serialization.XmlIgnoreAttribute()]
		public string ApplicationDataFolder
		{
			get
			{
				return System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), System.Windows.Forms.Application.ProductName);
			}
		}



		#region シリアライズ

		const string FILE_PATH = "ApplicationSettings.xml";
		private static ApplicationSettings _default = null;

		/// <summary>
		/// 既定の設定を取得する
		/// </summary>
		public static ApplicationSettings Default
		{

			get
			{

				if (_default == null) {
					_default = Utility.Deserialize(FILE_PATH, typeof(ApplicationSettings)) as ApplicationSettings;
					if (_default == null) {
						_default = new ApplicationSettings();
						Utility.Serialize(FILE_PATH, _default, typeof(ApplicationSettings));
					}
				}

				return _default;
			}

		}

		#endregion



	}
}
