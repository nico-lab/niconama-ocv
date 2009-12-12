using System;
using System.Collections.Generic;
using System.Text;

namespace Hal.CookieGetterSharp
{
	class Utility
	{
		/// <summary>
		/// Unix���Ԃ�DateTime�ɕϊ�����
		/// </summary>
		/// <param name="UnixTime"></param>
		/// <returns></returns>
		public static DateTime UnixTimeToDateTime(int UnixTime)
		{
			return new DateTime(1970, 1, 1, 9, 0, 0).AddSeconds(UnixTime);
		}

		/// <summary>
		/// DateTime��Unix���Ԃɕϊ�����
		/// </summary>
		/// <param name="time"></param>
		/// <returns></returns>
		public static int DateTimeToUnixTime(DateTime time)
		{
			TimeSpan t = time.Subtract(new DateTime(1970, 1, 1, 9, 0, 0));
			return (int)t.TotalSeconds;
		}

		/// <summary>
		/// %APPDATA%�Ȃǂ����ۂ̃p�X�ɕϊ�����
		/// </summary>
		/// <param name="path"></param>
		/// <returns></returns>
		public static string ReplacePathSymbols(string path)
		{
			path = path.Replace("%APPDATA%", Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData));
			path = path.Replace("%LOCALAPPDATA%", Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData));
			path = path.Replace("%COOKIES%", Environment.GetFolderPath(Environment.SpecialFolder.Cookies));
			return path;
		}

		/// <summary>
		/// �K�v�������uri�̍Ō��/������
		/// Path�̎w�肪����ꍇ�Auri�̍Ō��/�����邩�Ȃ����Ŏ擾�ł��Ȃ��ꍇ������̂�
		/// </summary>
		/// <param name="uri"></param>
		/// <returns></returns>
		public static Uri AddSrashLast(Uri uri)
		{
			string o = uri.Segments[uri.Segments.Length - 1];
			string no = uri.OriginalString;//.Replace("http://", "http://o.");
			if (!o.Contains(".") && o[o.Length - 1] != '/') {
				no += "/";
			}
			return new Uri(no);
		}

		/// <summary>
		/// �N�b�L�[�R���e�i�ɃN�b�L�[��ǉ�����
		/// domain��.hal.fscs.jp�Ȃǂ��� http://hal.fscs.jp �ŃN�b�L�[���L���ɂȂ�Ȃ��̂�.����ƂȂ������w�肷��
		/// </summary>
		/// <param name="container"></param>
		/// <param name="cookie"></param>
		public static void AddCookieToContainer(System.Net.CookieContainer container, System.Net.Cookie cookie)
		{

			if (container == null) {
				throw new ArgumentNullException("container");
			}

			if (cookie == null) {
				throw new ArgumentNullException("cookie");
			}

			container.Add(cookie);
			if (cookie.Domain.StartsWith(".")) {
				container.Add(new System.Net.Cookie(cookie.Name, cookie.Value, cookie.Path, cookie.Domain.Substring(1)));
			}

		}

		
	}
}
