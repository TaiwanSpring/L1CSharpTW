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
}
