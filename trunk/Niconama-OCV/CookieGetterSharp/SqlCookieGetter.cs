using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SQLite;

namespace Hal.CookieGetterSharp
{
	/// <summary>
	/// SQLite�𗘗p���ăN�b�L�[��ۑ�����^�C�v�̃u���E�U����N�b�L�[���擾����N���X
	/// </summary>
	abstract class SqlCookieGetter : CookieGetter
	{
		const string CONNECTIONSTRING_FORMAT = "Data Source={0}";

		public override System.Net.Cookie GetCookie(Uri url, string key)
		{
			System.Net.CookieContainer container = GetCookies(base.CookiePath, MakeQuery(url, key));
			System.Net.CookieCollection collection = container.GetCookies(Utility.AddSrashLast(url));
			return collection[key];
		}

		public override System.Net.CookieCollection GetCookieCollection(Uri url)
		{
			System.Net.CookieContainer container = GetCookies(base.CookiePath, MakeQuery(url));
			return container.GetCookies(Utility.AddSrashLast(url));
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
							Utility.AddCookieToContainer(container, cookie);
						} catch {
							Console.WriteLine(string.Format("Invalid Format! domain:{0},key:{1},value:{2}", cookie.Domain, cookie.Name, cookie.Value));
						}

					}

					sqlConnection.Close();
				}

			} catch (Exception ex) {
				throw new CookieGetterException("�N�b�L�[���擾���ASqlite�A�N�Z�X�ŃG���[���������܂����B", ex);
			} finally {
				if (temp != null) {
					System.IO.File.Delete(temp);
				}
			}

			return container;
		}

		/// <summary>
		/// SQL����擾�����f�[�^���N�b�L�[�ɕϊ�����
		/// </summary>
		/// <param name="data">�w�肳�ꂽQuery�Ŏ擾�����P�s���̃f�[�^</param>
		/// <returns></returns>
		protected abstract System.Net.Cookie DataToCookie(object[] data);

		/// <summary>
		/// ���ׂẴN�b�L�[���擾���邽�߂̃N�G���[�𐶐�����
		/// </summary>
		/// <returns></returns>
		protected abstract string MakeQuery();

		/// <summary>
		/// �w�肳�ꂽURL�Ɋ֘A�����N�b�L�[���擾���邽�߂̃N�G���[�𐶐�����
		/// </summary>
		/// <param name="url"></param>
		/// <returns></returns>
		protected abstract string MakeQuery(Uri url);

		/// <summary>
		/// �w�肳�ꂽURL�̖��O��key�ł���N�b�L�[���擾���邽�߂̃N�G���[�𐶐�����
		/// </summary>
		/// <param name="url"></param>
		/// <param name="key"></param>
		/// <returns></returns>
		protected abstract string MakeQuery(Uri url, string key);
	}
}
