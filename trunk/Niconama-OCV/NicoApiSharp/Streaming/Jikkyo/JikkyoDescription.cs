using System;
using System.Collections.Generic;
using System.Text;

using Regex = System.Text.RegularExpressions.Regex;
using Match = System.Text.RegularExpressions.Match;

namespace Hal.NicoApiSharp.Streaming.Jikkyo
{
	/// <summary>
	/// �����̏ڍׂ��擾���܂�
	/// </summary>
	public class JikkyoDescription : Streaming.IDescription, IErrorData
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
		private string _description;

		private ERROR_CODE _errorCode = ERROR_CODE.None;

		/// <summary>
		/// �����y�[�W��������^�C�g���Ȃǂ��܂ޏ����擾����
		/// </summary>
		/// <param name="jikkyoId"></param>
		/// <param name="cookies"></param>
		/// <returns></returns>
		public static JikkyoDescription GetInstance(string jikkyoId, System.Net.CookieContainer cookies)
		{

			if (jikkyoId == null) {
				throw new ArgumentException("jikkyoId��null�ł��B", "liveId");
			}

			JikkyoDescription info = new JikkyoDescription();

			try {
				string url = string.Format(ApiSettings.Default.JikkyoWatchUrlFormat, jikkyoId);
				string html = Utility.GetResponseText(url, cookies, ApiSettings.Default.DefaultApiTimeout);

				if (html != null) {

					info._liveId = jikkyoId;

					Match title = Regex.Match(html, ApiSettings.Default.JikkyoTitleRegPattern);
					Match comid = Regex.Match(html, ApiSettings.Default.JikkyoCommunityIdRegPattern);
					Match comname = Regex.Match(html, ApiSettings.Default.JikkyoCommunityNameRegPattern);
					Match desc = Regex.Match(html, ApiSettings.Default.JikkyoDescriptionRegPattern, System.Text.RegularExpressions.RegexOptions.Singleline);

					if (title.Groups["t"].Success && comname.Groups["t"].Success && desc.Groups["t"].Success) {

						info._title = Utility.Unsanitizing(title.Groups["t"].Value);
						info._caster = "";
						info._communityId = comid.Groups["t"].Value;
						info._communityName = Utility.Unsanitizing(comname.Groups["t"].Value.Trim());
						info._description = Utility.Unsanitizing(desc.Groups["t"].Value);

					} else {
						Logger.Default.LogErrorMessage("�����y�[�W�̉�͂Ɏ��s���܂����B���K�\�����C������K�v������܂��B");
						info._errorCode = ERROR_CODE.ParseError;
					}

				} else {
					Logger.Default.LogErrorMessage("�����y�[�W���擾�ł��܂���ł����B");
					info._errorCode = ERROR_CODE.WebEerror;
				}

			} catch (Exception ex) {
				Logger.Default.LogException(ex);
				return null;
			}

			return info;

		}

		/// <summary>
		/// �����y�[�W��������^�C�g���Ȃǂ��܂ޏ����擾����
		/// </summary>
		/// <param name="liveId"></param>
		/// <returns></returns>
		public static JikkyoDescription GetInstance(string liveId)
		{
			if (LoginManager.DefaultCookies != null) {
				return GetInstance(liveId, LoginManager.DefaultCookies);
			}

			return null;
		}


		#region ILiveDescription �����o

		/// <summary>
		/// ����ID���擾���܂�
		/// </summary>
		public string Id
		{
			get { return _liveId; }
		}

		/// <summary>
		/// �R�~���j�e�BID���擾���܂�
		/// </summary>
		public string CommunityId
		{
			get { return _communityId; }
		}

		/// <summary>
		/// �ԑg�����擾���܂�
		/// </summary>
		public string Title
		{
			get { return _title; }
		}

		/// <summary>
		/// �R�~���j�e�B�����擾���܂�
		/// </summary>
		public string CommunityName
		{
			get { return _communityName; }
		}

		/// <summary>
		/// �����҂��擾���܂�
		/// </summary>
		public string Caster
		{
			get { return _caster; }
		}

		/// <summary>
		/// �����̏ڍ�
		/// </summary>
		public string Description 
		{
			get { return _description; }
		}

		#endregion

		#region IErrorData �����o

		/// <summary>
		/// �G���[�R�[�h���擾���܂�
		/// </summary>
		public string ErrorCode
		{
			get { return _errorCode.ToString(); }
		}

		/// <summary>
		/// �G���[�R�[�h�̐������擾���܂�
		/// </summary>
		public string ErrorMessage
		{
			get { return _errorCode.ToString(); }
		}

		/// <summary>
		/// �G���[�����邩�ǂ������m���߂܂�
		/// </summary>
		public bool HasError
		{
			get { return _errorCode != ERROR_CODE.None; }
		}

		#endregion
	}
}
