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

/**
 * Главный класс клиента
 */
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
            Init();

            // Подключение к серверу с спользованием GeneralRemote
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
            }
            catch(Exception)
            {
                Console.WriteLine("Connection to server FAILED");
                Console.Read();
                return;
            }

            // Получение задания с сервера
            var task = remote.GetTaskFromServer();
            GetTaskFile(task.GetTask().Item1, ClientSettings.InputFile1);
            GetTaskFile(task.GetTask().Item2, ClientSettings.InputFile2);

            Console.Read();
        }

        /// <summary>
        /// Обеспечивает получение настроек приложения
        /// </summary>
        private static void Init()
        {
            Ini ini = new Ini("settings.ini");
            ini.Load();
            ClientSettings.Port = Convert.ToInt32(ini.GetValue("Port", "General"));
            ClientSettings.Address = ini.GetValue("Address", "General");
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
