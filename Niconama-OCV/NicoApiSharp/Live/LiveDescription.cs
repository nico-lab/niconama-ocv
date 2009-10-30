﻿using System;
using System.Collections.Generic;
using System.Text;

using Regex = System.Text.RegularExpressions.Regex;
using Match = System.Text.RegularExpressions.Match;

namespace Hal.NicoApiSharp.Live
{

	/// <summary>
	/// 放送の詳細をあらわすクラス
	/// </summary>
	public class LiveDescription : ILiveDescription, IErrorData
	{

		private enum ERROR_CODE
		{
			None,
			ParseError,
			WebEerror,
			Undefined
		}

		private string _liveId;
		private string _communityId;
		private string _title;
		private string _caster;
		private string _communityName;

		private ERROR_CODE _errorCode = ERROR_CODE.None;

		/// <summary>
		/// 放送ページから放送タイトルなどを含む情報を取得する
		/// </summary>
		/// <param name="liveId"></param>
		/// <param name="cookies"></param>
		/// <returns></returns>
		public static LiveDescription GetInstance(string liveId, System.Net.CookieContainer cookies)
		{

			if (liveId == null) {
				throw new ArgumentException("liveIdがnullです。", "liveId");
			}

			LiveDescription info = new LiveDescription();

			try {
				string url = string.Format(ApiSettings.Default.LiveWatchUrlFormat, liveId);
				string html = Utility.GetResponseText(url, cookies, ApiSettings.Default.DefaultApiTimeout);

				if (html != null) {

					info._liveId = liveId;

					Match title = Regex.Match(html, ApiSettings.Default.LiveTitleRegPattern);
					Match caster = Regex.Match(html, ApiSettings.Default.LiveCasterRegPattern);
					Match comid = Regex.Match(html, ApiSettings.Default.LiveCommunityIdRegPattern);
					Match comname = Regex.Match(html, ApiSettings.Default.LiveCommunityNameRegPattern);

					if (title.Groups["t"].Success && caster.Groups["t"].Success && comname.Groups["t"].Success) {

						info._title = Utility.Unsanitizing(title.Groups["t"].Value);
						info._caster = Utility.Unsanitizing(caster.Groups["t"].Value);
						info._communityId = comid.Groups["t"].Value;
						info._communityName = Utility.Unsanitizing(comname.Groups["t"].Value.Trim());

					} else {
						Logger.Default.LogErrorMessage("放送ページの解析に失敗しました。正規表現を修正する必要があります。");
						info._errorCode = ERROR_CODE.ParseError;
					}

				} else {
					Logger.Default.LogErrorMessage("放送ページを取得できませんでした。");
					info._errorCode = ERROR_CODE.WebEerror;
				}

			} catch (Exception ex) {
				Logger.Default.LogException(ex);
				return null;
			}

			return info;

		}

		#region IDescription メンバ

		/// <summary>
		/// 放送IDを取得します
		/// </summary>
		public string LiveId
		{
			get { return _liveId; }
		}

		/// <summary>
		/// コミュニティIDを取得します
		/// </summary>
		public string CommunityId
		{
			get { return _communityId; }
		}

		/// <summary>
		/// 番組名を取得します
		/// </summary>
		public string LiveName
		{
			get { return _title; }
		}

		/// <summary>
		/// コミュニティ名を取得します
		/// </summary>
		public string CommunityName
		{
			get { return _communityName; }
		}

		/// <summary>
		/// 放送者を取得します
		/// </summary>
		public string Caster
		{
			get { return _caster; }
		}

		#endregion

		#region IErrorData メンバ

		/// <summary>
		/// エラーコードを取得します
		/// </summary>
		public string ErrorCode
		{
			get { return _errorCode.ToString(); }
		}

		/// <summary>
		/// エラーコードの説明を取得します
		/// </summary>
		public string ErrorMessage
		{
			get { return _errorCode.ToString(); }
		}

		/// <summary>
		/// エラーがあるかどうかを確かめます
		/// </summary>
		public bool HasError
		{
			get { return _errorCode != ERROR_CODE.None; }
		}

		#endregion


	}
}
