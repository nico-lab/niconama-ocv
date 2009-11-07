using System;
using System.Collections.Generic;
using System.Text;


namespace Hal.OpenCommentViewer.Cookie
{


	/// <summary>
	/// SQLiteを利用してクッキーを保存しているブラウザからクッキーを取得する
	/// </summary>
	abstract class SqliteCookieGetter : ICookieGetter
	{

		const string CONNECTIONSTRING_FORMAT = "Data Source={0}";
		public abstract string[] GetCookieValues(string url, string key);


		/// <summary>
		/// 指定されたpathにあるSqliteDatabaseに対してQueryを実行して値を取得する
		/// </summary>
		/// <param name="path"></param>
		/// <param name="query"></param>
		/// <returns></returns>
		protected string[] getDatabaseValues(string path, string query)
		{
			string dpstr;
			try {

				if (Environment.OSVersion.ToString().Contains("Windows")) {
					dpstr = "System.Data.SQLite";
				} else {
					dpstr = "Mono.Data.Sqlite";
				}

				// DBプロバイダファクトリ作成
				System.Data.Common.DbProviderFactory dpf = System.Data.Common.DbProviderFactories.GetFactory(dpstr);

				// 1.DBコネクションオブジェクト作成
				using (System.Data.Common.DbConnection dbcon = dpf.CreateConnection()) {
					dbcon.ConnectionString = string.Format(CONNECTIONSTRING_FORMAT, path);
					dbcon.Open();

					System.Data.Common.DbCommand command = dpf.CreateCommand();
					command.Connection = dbcon;
					command.CommandText = query;

					string res = command.ExecuteScalar() as string;
					dbcon.Close();
					return new string[] { res };

				}

			} catch (Exception ex) {
				Logger.Default.LogException(ex);
			}

			return null;

		}
	}
}
