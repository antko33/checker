using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace FileTransfer
{
    public class FileReciever : IDisposable
    {
        #region Private Fields

        private TcpListener _listener;
        private Socket _recieveSocket;
        private readonly int _bufSize;

        #endregion

        public FileReciever(string strIpAddress, int port, int size = 256)
        {
            IPAddress ipAddress = IPAddress.Parse(strIpAddress);
            _listener = new TcpListener(ipAddress, port);
            _bufSize = size;
        }

        public void Recieve()
        {
            while (true)
            {
                try
                {
                    _listener.Start();

                    _recieveSocket = _listener.AcceptSocket();

                    byte[] bytes = new byte[_bufSize];

                    using (MemoryStream data = new MemoryStream())
                    {
                        int recievedBytes;
                        int first256 = 0;   // Первые 256 байт
                        string path = "";

                        do
                        {
                            recievedBytes = _recieveSocket.Receive(bytes, bytes.Length, 0);

                            if (first256 < 256)
                            {
                                first256 += recievedBytes;
                                byte[] toStr = bytes;

                                if (first256 > 256)
                                {
                                    int start = first256 - recievedBytes;
                                    int toGet = 256 - start;

                                    first256 = 256;

                                    toStr = bytes.Take(toGet).ToArray();
                                    bytes = bytes.Skip(toGet).ToArray();

                                    data.Write(bytes, 0, recievedBytes);
                                }

                                path = Encoding.UTF8.GetString(toStr);
                            }
                            else
                            {
                                data.Write(bytes, 0, recievedBytes);
                            }
                        } while (recievedBytes == data.Length);

                        string clearPath = path.Substring(0, path.IndexOf('\0'));

                        using (FileStream file = new FileStream(clearPath, FileMode.Create))
                        {
                            file.Write(data.ToArray(), 0, data.ToArray().Length);
                            Console.WriteLine("File {0} recieved", clearPath);
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }

        public void Dispose()
        {
            _recieveSocket?.Dispose();
            _listener = null;
        }
    }
}
