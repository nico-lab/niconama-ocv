using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SQLite;

namespace Hal.NicoApiSharp.Cookie
{
	abstract class SqlCookieGetter : CookieGetter
	{
		const string CONNECTIONSTRING_FORMAT = "Data Source={0}";
		private string _query = "";
		
		public override System.Net.Cookie GetCookie(Uri url, string key, string filePath)
		{
			_query = MakeQuery(url, key);
			System.Net.CookieContainer container = GetAllCookies(filePath);
			System.Net.CookieCollection collection = container.GetCookies(url);
			return collection[key];
		}

		public override System.Net.CookieCollection GetCookieCollection(Uri url, string path)
		{
			_query = MakeQuery(url);
			System.Net.CookieContainer container = GetAllCookies(path);
			return container.GetCookies(url);
		}


		public override System.Net.CookieContainer GetAllCookies(string path)
		{
			System.Net.CookieContainer container = new System.Net.CookieContainer();

			if (!System.IO.File.Exists(path)) {
				Logger.Default.LogErrorMessage("指定されたファイルが見つかりませんでした。 " + path);
				return container;
			}

			try {

				using (SQLiteConnection sqlConnection = new SQLiteConnection(string.Format(CONNECTIONSTRING_FORMAT, path))) {
					sqlConnection.Open();

					SQLiteCommand command = sqlConnection.CreateCommand();
					command.Connection = sqlConnection;
					command.CommandText = _query;
					SQLiteDataReader sdr = command.ExecuteReader();

					while (sdr.Read()) {
						List<object> items = new List<object>();

						for (int i = 0; i < sdr.FieldCount; i++) {
							items.Add(sdr[i]);
						}

						System.Net.Cookie cookie = DataToCookie(items.ToArray());
						container.Add(cookie);
						
					}

					sqlConnection.Close();
				}

			} catch (Exception ex) {
				Logger.Default.LogException(ex);
			}

			return container;
		}

		protected abstract System.Net.Cookie DataToCookie(object[] data);
		protected abstract string MakeQuery(Uri url);
		protected abstract string MakeQuery(Uri url, string key);
	}
}
