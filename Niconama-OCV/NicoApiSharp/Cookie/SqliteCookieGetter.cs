﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SQLite;

namespace Hal.NicoApiSharp.Cookie
{


	/// <summary>
	/// SQLiteを利用してクッキーを保存しているブラウザからクッキーを取得する
	/// </summary>
	abstract class SqliteCookieGetter : ICookieGetter
	{

		const string CONNECTIONSTRING_FORMAT = "Data Source={0}";
		public abstract System.Net.Cookie[] GetCookies(Uri url, string key);
		public abstract System.Net.Cookie[] GetCookies(Uri url, string key, string path);
		public abstract System.Net.CookieCollection[] GetCookieCollection(Uri url);
		public abstract System.Net.CookieCollection[] GetCookieCollection(Uri url, string path);

		/// <summary>
		/// 指定されたpathにあるSqliteDatabaseに対してQueryを実行して値を取得する
		/// </summary>
		/// <param name="path"></param>
		/// <param name="query"></param>
		/// <returns></returns>
		protected object[] getDatabaseValues(string path, string query)
		{
			//string dpstr;
			try {

				//if (Environment.OSVersion.ToString().Contains("Windows")) {
				//    dpstr = "System.Data.SQLite";
				//    Logger.Default.LogMessage("Start Windowsmode");
				//} else {
				//    dpstr = "Mono.Data.Sqlite";
				//    Logger.Default.LogMessage("Start monomode");

				//}

				//// DBプロバイダファクトリ作成
				//System.Data.Common.DbProviderFactory dpf = System.Data.Common.DbProviderFactories.GetFactory(dpstr);

				//// 1.DBコネクションオブジェクト作成
				//using (System.Data.Common.DbConnection dbcon = dpf.CreateConnection()) {
				//    dbcon.ConnectionString = string.Format(CONNECTIONSTRING_FORMAT, path);
				//    dbcon.Open();

				//    System.Data.Common.DbCommand command = dpf.CreateCommand();
				//    command.Connection = dbcon;
				//    command.CommandText = query;

				//    string res = command.ExecuteScalar() as string;
				//    dbcon.Close();
				//    return new string[] { res };

				//}

				using (SQLiteConnection sqlConnection = new SQLiteConnection(string.Format(CONNECTIONSTRING_FORMAT, path))) {
					sqlConnection.Open();

					SQLiteCommand command = sqlConnection.CreateCommand();
					command.Connection = sqlConnection;
					command.CommandText = query;
					SQLiteDataReader sdr = command.ExecuteReader();
					List<object> result = new List<object>();
					if (sdr.Read()) {
						for (int i = 0; i < sdr.FieldCount; i++) {
							result.Add(sdr[i]);
						}
					}
					sqlConnection.Close();
					return result.ToArray();
				}

			} catch (Exception ex) {
				Logger.Default.LogException(ex);
			}

			return null;

		}
	}
}
