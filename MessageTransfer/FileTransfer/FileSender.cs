using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace FileTransfer
{
    public class FileSender : IDisposable
    {
        #region Private Fields

        private EndPoint _endPoint;
        private readonly Socket _connector;
        private readonly int _bufSize;

        #endregion

        public FileSender(string strIpAddress, int port, int size = 256)
        {
            var ipAddress = IPAddress.Parse(strIpAddress);
            _endPoint = new IPEndPoint(ipAddress, port);
            _connector = new Socket(_endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);    // Создание сокета
            _bufSize = size;
        }

        public void Send(string filename)
        {
            try
            {
                _connector.Connect(_endPoint);

                List<byte> first256 = Encoding.UTF8.GetBytes(filename).ToList<byte>();  // Имя файла
                int diff = 256 - first256.Count;
                for (int i = 0; i < diff; i++)
                    first256.Add(0);

                byte[] readBytes = new byte[_bufSize];
                using (var stream = new FileStream(filename, FileMode.Open))
                {
                    using (var reader = new BinaryReader(stream))
                    {
                        int cursor;

                        _connector.Send(first256.ToArray());    // Передаём сначала имя файла

                        do
                        {
                            cursor = reader.Read(readBytes, 0, readBytes.Length);
                            _connector.Send(readBytes, cursor, SocketFlags.None);
                        } while (cursor == readBytes.Length);
                    }
                }

                _connector.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public void Dispose()
        {
            _connector?.Dispose();
            _endPoint = null;
        }
    }
}
