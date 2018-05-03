using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Xml;
using PeremClient.Class;
using GeneralRemote;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Http;
using System.IO;

/**
 * Главный класс клиента
 */
namespace PeremClient
{
    internal static partial class Program
    {
        //Из конфига
        private const int Port = 1712;
        private const string Address = "127.0.0.1";
        private const string pathMainCoords = "";

        public static void Main(string[] args)
        {
            HttpChannel channel = new HttpChannel();
            ChannelServices.RegisterChannel(channel, false);
            
            GeneralRemoteClass remote =
                (GeneralRemoteClass)Activator.GetObject(
                    typeof(GeneralRemoteClass),
                    "http://localhost:32321/checker.soap");

            remote.SendToServer("aaa");
            var finfo = remote.GetTaskFromServer();
            return;

            /*TcpClient client = null;
            try
            {
                client = new TcpClient(Address, Port); //Клиентские адрес и порт указать
                NetworkStream stream = client.GetStream();

                while (true)
                {
                    //Проба
                    string message = Console.ReadLine();
                    byte[] data = Encoding.Unicode.GetBytes(message);
                    stream.Write(data, 0, data.Length);

                    data = new byte[64];
                    StringBuilder builder = new StringBuilder();
                    int bytes = 0;
                    do
                    {
                        bytes = stream.Read(data, 0, data.Length);
                        builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                    } while (stream.DataAvailable);

                    message = builder.ToString();
                    Console.WriteLine("Server: {0}", message);
                    /**
                     * <TODO>
                     * 1. Готовность -> сервер отправляет задание
                     * 2. Решаем
                     * 3. Генерим отчёт и посылаем на сервак
                     * 
                     * Как происходит решение:
                     * 1. Считывание ко-т узлов модели в список
                     * 2. Берём очередной узел из ПЕ
                     * 3. Ишем в модели узел с соответствующими ко-тами
                     * 
                     * </TODO>
                     * 
                     * КОДЫ ОШИБОК:
                     * 0 - всё хорошо
                     * 1 - файл координат узлов модели не найден
                     * 2 - файл перемещений в узлах модели не найден
                     * 3 - файл координат узлов ПЕ не найден
                     * 4 - файл перемещений в узлах ПЕ не найден
                     * 5 - файл координат узлов модели неверен
                     * 6 - файл перемещений в узлах модели неверен
                     * 7 - файл координат узлов ПЕ неверен
                     * 8 - файл перемещений в узлах ПЕ неверен
                     */
            /*}
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }
        finally
        {
            client?.Close();
        }*/

            /*coordsMain = Parser.ParseCoords("0.csv");
            coordsPU = Parser.ParseCoords("1.csv");
            deltasMain = Parser.ParseDeltas("7.0");
            deltasPU = Parser.ParseDeltas("7.1");
            wrongNodes = new List<NodePair>(); //Костыль!
            Check();
            Console.Write("okay {0} {1} {2} {3}", coordsMain.Count, coordsPU.Count, deltasMain[1][31].X, deltasPU[12][7].Z);*/
            Console.Read();
        }
    }
}
