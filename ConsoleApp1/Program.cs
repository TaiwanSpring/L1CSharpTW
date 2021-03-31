using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace ConsoleApp1
{
	class Program
	{
		static void Main(string[] args)
		{
			const string PATH = @"C:\GithubSourceCode\l1j-tw-99nets\L1J-TW_3.80c\data\xml\NpcActions\ItemMaking.xml";

			XmlDocument xmlDocument = new XmlDocument();

			xmlDocument.Load(PATH);

			PrintNode(xmlDocument.DocumentElement);

			void PrintNode(XmlElement xmlElement)
			{
				for (int i = 0; i < xmlElement.ChildNodes.Count; i++)
				{
					if (xmlElement.ChildNodes[i] is XmlElement element)
					{
						Debug.Print(element.Name);
						PrintNode(element);
					}
				}
			}

			DirectoryInfo directoryInfo = new DirectoryInfo(PATH);

			if (directoryInfo.Exists)
			{

				foreach (var item in directoryInfo.GetFiles())
				{
					if (item.Extension == ".txt")
					{
						string mapId = item.Name.Remove(item.Name.Length - item.Extension.Length);
						byte[] buffer = File.ReadAllBytes(item.FullName);
						byte value = 0;
						List<List<byte>> result = new List<List<byte>>(ushort.MaxValue);
						List<byte> currentLine = new List<byte>(ushort.MaxValue);
						for (int i = 0; i < buffer.Length; i++)
						{
							if (buffer[i] == ',')
							{
								currentLine.Add(value);
								value = 0x00;
							}
							else if (buffer[i] >= '0' && buffer[i] <= '9')
							{
								value = (byte)( value * 10 + ( buffer[i] - '0' ) );
							}
							else if (buffer[i] == 0x0A)//換行
							{
								result.Add(currentLine);
								currentLine = new List<byte>(ushort.MaxValue);
							}
						}
					}
				}
			}

			FileInfo fileInfo = new FileInfo(PATH);

			if (fileInfo.Exists)
			{
				XmlSerializer serializer =
	   new XmlSerializer(typeof(TreasureBoxList));

				// Declare an object variable of the type to be deserialized.
				using (XmlReader xmlReader = XmlReader.Create(fileInfo.OpenText()))
				{
					XmlSerializer xmlSerializer = new XmlSerializer(typeof(TreasureBoxList));
					TreasureBoxList treasureBoxList = xmlSerializer.Deserialize(xmlReader) as TreasureBoxList;
				}


			}
			string connectString = $"server=localHost;uid=root;pwd=753951;database=l1jdbtw";
			MySqlConnection mySqlConnection = new MySqlConnection(connectString);
			mySqlConnection.Open();

			var comm = mySqlConnection.CreateCommand();
			comm.CommandText = "SELECT * FROM accounts";
			var reader = comm.ExecuteReader();
			IDictionary<string, object> dataTable = new Dictionary<string, object>();

			if (reader.HasRows)
			{
				while (reader.Read())
				{
					for (int i = 0; i < reader.FieldCount; i++)
					{
						dataTable.Add(reader.GetName(i), reader.GetValue(i));
					}
				}
			}

			//GameServer gameServer = new GameServer();
			//Task.Run(() => gameServer.StartServer());

			//Console.WriteLine("Server start.");
			//Console.WriteLine("Press any key to stop.");
			Console.ReadLine();
		}
	}

	enum Opcode : byte
	{
		S_OPCODE_INITPACKET = 150,
	}
	class GameServer
	{
		readonly TcpListener tcpListener = new TcpListener(IPAddress.Any, 2000);

		internal void StartServer()
		{
			this.tcpListener.Start();
			this.tcpListener.BeginAcceptSocket(OnAccept, this.tcpListener);
		}

		private void OnAccept(IAsyncResult ar)
		{
			if (ar.IsCompleted)
			{
				if (ar.AsyncState is TcpListener server)
				{
					try
					{
						TcpClient tcpClient = server.EndAcceptTcpClient(ar);
						GameClient gameClient = new GameClient(tcpClient);
						gameClient.Initialize();
					}
					catch (Exception)
					{

					}
				}
			}

			this.tcpListener.BeginAcceptSocket(OnAccept, this.tcpListener);
		}
	}



	/// <summary>
	/// 
	/// </summary>
	/// <param name="hostName">資料庫位址</param>
	/// <param name="user">資料庫使用者名稱</param>
	/// <param name="password">資料庫使用者密碼</param>
	/// <param name="dbName">資料庫名稱</param>
	/// <returns></returns>
	//public bool Initialize(string hostName, string user, string password, string dbName)
	//{
	//    this.connectString = $"server={hostName};uid={user};pwd={password};database={dbName}";
	//    try
	//    {
	//        MySqlConnection connection = new MySqlConnection(this.connectString);
	//        connection.Open();

	//        Connection = new DataBaseConnectionImp(connection);
	//    }
	//    catch (MySqlException ex)
	//    {
	//        switch (ex.Number)
	//        {
	//            case 0:
	//                System.Console.WriteLine("無法連線到資料庫.");
	//                break;
	//            case 1045:
	//                System.Console.WriteLine("使用者帳號或密碼錯誤,請再試一次.");
	//                break;
	//        }
	//        return false;
	//    }
	//    catch (Exception e)
	//    {
	//        //_log.fine("Database Connection FAILED");
	//        System.Console.WriteLine("could not init DB connection:" + e.Message);
	//        return false;
	//    }
	//    return true;
	//}

}
