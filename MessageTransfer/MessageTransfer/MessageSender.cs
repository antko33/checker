using System;
using System.Data;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace MessageTransfer
{
    public class MessageSender : IDisposable
    {
        #region Private Fields

        private EndPoint _endPoint;
        private readonly Socket _connector;

        #endregion

        public MessageSender(string strIpAddress, int port)
        {
            var ipAddress = IPAddress.Parse(strIpAddress);
            _endPoint = new IPEndPoint(ipAddress, port);
            _connector = new Socket(_endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);    // Создание сокета
        }

        public void Send(object message)
        {
            try
            {
                string strMessage;
                if (message is string)
                {
                    strMessage = message as string;
                }
                else
                {
                    throw new DataException("На вход необходимо подать строку");
                }

                _connector.Connect(_endPoint);

                byte[] sendBytes = Encoding.UTF8.GetBytes(strMessage);

                _connector.Send(sendBytes);
                _connector.Close();
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void Dispose()
        {
            _connector?.Dispose();
            _endPoint = null;
        }
    }
}
