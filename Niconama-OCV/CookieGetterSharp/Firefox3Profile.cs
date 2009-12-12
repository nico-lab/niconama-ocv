using System;
using System.Collections.Generic;
using System.Text;

namespace Hal.CookieGetterSharp
{

	/// <summary>
	/// Firefox3のプロフィールを表現します。
	/// </summary>
	class Firefox3Profile
	{
		public string name;
		public bool isRelative;
		public string path;
		public bool isDefault;

		/// <summary>
		/// 既定のプロファイルを取得する
		/// </summary>
		/// <returns></returns>
		public static Firefox3Profile GetDefaultProfile(string moz_path, string iniFileName)
		{
			Firefox3Profile[] profs = GetProfiles(moz_path, iniFileName);
			if (profs.Length == 1) {
				return profs[0];
			} else {
				foreach (Firefox3Profile prof in profs) {
					if (prof.isDefault) {
						return prof;
					}
				}
			}

			return null;

		}

		/// <summary>
		/// Firefoxのプロフィールフォルダ内のフォルダをすべて取得する
		/// </summary>
		/// <returns></returns>
		public static Firefox3Profile[] GetProfiles(string moz_path, string iniFileName)
		{
			string profile_path = System.IO.Path.Combine(moz_path, iniFileName);

			List<Firefox3Profile> results = new List<Firefox3Profile>();

			if (System.IO.File.Exists(profile_path)) {
				using (System.IO.StreamReader sr = new System.IO.StreamReader(profile_path)) {
					Firefox3Profile prof = null;
					while (!sr.EndOfStream) {
						string line = sr.ReadLine();

						if (line.StartsWith("[Profile")) {
							prof = new Firefox3Profile();
							results.Add(prof);
						}

						if (prof != null) {
							KeyValuePair<string, string> kvp = getKVP(line);

							switch (kvp.Key) {
								case "Name":
									prof.name = kvp.Value;
									break;
								case "IsRelative":
									prof.isRelative = kvp.Value == "1";
									break;
								case "Path":
									prof.path = kvp.Value.Replace('/', '\\');
									if (prof.isRelative) {
										prof.path = System.IO.Path.Combine(moz_path, prof.path);
									}
									break;
								case "Default":
									prof.isDefault = kvp.Value == "1";
									break;
							}
						}
					}
				}

			}

			return results.ToArray();

		}

		public static KeyValuePair<string, string> getKVP(string line)
		{
			string[] x = line.Split('=');
			if (x.Length == 2) {
				return new KeyValuePair<string, string>(x[0], x[1]);
			}
			return new KeyValuePair<string, string>();
		}
	}
}
