using System;
using System.Collections.Generic;
using System.Text;

namespace Hal.NicoApiSharp
{
	/// <summary>
	/// アプリケーションが使用する設定を管理するクラス
	/// </summary>
	[Serializable]
	public class ApiSettings
	{
		const int SETTING_VERSION = 1;
		public int SettingVersion = SETTING_VERSION;

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

		/// <summary>
		/// 名前を種とくるためのユーザーページ（チャンネル）のURLフォーマット
		/// {0} = ユーザーID
		/// </summary>
		public string UserPageUrlFormat = "http://www.nicovideo.jp/user/{0}/channel";

		/// <summary>
		/// 自分のアカウント情報を確認するためのURL
		/// </summary>
		public string MyAccountCheckUrl = "http://www.nicovideo.jp/my/channel";

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
		public string LiveCasterRegPattern = "放送者:<strong class=\"[^\"]*\">(<a href=\"http://www.nicovideo.jp/user/\\d+\" target=\"_blank\">)?(?'t'[^<>]*)";

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

		/// <summary>
		/// ユーザーページからIDを取得するための正規表現
		/// </summary>
		public string UserProfileIdRegPattern = "ID：<strong>(\\d+)";

		/// <summary>
		/// ユーザーページからプレミアムかどうかを取得するための正規表現
		/// </summary>
		public string UserProfilePremiumRegPattern = "</span> プレミアム会員<br>";

		/// <summary>
		/// ユーザーページから名前を取得するための正規表現
		/// </summary>
		public string UserProfileNameRegPattern = "<strong style=\"font-size:18px; line-height:1;\">([^<>]+)</strong> さん";

		/// <summary>
		/// ログインした人のアカウントIDを取得するための正規表現
		/// </summary>
		public string MyAccountIdRegPattern = "var\\s+User\\s+=\\s+{\\s+id:\\s+(\\d+),";

		/// <summary>
		/// ログインした人のアカウントがプレミアムかどうかを取得するための正規表現
		/// </summary>
		public string MyAccountPremiumRegPattern = "var\\s+User\\s+=\\s+{\\s+id:\\s+\\d+,\\s+isPremium:\\s+true";

		/// <summary>
		/// ログインした人のアカウント名を取得するための正規表現
		/// </summary>
		public string MyAccountNameRegPattern = "<strong style=\"font-size:16px; line-height:1;\">([^<>]+)</strong> さん";

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

		#region シリアライズ

		const string FILE_PATH = "ApiSettings.xml";
		private static ApiSettings _default = null;

		/// <summary>
		/// 既定の設定を取得する
		/// </summary>
		public static ApiSettings Default
		{

			get
			{

				if (_default == null) {
					_default = Utility.Deserialize(FILE_PATH, typeof(ApiSettings)) as ApiSettings;
					if (_default == null || _default.SettingVersion < SETTING_VERSION) {
						_default = new ApiSettings();
						Utility.Serialize(FILE_PATH, _default, typeof(ApiSettings));
					}
				}

				return _default;
			}

		}

		#endregion



	}
}
