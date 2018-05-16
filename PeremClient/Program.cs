using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Xml;
using PeremClient.Class;
using GeneralRemote;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.IO;

/// <summary>
/// Главный класс клиента
/// </summary>
namespace PeremClient
{
    /// <summary>
    /// Представляет набор свойств класса клиентского приложения
    /// </summary>
    internal static class ClientSettings
    {
        //Из конфига
        internal static int Port;
        internal static string Address;
        internal static string RemName;
        internal static string InputFile1;
        internal static string InputFile2;
        internal static string GeneralInputFile1;
        internal static string GeneralInputFile2;
        internal static string PathToFiles;
    }

    internal static partial class Program
    {
        public static void Main(string[] args)
        {
            Console.Write("Initializing... ");
            Init();
            Console.WriteLine("Success");

            // Подключение к серверу с спользованием GeneralRemote
            Console.Write("Connecting to server... ");
            TcpChannel tcpChannel = new TcpChannel();
            ChannelServices.RegisterChannel(tcpChannel, false);

            GeneralRemoteClass remote = null;

            try
            {
                remote =
                  (GeneralRemoteClass)Activator.GetObject(
                      typeof(GeneralRemoteClass),
                      String.Format("tcp://{0}:{1}/{2}", ClientSettings.Address, ClientSettings.Port, ClientSettings.RemName));

                string host = System.Net.Dns.GetHostName();
                remote.SendToServer(host + " connected");
                Console.WriteLine("Success");

                // Получение задания с сервера
                Console.Write("Recieving task... ");
                var task = remote.GetTaskFromServer();
                GetTaskFile(task.GetTask().Item1, ClientSettings.InputFile1);
                GetTaskFile(task.GetTask().Item2, ClientSettings.InputFile2);

                var model = remote.GetModelFromServer();
                GetTaskFile(model.GetTask().Item1, ClientSettings.GeneralInputFile1);
                GetTaskFile(model.GetTask().Item2, ClientSettings.GeneralInputFile2);

                Console.WriteLine("Success");
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                Console.Read();
                return;
            }

            Console.WriteLine("Parsing...");
            CoordsPU = Parser.ParseCoords(ClientSettings.InputFile1);
            Console.WriteLine("\tPU coords parsed");
            DeltasPU = Parser.ParseDeltas(ClientSettings.InputFile2);
            Console.WriteLine("\tPU deltas parsed");
            CoordsMain = Parser.ParseCoords(ClientSettings.GeneralInputFile1);
            Console.WriteLine("\tModel coords parsed");
            DeltasMain = Parser.ParseDeltas(ClientSettings.GeneralInputFile2);
            Console.WriteLine("\tModel deltas parsed");
            Console.WriteLine("Success");

            Console.Write("Checking... ");
            Check();
            Console.WriteLine("Success");
            Console.WriteLine(WrongNodes.Count.ToString());

            Console.Read();
        }

        /// <summary>
        /// Обеспечивает получение настроек приложения
        /// </summary>
        private static void Init()
        {
            Ini ini = new Ini("settings.ini");
            ClientSettings.Port = Convert.ToInt32(ini.GetValue("Port", "General"));
            ClientSettings.Address = ini.GetValue("Address", "General", "localhost");
            ClientSettings.RemName = ini.GetValue("RemName", "General");
            ClientSettings.PathToFiles = ini.GetValue("PathToFiles", "General");
            ClientSettings.InputFile1 = Path.Combine
                (ClientSettings.PathToFiles,
                ini.GetValue("InputFile1", "Project Units Files"));
            ClientSettings.InputFile2 = Path.Combine
                (ClientSettings.PathToFiles,
                ini.GetValue("InputFile2", "Project Units Files"));
            ClientSettings.GeneralInputFile1 = Path.Combine
                (ClientSettings.PathToFiles,
                ini.GetValue("GeneralInputFile1", "Model Files"));
            ClientSettings.GeneralInputFile2 = Path.Combine
                (ClientSettings.PathToFiles,
                ini.GetValue("GeneralInputFile2", "Model Files"));
        }

        private static void GetTaskFile(PeremServer.TaskItem item, string file)
        {
            using (StreamReader sr = new StreamReader(item.GetFileStream()))
            {
                string l = sr.ReadToEnd();
                using (StreamWriter sw = new StreamWriter(Path.Combine(Directory.GetCurrentDirectory(), file)))
                {
                    sw.Write(l);
                }
            }
        }
    }
}
