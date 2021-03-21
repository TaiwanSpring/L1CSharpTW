using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            GameServer gameServer = new GameServer();
            Task.Run(() => gameServer.StartServer());

            Console.WriteLine("Server start.");
            Console.WriteLine("Press any key to stop.");
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
