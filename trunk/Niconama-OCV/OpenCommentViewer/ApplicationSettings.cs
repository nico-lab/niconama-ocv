using System;
using System.Collections.Generic;
using System.Text;

namespace Hal.OpenCommentViewer
{
	/// <summary>
	/// アプリケーションが使用する設定を管理するクラス
	/// </summary>
	[Serializable]
	public class ApplicationSettings
	{
		const int SETTING_VERSION = 1;
		public int SettingVersion = SETTING_VERSION;

		/// <summary>
		/// メイン画面のステータスメッセージ色の内部表現
		/// </summary>
		public int _StatusMessageColor = System.Drawing.Color.Black.ToArgb();

		/// <summary>
		/// メイン画面の警告メッセージ色の内部表現
		/// </summary>
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


		/// <summary>
		/// Plugin用DLLが保存されるフォルダ　%APPDATA%はアプリケーションデータフォルダーに置換される(例 %APPDATA%/Plugins)
		/// </summary>
		[System.Xml.Serialization.XmlIgnoreAttribute()]
		public string PluginsFolder
		{
			get
			{
				string path = NicoApiSharp.Utility.ReplacePathSymbols(_PluginsFolder);

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

		/// <summary>
		/// Pluginの設定が保存されるフォルダ　%APPDATA%はアプリケーションデータフォルダーに置換される(例 %APPDATA%/Plugins)
		/// </summary>
		[System.Xml.Serialization.XmlIgnoreAttribute()]
		public string PluginsDataFolder
		{
			get
			{
				string path = NicoApiSharp.Utility.ReplacePathSymbols(_PluginsDataFolder);

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
				string path = NicoApiSharp.Utility.ReplacePathSymbols(_LiveTicketsFolder); 
				if (!System.IO.Directory.Exists(path)) {
					System.IO.Directory.CreateDirectory(path);
				}

				return path;
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
					if (_default == null || _default.SettingVersion < SETTING_VERSION) {
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
