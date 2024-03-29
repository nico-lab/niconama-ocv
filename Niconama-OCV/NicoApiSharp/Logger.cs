﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Hal.NicoApiSharp
{

	/// <summary>
	/// デバッグ情報などを記録するためのクラス
	/// システム全体で分ける必要がない場合はDefaultを使用する
	/// 個別に記述したい場合はインスタンス化して使用する
	/// </summary>
	public class Logger
	{

		/// <summary>
		/// 既定のロガー
		/// </summary>
		private volatile static Logger _defaultLogger = null;
		private static object syncRoot = new Object();

		/// <summary>
		/// 既定のロガー
		/// </summary>
		public static Logger Default
		{
			get
			{
				if (_defaultLogger == null) {
					lock (syncRoot) {
						if (_defaultLogger == null) {
							_defaultLogger = new Logger();
						}
					}
				}

				return _defaultLogger;
			}
		}

		/// <summary>
		/// ログ一覧
		/// </summary>
		public List<LogData> Loglist = new List<LogData>();
		private bool _saveLog = true;
		private bool _hasErrorLog = false;

		/// <summary>
		/// ログを取るかどうかを取得・設定する
		/// </summary>
		[System.Xml.Serialization.XmlIgnoreAttribute()]
		public bool SaveLog
		{
			get { return _saveLog; }
			set { _saveLog = value; }
		}

		/// <summary>
		/// エラーログを保持しているか
		/// </summary>
		[System.Xml.Serialization.XmlIgnoreAttribute()]
		public bool HasErrorLog
		{
			get { return _hasErrorLog; }
		}

		/// <summary>
		/// メッセージを記録する
		/// </summary>
		/// <param name="message"></param>
		public void LogMessage(string message)
		{
			if (_saveLog) {
				LogData data = new LogData(LogData.LogType.Message, message);
				Loglist.Add(data);

				DebugWrite(message);

			}

		}

		/// <summary>
		/// エラーメッセージを記録する
		/// </summary>
		/// <param name="message"></param>
		public void LogErrorMessage(string message)
		{
			if (_saveLog) {
				LogData data = new LogData(LogData.LogType.Error, message);
				Loglist.Add(data);
				_hasErrorLog = true;

				DebugWrite("err:" + message);

			}

		}

		/// <summary>
		/// 例外情報を記録する
		/// </summary>
		/// <param name="ex"></param>
		public void LogException(Exception ex)
		{
			if (_saveLog) {
				string message = string.Format("message:{0}\nstack:{1}", ex.Message, ex.StackTrace);
				LogData data = new LogData(LogData.LogType.Error, message);
				Loglist.Add(data);
				_hasErrorLog = true;

				DebugWrite("err:" + message);

			}

		}

		[System.Diagnostics.ConditionalAttribute("DEBUG")] 
		private void DebugWrite(string message) {

			// MONODevelopでDebug.Writeされた文字がどこに表示されるかよくわからないので関数を作った
			System.Console.WriteLine(message);
		}

		/// <summary>
		/// ログを保存する
		/// </summary>
		/// <param name="path"></param>
		public void Save(string path)
		{
			string log = Utility.XmlSerialize(this, typeof(Logger));
			try {
				using (System.IO.StreamWriter sw = new System.IO.StreamWriter(path, true)) {
					sw.WriteLine(log);
				}
			} catch (Exception ex){
				Console.WriteLine("ログの書き込みに失敗しました。" + ex.Message);
			}
		}

		/// <summary>
		/// ログデータ
		/// </summary>
		public class LogData
		{

			/// <summary>
			/// ログの種類
			/// </summary>
			public enum LogType
			{
				/// <summary>
				/// メッセージ
				/// </summary>
				Message,

				/// <summary>
				/// エラーメッセージ
				/// </summary>
				Error
			}

			/// <summary>
			/// ログの種類
			/// </summary>
			public LogType Type = LogType.Message;

			/// <summary>
			/// ログメッセージ
			/// </summary>
			public string Message;

			/// <summary>
			/// 登録日
			/// </summary>
			public DateTime Date;

			/// <summary>
			/// デフォルトコンストラクタ
			/// </summary>
			public LogData()
			{
				Date = DateTime.Now;
			}

			/// <summary>
			/// ログの内容を指定してログデータを生成する
			/// </summary>
			/// <param name="type"></param>
			/// <param name="message"></param>
			public LogData(LogType type, string message)
				: this()
			{
				this.Type = type;
				this.Message = message;
			}
		}

	}




}
