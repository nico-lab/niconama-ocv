using System;
using System.Collections.Generic;
using System.Text;

namespace Hal.NCSPlugin
{

	/// <summary>
	/// プラグインのインターフェース
	/// version 1
	/// </summary>
	public interface IPlugin : IDisposable
	{

		/// <summary>
		/// プラグインの名前
		/// </summary>
		string Name { get;}

		/// <summary>
		/// プラグインの説明
		/// </summary>
		string Description { get;}

		/// <summary>
		/// ホストアプリケーション開始時に実行される
		/// </summary>
		/// <param name="host"></param>
		void Initialize(IPluginHost host);
	}
}
