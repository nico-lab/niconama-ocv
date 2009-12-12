using System;
using System.Collections.Generic;
using System.Text;

namespace Hal.CookieGetterSharp
{

	/// <summary>
	/// IE�̃N�b�L�[�̂���Vista�ȍ~�̕ی샂�[�h�Ŏg����N�b�L�[�݂̂��擾����
	/// </summary>
	class IESafemodeBrowserManager : IBrowserManager
	{
		#region IBrowserManager �����o

		public CookieGetter.BROWSER_TYPE BrowserType
		{
			get { return CookieGetter.BROWSER_TYPE.IESafemode; }
		}

		public IBrowserStatus GetDefaultStatus()
		{
			string cookieFolder = Environment.GetFolderPath(Environment.SpecialFolder.Cookies);
			string lowFolder = System.IO.Path.Combine(cookieFolder, "low");

			BrowserStatus bs = new BrowserStatus();
			bs.Name = BrowserType.ToString();
			bs.CookiePath = lowFolder;
			bs.CookieGetter = new IECookieGetter(lowFolder, false);
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
