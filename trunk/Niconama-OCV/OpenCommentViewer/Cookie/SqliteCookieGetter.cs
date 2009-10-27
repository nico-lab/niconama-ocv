using System;
using System.Collections.Generic;
using System.Text;

#if MONO
using SQLiteConnection = Mono.Data.Sqlite.SqliteConnection;
using SQLiteCommand = Mono.Data.Sqlite.SqliteCommand;
#else
using System.Data.SQLite;
#endif


namespace OpenCommentViewer.Cookie
{


	/// <summary>
	/// SQLiteを利用してクッキーを保存しているブラウザからクッキーを取得する
	/// </summary>
	abstract class SqliteCookieGetter : ICookieGetter
	{

		const string CONNECTIONSTRING_FORMAT = "Data Source={0}";
		public abstract string GetCookieValue(string url, string key);


		/// <summary>
		/// 指定されたpathにあるSqliteDatabaseに対してQueryを実行して値を取得する
		/// </summary>
		/// <param name="path"></param>
		/// <param name="query"></param>
		/// <returns></returns>
		protected string getDatabaseValue(string path, string query)
		{

			try {

				using (SQLiteConnection connection = new SQLiteConnection(string.Format(CONNECTIONSTRING_FORMAT, path))) {
					SQLiteCommand command = new SQLiteCommand(query, connection);

					connection.Open();
					string res = command.ExecuteScalar() as string;
					connection.Close();
					return res;

				}

			} catch (Exception ex) {
				Logger.Default.LogException(ex);
			}

			return null;

		}

	}
}
