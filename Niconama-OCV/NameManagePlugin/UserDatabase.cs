using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Common;

namespace Hal.NameManagePlugin
{
	class UserDatabase : IDisposable
	{
		const string CONNECTIONSTRING_FORMAT = "Data Source={0}";
		static UserDatabase _SingleObject = null;

		public static UserDatabase GetInstance(string filePath) {
			
			if (_SingleObject == null) {
				lock (typeof(UserDatabase)) {
					if (_SingleObject == null) {
						_SingleObject = new UserDatabase(filePath);
					}
				}
			}

			return _SingleObject;
		}

		DbConnection _connection = null;

		private UserDatabase(string filePath) {
			string dpstr;

			if (Environment.OSVersion.ToString().Contains("Windows")) {
				dpstr = "System.Data.SQLite";
			} else {
				dpstr = "Mono.Data.Sqlite";
			}

			bool existdb = System.IO.File.Exists(filePath);

			// DBプロバイダファクトリ作成
			DbProviderFactory dpf = DbProviderFactories.GetFactory(dpstr);
			_connection = dpf.CreateConnection();
			_connection.ConnectionString = string.Format(CONNECTIONSTRING_FORMAT, filePath);
			_connection.Open();

			if (!existdb) {
				CreateTable();
			}

		}

		/// <summary>
		/// ユーザーテーブルを作成する
		/// </summary>
		private void CreateTable()
		{

			// トランザクション開始
			using (DbTransaction tran = _connection.BeginTransaction())
			// Create用コマンドオブジェクト作成
			using (DbCommand cusrcmd = _connection.CreateCommand()) 
			using (DbCommand ccomcmd = _connection.CreateCommand()) {

				string makeUserStr = "CREATE TABLE user (uid VARCHAR(32), color INTEGER NOT NULL, name VARCHAR(64), date INTEGER NOT NULL, PRIMARY KEY (uid))";
				string makeComStr = "CREATE TABLE com_user (uid VARCHAR(32), cid VARCHAR(16), PRIMARY KEY (cid, uid))";

				cusrcmd.CommandText = makeUserStr;
				ccomcmd.CommandText = makeComStr;
				cusrcmd.Transaction = tran;
				ccomcmd.Transaction = tran;

				try {
					
					cusrcmd.ExecuteNonQuery();
					ccomcmd.ExecuteNonQuery();

					// コミット
					tran.Commit();

				} catch {
					// ロールバック
					tran.Rollback();
					throw;
				}
			}

		}


		public List<User> GetUsers(string comid) {

			// Select用コマンドオブジェクト作成
			using (DbCommand scmd = _connection.CreateCommand()) {

				string select = "SELECT uid, name, color, date FROM user NATURAL INNER JOIN com_user WHERE com_user.cid = @cid";
				scmd.CommandText = select;

				DbParameter p1 = scmd.CreateParameter();
				p1.ParameterName = "@cid";
				p1.Value = comid;
				scmd.Parameters.Add(p1);

				using (DbDataReader dr = scmd.ExecuteReader()) {
					List<User> users = new List<User>();
					
					while (dr.Read()) { 
						User u = new User((string)dr["uid"], (string)dr["name"], (int)(long)dr["color"], (long)dr["date"]);
						users.Add(u);
					}

					return users;
				}
			}
		}

		public void UpdateUsers(string cid, IEnumerable<User> users) {
			// トランザクション開始
			using (DbTransaction tran = _connection.BeginTransaction())
			// Create用コマンドオブジェクト作成
			using (DbCommand cmd = _connection.CreateCommand()) {

				string iUserCommand = "INSERT INTO user(uid, name, color, date) VALUES(@uid, @name, @color, @date)";
				string iCommCommand = "INSERT INTO com_user(uid, cid) VALUES(@uid, @cid)";
				string uUserCommand = "UPDATE user SET name = @name, color = @color, date = @date WHERE uid = @uid";
				string dUserCommand = "DELETE FROM user WHERE uid = @uid";
				string dCommCommand = "DELETE FROM user_com WHERE uid = @uid";

				DbParameter pUid = cmd.CreateParameter();
				pUid.ParameterName = "@uid";

				DbParameter pName = cmd.CreateParameter();
				pName.ParameterName = "@name";

				DbParameter pColor = cmd.CreateParameter();
				pColor.ParameterName = "@color";

				DbParameter pDate = cmd.CreateParameter();
				pDate.ParameterName = "@date";

				DbParameter pCid = cmd.CreateParameter();
				pCid.ParameterName = "@cid";
				pCid.Value = cid;

				cmd.Parameters.Add(pUid);
				cmd.Parameters.Add(pName);
				cmd.Parameters.Add(pColor);
				cmd.Parameters.Add(pDate);
				cmd.Parameters.Add(pCid);
				cmd.Transaction = tran;

				try {
					foreach (User user in users) {
						if (user.State != User.UserState.None) {
							pUid.Value = user.Id;
							pName.Value = user.Name;
							pColor.Value = user.Color.ToArgb();
							pDate.Value = user.LastCommentDate.Ticks;

							switch (user.State) {
								case User.UserState.New:
									cmd.CommandText = iUserCommand;
									cmd.ExecuteNonQuery();
									cmd.CommandText = iCommCommand;
									cmd.ExecuteNonQuery();
									break;
								case User.UserState.Update:
									cmd.CommandText = uUserCommand;
									cmd.ExecuteNonQuery();
									break;
								case User.UserState.Delete:
									cmd.CommandText = dUserCommand;
									cmd.ExecuteNonQuery();
									cmd.CommandText = dCommCommand;
									cmd.ExecuteNonQuery();
									break;
							}
						}
					}

					tran.Commit();
				} catch {
					// ロールバック
					tran.Rollback();
					throw;
				}
			}
		}

		#region IDisposable メンバ

		public void Dispose()
		{
			lock (typeof(UserDatabase)) {
				if (_connection != null) {
					_connection.Close();
					_connection.Dispose();
					_connection = null;
				}

				_SingleObject = null;
			}
		}

		#endregion
	}
}
