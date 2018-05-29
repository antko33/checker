using System;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.IO;
using GeneralRemote;

namespace PeremServer
{
    /// <summary>
    /// Главный класс сервера
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Init();
            }
            catch(ApplicationException)
            {
                EmergencyExit("Файл настроек отсутствует или повреждён");
            }

            TcpChannel tcpChannel = new TcpChannel(ServerSettings.Port);
            try
            {
                ChannelServices.RegisterChannel(tcpChannel, false);
            }
            catch (RemotingException e)
            {
                EmergencyExit(e.Message);
            }
            catch (System.Security.SecurityException)
            {
                EmergencyExit("Вы не имеете право настраивать удалённое взаимодействие");
            }

            RemotingConfiguration.RegisterWellKnownServiceType(
                typeof(GeneralRemoteClass),
                ServerSettings.RemName,
                WellKnownObjectMode.Singleton);

            Console.WriteLine("ГОТОВ");

            try
            {
                ServerSettings.sem.Wait();
            }
            catch (System.Threading.ThreadAbortException e)
            {
                OnClientExit(e.ExceptionState);
                System.Threading.Thread.ResetAbort();
                ChannelServices.UnregisterChannel(tcpChannel);
                EmergencyExit(null);
            }
            ServerSettings.NeedToRaiseException = false;

            Console.WriteLine("Создание отчёта...");
            Report report = new Report(ServerSettings.Report, ServerSettings.Result);
            try
            {
                report.GenerateReport();
            }
            catch(ApplicationException)
            {
                EmergencyExit("Не удалось создать отчёт");
            }
            Console.WriteLine("Отчёт готов");
            Console.WriteLine("ПРОВЕРКА ЗАВЕРШЕНА");

            Console.Read();
        }

        private static void EmergencyExit(string message)
        {
            Console.WriteLine(message);
            Console.Read();
            Environment.Exit(0);
        }

        private static void OnClientExit(object exceptionInfo)
        {
            Console.WriteLine($"Потеряно соединение с {exceptionInfo.ToString()}. Операция прервана.");
        }

        // Чтение настроек из ini-файла
        private static void Init()
        {
            ServerSettings.serverThread = System.Threading.Thread.CurrentThread;
            ServerSettings.NeedToRaiseException = true;
            Ini ini = new Ini("settings.ini");
            ServerSettings.Port = Convert.ToInt32(ini.GetValue("Port", "General"));
            ServerSettings.RemName = ini.GetValue("RemName", "General");
            ServerSettings.ProjectUnits = Convert.ToInt32(ini.GetValue("ProjectUnits", "General"));
            ServerSettings.PathToFiles = ini.GetValue("PathToFiles", "General");
            ServerSettings.Report = ini.GetValue("Report", "General");

            string file1, file2;
            file1 = Path.Combine(ServerSettings.PathToFiles, ini.GetValue("File1", "Model"));
            file2 = Path.Combine(ServerSettings.PathToFiles, ini.GetValue("File2", "Model"));
            ServerSettings.Model = new Task(file1, file2);
            for (int i = 1; i <= ServerSettings.ProjectUnits; i++)
            {
                file1 = Path.Combine(ServerSettings.PathToFiles, ini.GetValue("File1", "Project Unit " + i.ToString()));
                file2 = Path.Combine(ServerSettings.PathToFiles, ini.GetValue("File2", "Project Unit " + i.ToString()));
                Task t = new Task(file1, file2);
                ServerSettings.Tasks.Enqueue(t);
            }

            ServerSettings.CoordEpsilon = Convert.ToDouble(ini.GetValue("CoordEpsilon", "General", "0.1"));
            ServerSettings.DeltaEpsilon = Convert.ToDouble(ini.GetValue("DeltaEpsilon", "General", "0.2"));
        }
    }
}
