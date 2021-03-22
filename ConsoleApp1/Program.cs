using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
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
    public bool Initialize(string hostName, string user, string password, string dbName)
    {
        this.connectString = $"server={hostName};uid={user};pwd={password};database={dbName}";
        try
        {
            MySqlConnection connection = new MySqlConnection(this.connectString);
            connection.Open();

            Connection = new DataBaseConnectionImp(connection);
        }
        catch (MySqlException ex)
        {
            switch (ex.Number)
            {
                case 0:
                    System.Console.WriteLine("無法連線到資料庫.");
                    break;
                case 1045:
                    System.Console.WriteLine("使用者帳號或密碼錯誤,請再試一次.");
                    break;
            }
            return false;
        }
        catch (Exception e)
        {
            //_log.fine("Database Connection FAILED");
            System.Console.WriteLine("could not init DB connection:" + e.Message);
            return false;
        }
        return true;
    }

}
