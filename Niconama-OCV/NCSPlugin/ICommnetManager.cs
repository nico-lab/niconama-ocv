using System;
using System.Collections.Generic;
using System.Text;

namespace Hal.NCSPlugin
{
	
	/// <summary>
	/// 入力されたIDから該当するコメントを取得する機能を表すインターフェース
	/// 複数のICommnetClientが定義されている場合はPriorityが高いものが実際の処理を担当する
	/// </summary>
	public interface ICommnetManager : ICommnetAgent, IContentStatus, IPlugin
	{

		/// <summary>
		/// 数値が大きいほど優先的に処理される
		/// </summary>
		int Priority { get; }

		/// <summary>
		/// 指定されたIDを処理することが出来るか
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		bool CanHandle(string id);

		
	}
}
