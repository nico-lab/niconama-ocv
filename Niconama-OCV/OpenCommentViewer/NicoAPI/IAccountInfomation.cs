using System;
using System.Collections.Generic;
using System.Text;

namespace OpenCommentViewer.NicoAPI {

	/// <summary>
	/// アカウント情報をあらわすインターフェース
	/// </summary>
	public interface IAccountInfomation
	{
		/// <summary>
		/// ユーザーIDを取得する
		/// </summary>
		int UserId { get; }

		/// <summary>
		/// ユーザー名を取得する
		/// </summary>
		string UserName { get; }

		/// <summary>
		/// プレミアム会員かどうか
		/// </summary>
		bool IsPremium { get; }
	}
}
