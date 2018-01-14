using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace MessageTransfer
{
    public class MessageReciever : IDisposable
    {
        #region Private Fields

        private TcpListener _listener;
        private Socket _recieveSocket;
        private readonly int _bufSize;

        #endregion

        public MessageReciever(string strIpAddress, int port, int size = 256)
        {
            IPAddress ipAddress = IPAddress.Parse(strIpAddress);
            _listener = new TcpListener(ipAddress, port);
            _bufSize = size;
        }

        public string Recieve()
        {
            while (true)
            {
                try
                {
                    _listener.Start();

                    // Пришло сообщение
                    _recieveSocket = _listener.AcceptSocket();
                    byte[] bytes = new byte[_bufSize];

                    using (MemoryStream messageStream = new MemoryStream())
                    {
                        do
                        {
                            int recievedBytes = _recieveSocket.Receive(bytes, bytes.Length, 0);
                            messageStream.Write(bytes, 0, recievedBytes);
                        } while (_recieveSocket.Available > 0);

                        _listener.Stop();

                        return Encoding.UTF8.GetString(messageStream.ToArray());
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
