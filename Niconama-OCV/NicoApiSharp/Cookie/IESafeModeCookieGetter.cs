using System;
using System.Collections.Generic;
using System.Text;

namespace Hal.NicoApiSharp.Cookie
{

	/// <summary>
	/// Vista・IEの保護モードクッキーを取得する
	/// </summary>
	class IESafeModeCookieGetter : IEComponentCookieGetter
	{

		protected override string GetCookieDirectory()
		{
			string dir = base.GetCookieDirectory();
			return System.IO.Path.Combine(dir, "low");
		}

	}
}
