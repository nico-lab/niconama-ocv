using System;
using System.Collections.Generic;
using System.Text;

namespace Hal.OpenCommentViewer
{

	/// <summary>
	/// ユーザー単位の設定を管理するクラス
	/// </summary>
	[Serializable]
	public partial class UserSettings
	{

		/// <summary>
		/// 最後に接続した放送ID
		/// </summary>
		public string LastAccessId = "";
		public List<ColumnStatus> ColumnStates = new List<ColumnStatus>();

		public bool ShowAccountForm = true;
		public int _BrowserType = 0;
		public string CookieFilePath = null;


		/// <summary>
		/// メインフォームのサイズ、位置、初期状態を保持するオブジェクト
		/// </summary>
		public WindowState MainformWindowState = new WindowState();

		[System.Xml.Serialization.XmlIgnoreAttribute()]
		public Hal.NicoApiSharp.Cookie.CookieGetter.BROWSER_TYPE BrowserType
		{
			get { return (NicoApiSharp.Cookie.CookieGetter.BROWSER_TYPE)_BrowserType; }
			set { _BrowserType = (int)value; }
		}

		#region シリアライズ

		const string FILE_PATH = "UserSettings.xml";
		private static UserSettings _default = null;

		public static UserSettings Default
		{

			get
			{
				if (_default == null) {
					_default = Utility.Deserialize(FILE_PATH, typeof(UserSettings)) as UserSettings;
					if (_default == null) {
						_default = new UserSettings();
					}
				}

				return _default;
			}

		}

		public static void Reload()
		{
			_default = Utility.Deserialize(FILE_PATH, typeof(UserSettings)) as UserSettings;
		}

		public void Save()
		{
			Utility.Serialize(FILE_PATH, this, typeof(UserSettings));
		}

		#endregion

		[Serializable]
		public class ColumnStatus
		{
			public string Name = "";
			public int Width = 50;
			public bool Visible = true;
			public int DisplayIndex = 0;
		}

		[Serializable]
		public class WindowState
		{

			public System.Windows.Forms.FormWindowState state = System.Windows.Forms.FormWindowState.Normal;
			public int Width = 100;
			public int Height = 100;
			public int Left = 0;
			public int Top = 0;

			public bool Initialized = false;

			private void Initialize(System.Windows.Forms.Form form)
			{
				Width = form.Width;
				Height = form.Height;
				Left = form.Left;
				Top = form.Top;

				Initialized = true;
			}

			public void Save(System.Windows.Forms.Form form)
			{

				if (form.WindowState == System.Windows.Forms.FormWindowState.Normal) {
					Width = form.Width;
					Height = form.Height;
					Left = form.Left;
					Top = form.Top;
				}
				state = form.WindowState;
			}

			public void Load(System.Windows.Forms.Form form)
			{
				if (!Initialized) {
					form.StartPosition = System.Windows.Forms.FormStartPosition.WindowsDefaultLocation;
					this.Initialize(form);

				} else {
					form.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
					form.WindowState = state;
					form.Width = Width;
					form.Height = Height;
					form.Left = Left;
					form.Top = Top;

				}

			}

		}
	}
}
