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

		public BrowserType BrowserType
		{
			get { return BrowserType.IEComponet; }
		}

		public ICookieGetter CreateDefaultCookieGetter()
		{
			string cookieFolder = Environment.GetFolderPath(Environment.SpecialFolder.Cookies);
			CookieStatus status = new CookieStatus(this.BrowserType.ToString(), cookieFolder, this.BrowserType, PathType.Directory);
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
