//using ConsoleApp1.Model;
//using System;
//using System.Collections.Concurrent;
//using System.Collections.Generic;
//using System.IO;
//using System.Net.Sockets;
//using System.Threading.Tasks;

//namespace ConsoleApp1
//{
//    class GameClient
//    {
//        readonly Cipher cipher;
//        static readonly HashSet<GameClient> refSet = new HashSet<GameClient>();
//        static readonly Random random = new Random();
//        int key;
//        const int dataPackageLength = 1024;
//        //3.8 first packet
//        static readonly byte[] firstPacket = new byte[]
//        {
//            0x9d,
//            0xd1,
//            0xd6,
//            0x7a,
//            0xf4,
//            0x62,
//            0xe7,
//            0xa0,
//            0x66,
//            0x02,
//            0xfa
//       };
//        readonly TcpClient tcpClient;
//        NetworkStream networkStream;

//        readonly ConcurrentQueue<byte[]> sendPacketQueue;

//        public GameClient(TcpClient tcpClient)
//        {
//            // 採取亂數取seed
//            this.key = random.Next(int.MaxValue);
//            this.cipher = new Cipher(this.key);
//            this.tcpClient = tcpClient;
//            this.sendPacketQueue = new ConcurrentQueue<byte[]>();
//        }

//        public void Initialize()
//        {
//            if (this.tcpClient.Connected)
//            {
//                refSet.Add(this);

//                this.networkStream = this.tcpClient.GetStream();

//                this.sendPacketQueue.Enqueue(MakeFirstPacket(this.key));

//                BeginSendServerPacket();

//                BeginReceiveClientPacket();
//            }
//        }

//        private void BeginReceiveClientPacket()
//        {


//            if (this.networkStream.CanRead)
//            {
//                Task.Run(() =>
//                {
//                    byte[] header = new byte[2];

//                    while (true)
//                    {
//                        try
//                        {
//                            int length = this.networkStream.Read(header, 0, header.Length);
//                            if (length == header.Length)
//                            {
//                                int dataLength = (header[1] << 8) + header[0] - 2;
//                                byte[] data = new byte[dataLength];
//                                this.networkStream.Read(data, 0, data.Length);
//                                if (data.Length > 0)
//                                {
//                                    HandlerClientPacket(data);
//                                }
//                            }
//                        }
//                        catch (Exception)
//                        {
//                            break;
//                        }
//                    }

//                    lock (refSet)
//                    {
//                        refSet.Remove(this);
//                    }
//                });
//            }
//        }

//        private void HandlerClientPacket(byte[] buffer)
//        {
//            buffer = this.cipher.Decrypt(buffer);
//            //MemoryStream memoryStream = new MemoryStream(buffer);
//            byte opcode = buffer[0];


//        }

//        public static void SendServerPacketToAll(byte[] buffer)
//        {
//            lock (refSet)
//            {
//                foreach (var item in refSet)
//                {
//                    item.SendServerPacket(buffer);
//                }
//            }
//        }

//        public void SendServerPacket(byte[] buffer)
//        {
//            this.sendPacketQueue.Enqueue(buffer);
//        }

//        private void BeginSendServerPacket()
//        {
//            Task.Run(() =>
//            {
//                while (this.tcpClient.Connected)
//                {
//                    if (!this.sendPacketQueue.IsEmpty)
//                    {
//                        if (this.sendPacketQueue.TryDequeue(out byte[] buffer))
//                        {
//                            try
//                            {
//                                this.networkStream.Write(buffer, 0, buffer.Length);
//                                this.networkStream.Flush();
//                            }
//                            catch (Exception e)
//                            {
//                                break;
//                            }

//                        }
//                    }
//                }
//                lock (refSet)
//                {
//                    if (refSet.Remove(this))
//                    {
//                    }
//                }
//            });
//        }

//        private byte[] MakeFirstPacket(int key)
//        {
//            int bogus = firstPacket.Length + 7;
//            MemoryStream memoryStream = new MemoryStream();
//            memoryStream.WriteByte(bogus.GetLow());
//            memoryStream.WriteByte(bogus.GetHigh());
//            memoryStream.WriteByte((byte)Opcode.S_OPCODE_INITPACKET); // 3.7C Taiwan Server
//            memoryStream.Write(BitConverter.GetBytes(key));
//            memoryStream.Write(firstPacket, 0, firstPacket.Length);
//            byte[] buffer = memoryStream.GetBuffer();
//            memoryStream.Close();
//            return buffer;
//        }
//    }
//}
