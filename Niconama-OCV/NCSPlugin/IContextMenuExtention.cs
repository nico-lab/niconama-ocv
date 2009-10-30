using System;
using System.Collections.Generic;
using System.Text;

namespace Hal.NCSPlugin
{
	/// <summary>
	/// ビューのコンテクストメニューを拡張するためのインターフェース
	/// 実装は任意
	/// PluginHost側もこれを扱えるかどうかは任意
	/// </summary>
	public interface IContextMenuExtention
	{
		/// <summary>
		/// コンテクストメニューに追加するToolStripMenuItemクラスインスタンス
		/// コンストラクタ完了時にアクセス可能である必要がある
		/// </summary>
		object ToolStripMenuItem { get; }

		/// <summary>
		/// チャットビューのコンテクストメニューが開かれる前に実行されます。
		/// </summary>
		void OnContextMenuOpening();
	}
}
