using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Hal.NicoApiSharp.Cookie
{
	class OperaCookieGetter : CookieGetter
	{
		const byte MSB = 0x80;

		private struct Header
		{
			public int file_version_number;
			public int app_version_number;
			public int idtag_length;
			public int length_length;
		}

		private struct Record
		{
			// application specific tag to identify content type
			public int tag_id;
			// length of payload
			public int length;
			// Payload/content of the record
			public byte[] bytepayload;
		};

		public static new ICookieGetter GetInstance(Cookie.CookieGetter.BROWSER_TYPE type)
		{
			switch (type) {
				case CookieGetter.BROWSER_TYPE.Opera10:
					return new OperaCookieGetter();
			}

			return null;
		}

		private OperaCookieGetter()
		{ 
			base._defaultPath = Utility.ReplacePathSymbols(ApiSettings.Default.Opera10CookieFilePath);
		}

		public override System.Net.CookieContainer GetAllCookies(string path)
		{
			System.Net.CookieContainer container = new System.Net.CookieContainer();
			if (!File.Exists(path)) return container;  // �t�@�C���̗L�����`�F�b�N

			try {
				using (FileStream reader = new FileStream(path, FileMode.Open, FileAccess.Read)) {
					// �w�肵���A�h���X�ɓǂݍ��݈ʒu���ړ�
					reader.Seek(0, SeekOrigin.Begin);
					Header headerData = getHeader(reader);
					Record recordData;
					Stack<string> domainStack = new Stack<string>();
					Stack<string> pathStack = new Stack<string>();

					//version check
					if ((headerData.file_version_number & 0xfffff000) == 0x00001000) {

						while (reader.Position < reader.Length) {
							recordData = getRecord(reader, headerData);
							switch (recordData.tag_id) {
								case 0x01:  // �h���C��
									string domain = getDomainRecode(new System.IO.MemoryStream(recordData.bytepayload), headerData);
									if (domain != null) {
										domainStack.Push(domain);
									}
									break;
								case 0x02:  // �p�X
									string page = getPageRecode(new System.IO.MemoryStream(recordData.bytepayload), headerData);
									if (page != null) {
										pathStack.Push(page);
									}
									break;
								case 0x03:  // �N�b�L�[
									string chost = string.Join(".", domainStack.ToArray());
									string cpath = '/' + string.Join("/", pathStack.ToArray());
									
									System.Net.Cookie cookie = getCookieRecode(new System.IO.MemoryStream(recordData.bytepayload), headerData);
									cookie.Domain = '.' + string.Join(".", domainStack.ToArray());
									cookie.Path = '/' + string.Join("/", pathStack.ToArray());
									container.Add(cookie);

									break;
								case 0x04 + MSB: //�h���C���I��
									if (0 < domainStack.Count) {
										domainStack.Pop();
									}
									break;
								case 0x05 + MSB: //�p�X�I��
									if (0 < pathStack.Count) {
										pathStack.Pop();
									}
									break;
							}
						}
					}
				}
			} catch (Exception ex) {
				Logger.Default.LogErrorMessage("");
				Logger.Default.LogException(ex);

			}

			return container;
		}

		private string getDomainRecode(System.IO.Stream stream, Header headerData)
		{
			Record recordData;

			while ( stream.Position < stream.Length ) {
				recordData = getRecord(stream, headerData);

				switch (recordData.tag_id) {
					case 0x1e:  // Domain Name
						return Encoding.ASCII.GetString(recordData.bytepayload);
				}
			}

			return null;
		}

		private string getPageRecode(System.IO.Stream stream, Header headerData)
		{
			Record recordData;

			while (stream.Position < stream.Length) {
				recordData = getRecord(stream, headerData);

				switch (recordData.tag_id) {
					case 0x1d:  // Page Name
						return Encoding.ASCII.GetString(recordData.bytepayload);
				}
			}

			return null;
		}

		private System.Net.Cookie getCookieRecode(System.IO.Stream stream, Header headerData)
		{
			Record recordData;
			System.Net.Cookie cookie = new System.Net.Cookie();

			while (stream.Position < stream.Length) {

				recordData = getRecord(stream, headerData);

				switch (recordData.tag_id) {
					case 0x10:  // Cookie Name
						cookie.Name = Encoding.ASCII.GetString(recordData.bytepayload);
						break;
					case 0x11:  // Cookie Value
						cookie.Value = Encoding.ASCII.GetString(recordData.bytepayload);
						break;
					case 0x12:
						long time = getNumber(new System.IO.MemoryStream(recordData.bytepayload), 8);
						cookie.Expires = Utility.UnixTimeToDateTime((int)time);
						break;
				}
			}

			return cookie;
		}

		private Header getHeader(System.IO.Stream stream)
		{
			Header headerData = new Header();

			headerData.file_version_number = (int)getNumber(stream, 4);
			headerData.app_version_number = (int)getNumber(stream, 4);
			headerData.idtag_length = (int)getNumber(stream, 2);
			headerData.length_length = (int)getNumber(stream, 2);

			return headerData;
		}

		private Record getRecord(System.IO.Stream stream, Header headerData)
		{
			Record recordData = new Record();
			int topData = stream.ReadByte();
			stream.Seek(-1, SeekOrigin.Current);
			recordData.tag_id = (int)getNumber(stream, headerData.idtag_length);
			
			// MSB��ON�̂Ƃ��ATag ID�݂̂ɂȂ�B�i�ȍ~��DataLength,Data�͂Ȃ��j
			if ((topData & MSB) == 0) {

				recordData.length = (int)getNumber(stream, headerData.length_length);
				recordData.bytepayload = new byte[recordData.length];
				stream.Read(recordData.bytepayload, 0, recordData.length);
			}

			return recordData;
		}

		private long getNumber(System.IO.Stream stream, int length)
		{
			long n = 0;
			for (int i = 0; i < length; i++) {
				n <<= 8;
				n += stream.ReadByte();
			}
			return n;
		}

	}
}