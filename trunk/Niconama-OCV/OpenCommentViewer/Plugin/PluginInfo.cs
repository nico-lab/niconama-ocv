using System;
using System.Collections.Generic;
using System.Text;

namespace OpenCommentViewer.Plugin
{
	class PluginInfo : IPluginInfo
	{
		private string _location;
		private string _className;

		/// <summary>
		/// PluginInfoクラスのコンストラクタ
		/// </summary>
		/// <param name="path">アセンブリファイルのパス</param>
		/// <param name="cls">クラスの名前</param>
		private PluginInfo(string path, string cls)
		{
			this._location = path;
			this._className = cls;
		}

		/// <summary>
		/// アセンブリファイルのパス
		/// </summary>
		public string Location
		{
			get { return _location; }
		}

		/// <summary>
		/// クラスの名前
		/// </summary>
		public string ClassName
		{
			get { return _className; }
		}

		/// <summary>
		/// 有効なプラグインを探す
		/// </summary>
		/// <returns>有効なプラグインのPluginInfo配列</returns>
		public static IPluginInfo[] FindPlugins()
		{
			List<IPluginInfo> plugins = new List<IPluginInfo>();
			
			//IPlugin型の名前
			string ipluginName = typeof(NCSPlugin.IPlugin).FullName;

			//プラグインフォルダ
			string folder = ApplicationSettings.Default.PluginsFolder;
			if (!System.IO.Directory.Exists(folder)){
				System.IO.Directory.CreateDirectory(folder);
				return new PluginInfo[0];
			}

			FindPlugins(plugins, ipluginName, folder);
			
			//１階層だけ下のフォルダを見に行く
			string[] folders = System.IO.Directory.GetDirectories(folder);

			foreach (string sub in folders) {
				FindPlugins(plugins, ipluginName, sub);
			}
			
			//コレクションを配列にして返す
			return plugins.ToArray();
		}

		private static void FindPlugins(List<IPluginInfo> plugins, string ipluginName, string folder)
		{
			//.dllファイルを探す
			string[] dlls = System.IO.Directory.GetFiles(folder, "*.dll");

			foreach (string dll in dlls) {
				try {
					//アセンブリとして読み込む
					System.Reflection.Assembly asm = System.Reflection.Assembly.LoadFrom(dll);
					foreach (Type t in asm.GetTypes()) {
						//アセンブリ内のすべての型について、
						//プラグインとして有効か調べる
						if (t.IsClass && t.IsPublic && !t.IsAbstract && t.GetInterface(ipluginName) != null) {
							//PluginInfoをコレクションに追加する
							plugins.Add(new PluginInfo(dll, t.FullName));
						}
					}
				} catch {
				}
			}

		}

		/// <summary>
		/// プラグインクラスのインスタンスを作成する
		/// </summary>
		/// <returns>プラグインクラスのインスタンス</returns>
		public NCSPlugin.IPlugin CreateInstance()
		{
			try {
				//アセンブリを読み込む
				System.Reflection.Assembly asm = System.Reflection.Assembly.LoadFrom(this.Location);
				
				//クラス名からインスタンスを作成する
				NCSPlugin.IPlugin plugin = (NCSPlugin.IPlugin)asm.CreateInstance(this.ClassName);
				
				return plugin;

			} catch {
				return null;
			}
		}

	}
}
