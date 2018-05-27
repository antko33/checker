using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Threading.Tasks;
using GeneralRemote;
using PeremClient.Class;

namespace PeremClient
{
    /// <summary>
    /// Представляет набор свойств класса клиентского приложения
    /// </summary>
    internal static class ClientSettings
    {
        // Из конфига
        internal static int Port { get; set; }
        internal static string Address { get; set; }
        internal static string RemName { get; set; }
        internal static string InputFile1 { get; set; }
        internal static string InputFile2 { get; set; }
        internal static string GeneralInputFile1 { get; set; }
        internal static string GeneralInputFile2 { get; set; }
        internal static string PathToFiles { get; set; }
        internal static int PUNumber;
    }

    /// <summary>
    /// Главный класс клиентского приложения
    /// </summary>
    internal static partial class Program
    {
        static GeneralRemoteClass remote = null;
        static string host;    // Имя компьютера
        static GeneralRemote.Task task, model = null;   // Написал с именем пространства имён, т. к. неоднозначная ссылка (конфликт с Threading, плохо!)

        public static void Main(string[] args)
        {
            Console.Write("Initializing... ");
            Init();
            Console.WriteLine("Success");

            try
            {
                Process p = Process.Start("ClientMonitor.exe", String.Join("\\//", ClientSettings.Address, ClientSettings.Port.ToString(), ClientSettings.RemName));
            }
            catch
            {
                Console.WriteLine("Не удалось запустить монитор процесса. Выполнение будет продолжено...");
            }

            // Подключение к серверу с спользованием GeneralRemote
            Console.Write("Connecting to server... ");
            TcpChannel tcpChannel = new TcpChannel();
            ChannelServices.RegisterChannel(tcpChannel, false);

            try
            {
                remote =
                  (GeneralRemoteClass)Activator.GetObject(
                      typeof(GeneralRemoteClass),
                      String.Format("tcp://{0}:{1}/{2}", ClientSettings.Address, ClientSettings.Port, ClientSettings.RemName));

                host = System.Net.Dns.GetHostName();
                remote.SendToServer(host + " connected");
                Console.WriteLine("Success");
            }
            catch
            {
                Console.WriteLine("Не удалось подключиться к серверу");
                return;
            }

            // Получение задания с сервера
            Console.Write("Recieving task... ");
            try
            {
                task = remote.GetTaskFromServer(out ClientSettings.PUNumber);   // "Побочный эффект" - возврат номера ПЕ
                model = remote.GetModelFromServer();
            }
            catch
            {
                Console.WriteLine("Не удалось получить задание: очередь заданий пуста");
                return;
            }

            GetTaskFile(task.GetTask().Item1, ClientSettings.InputFile1);
            GetTaskFile(task.GetTask().Item2, ClientSettings.InputFile2);

            GetTaskFile(model.GetTask().Item1, ClientSettings.GeneralInputFile1);
            GetTaskFile(model.GetTask().Item2, ClientSettings.GeneralInputFile2);

            Console.WriteLine("Success");
                        
            Parallel.Invoke(
                () =>
                {
                    CoordsPU = Parser.ParseCoords(ClientSettings.InputFile1);
                    Console.WriteLine("\tPU coords parsed");
                },
                () =>
                {
                    DeltasPU = Parser.ParseDeltas(ClientSettings.InputFile2);
                    Console.WriteLine("\tPU coords parsed");
                },
                () =>
                {
                    CoordsMain = Parser.ParseCoords(ClientSettings.GeneralInputFile1);
                    Console.WriteLine("\tModel coords parsed");
                },
                () =>
                {
                    DeltasMain = Parser.ParseDeltas(ClientSettings.GeneralInputFile2);
                    Console.WriteLine("\tModel deltas parsed");
                });
            Console.WriteLine("Success");
            
            Console.Write("Checking... ");
            Check();
            Console.WriteLine("Success");
            try
            {
                remote.SendToServer($"{host} finished");
                remote.Send(WrongNodes);
            }
            catch
            {
                Console.WriteLine("Сервер оборвал связь с клиентскими приложениями, выполнение задачи завершено");
                return;
            }

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

        private static void GetTaskFile(TaskItem item, string file)
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
