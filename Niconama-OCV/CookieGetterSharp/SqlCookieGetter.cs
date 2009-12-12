using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SQLite;

namespace Hal.CookieGetterSharp
{
	abstract class SqlCookieGetter : CookieGetter
	{
		const string CONNECTIONSTRING_FORMAT = "Data Source={0}";

		public override System.Net.Cookie GetCookie(Uri url, string key)
		{
			System.Net.CookieContainer container = GetCookies(base.CookiePath, MakeQuery(url, key));
			System.Net.CookieCollection collection = container.GetCookies(AddSrashLast(url));
			return collection[key];
		}

		public override System.Net.CookieCollection GetCookieCollection(Uri url)
		{
			System.Net.CookieContainer container = GetCookies(base.CookiePath, MakeQuery(url));
			return container.GetCookies(AddSrashLast(url));
		}

		public override System.Net.CookieContainer GetAllCookies()
		{
			return GetCookies(base.CookiePath, MakeQuery());
		}

		protected virtual System.Net.CookieContainer GetCookies(string path, string query)
		{
			System.Net.CookieContainer container = new System.Net.CookieContainer();

			if (path == null || !System.IO.File.Exists(path)) return container;

			string temp = null;

			try {
				temp = System.IO.Path.GetTempFileName();
				System.IO.File.Copy(path, temp, true);

				using (SQLiteConnection sqlConnection = new SQLiteConnection(string.Format(CONNECTIONSTRING_FORMAT, temp))) {
					sqlConnection.Open();

					SQLiteCommand command = sqlConnection.CreateCommand();
					command.Connection = sqlConnection;
					command.CommandText = query;
					SQLiteDataReader sdr = command.ExecuteReader();

					while (sdr.Read()) {
						List<object> items = new List<object>();

						for (int i = 0; i < sdr.FieldCount; i++) {
							items.Add(sdr[i]);
						}

						System.Net.Cookie cookie = DataToCookie(items.ToArray());
						try {
							AddCookieToContainer(container, cookie);
						} catch {
							Console.WriteLine(string.Format("Invalid Format! domain:{0},key:{1},value:{2}", cookie.Domain, cookie.Name, cookie.Value));
						}

					}

					sqlConnection.Close();
				}

			} catch (Exception ex) {
				throw new CookieGetterException("クッキーを取得中、Sqliteアクセスでエラーが発生しました。", ex);
			} finally {
				if (temp != null) {
					System.IO.File.Delete(temp);
				}
			}

			return container;
		}

		protected abstract System.Net.Cookie DataToCookie(object[] data);
		protected abstract string MakeQuery();
		protected abstract string MakeQuery(Uri url);
		protected abstract string MakeQuery(Uri url, string key);
	}
}
