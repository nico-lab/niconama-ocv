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

		public BrowserType BrowserType
		{
			get { return BrowserType.IESafemode; }
		}

		public ICookieGetter CreateDefaultCookieGetter()
		{
			string cookieFolder = Environment.GetFolderPath(Environment.SpecialFolder.Cookies);
			string lowFolder = System.IO.Path.Combine(cookieFolder, "low");
			CookieStatus status = new CookieStatus(this.BrowserType.ToString(), lowFolder, this.BrowserType, PathType.Directory);
			return new IECookieGetter(status, false);
		}

		/// <summary>
		/// IEBrowserManager�Ŋ��ɂ��킹�ēK�؂ȕ���Ԃ��悤�ɂ��Ă���̂ŁA�����ł͉������Ȃ�
		/// </summary>
		/// <returns></returns>
		public ICookieGetter[] CreateCookieGetters()
		{
			return new ICookieGetter[0];
		}

		#endregion
	}
}
