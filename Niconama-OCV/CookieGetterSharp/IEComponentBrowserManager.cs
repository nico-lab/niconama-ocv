using System;
using System.Collections.Generic;
using System.Text;

namespace Hal.CookieGetterSharp
{
	/// <summary>
	/// IE�R���|�[�l���g�ŃA�N�Z�X�\�ȃN�b�L�[�݂̂��擾����
	/// </summary>
	class IEComponentBrowserManager : IBrowserManager
	{
		#region IBrowserManager �����o

		public CookieGetter.BROWSER_TYPE BrowserType
		{
			get { return CookieGetter.BROWSER_TYPE.IEComponet; }
		}

		public IBrowserStatus GetDefaultStatus()
		{
			string cookieFolder = Environment.GetFolderPath(Environment.SpecialFolder.Cookies);
			BrowserStatus bs = new BrowserStatus();
			bs.Name = BrowserType.ToString();
			bs.CookiePath = cookieFolder;
			bs.CookieGetter = new IECookieGetter(cookieFolder, false);
			return bs;
		}

		/// <summary>
		/// IEBrowserManager�Ŋ��ɂ��킹�ēK�؂ȕ���Ԃ��悤�ɂ��Ă���̂ŁA�����ł͉������Ȃ�
		/// </summary>
		/// <returns></returns>
		public IBrowserStatus[] GetStatus()
		{
			return new IBrowserStatus[0];
		}

		#endregion
	}
}
