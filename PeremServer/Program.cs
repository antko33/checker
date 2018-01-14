using System;
using System.Net.Sockets;
using System.Threading;

/**
 * Главный класс сервера
 */
namespace PeremServer
{
    class Program
    {
        private const int Port = 1712;
        private static TcpListener _listener;

        private static void Main(string[] args)
        {
            try
            {
                _listener = new TcpListener(System.Net.IPAddress.Any, Port); // Из конфига

                _listener.Start();
                Console.WriteLine("Start");

                while (true)
                {
                    //Новый клиент
                    TcpClient client = _listener.AcceptTcpClient();
                    Client objClient = new Client(client);
                    
                    //Новый поток д/обработки клиента
                    Thread clientThread = new Thread(new ThreadStart(objClient.Process));
                    clientThread.Start();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            //Остановка listener
            _listener?.Stop();
        }
    }
}
