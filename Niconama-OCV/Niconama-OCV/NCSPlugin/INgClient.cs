using System;
using System.Collections.Generic;
using System.Text;

namespace NCSPlugin {

	/// <summary>
	/// NGの種類
	/// </summary>
	public enum NGType {
		/// <summary>
		/// NGなし
		/// </summary>
		None,

		/// <summary>
		/// 単語
		/// </summary>
		Word,

		/// <summary>
		/// ユーザー
		/// </summary>
		Id,

		/// <summary>
		/// コマンド
		/// </summary>
		Command
	}

	/// <summary>
	/// NGクライアントをあらわすインターフェース
	/// </summary>
  public interface INgClient {

		/// <summary>
		/// NGの種類を取得します
		/// </summary>
		NGType Type { get; }

		/// <summary>
		/// NGのソースを取得します
		/// </summary>
		string Source { get; }

		/// <summary>
		/// NGの追加日を取得します
		/// </summary>
		DateTime RegisterTime { get; }

		/// <summary>
		/// 読み取り専用かどうかを取得します
		/// </summary>
		bool ReadOnly { get; }

		/// <summary>
		/// 空白文字や文字種の違いを無視するかどうかを取得します
		/// </summary>
		bool UseCaseUnify { get; }

		/// <summary>
		/// 正規表現であるかどうかを取得します
		/// </summary>
		bool IsRegex { get; }
  }
}
