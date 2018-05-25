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
            Init();

            TcpChannel tcpChannel = new TcpChannel(ServerSettings.Port);
            ChannelServices.RegisterChannel(tcpChannel, false);

            RemotingConfiguration.RegisterWellKnownServiceType(
                typeof(GeneralRemoteClass),
                ServerSettings.RemName,
                WellKnownObjectMode.Singleton);

            Console.WriteLine("STARTED");

            try
            {
                ServerSettings.sem.Wait();
            }
            catch (System.Threading.ThreadAbortException e)
            {
                OnClientExit(e.ExceptionState);
                System.Threading.Thread.ResetAbort();
                Console.Read();
                return;
            }
            ServerSettings.NeedToRaiseException = false;

            Console.WriteLine("Generating report...");
            Report report = new Report(ServerSettings.Report, ServerSettings.Result);
            report.GenerateReport();
            Console.WriteLine("Report generarted");
            Console.WriteLine("FINISH");

            Console.Read();
        }

        private static void OnClientExit(object exceptionInfo)
        {
            Console.WriteLine($"Потеряно соединение с {exceptionInfo.ToString()}. Операция прервана.");
        }

        /// <summary>
        /// Чтение настроек из ini-файла
        /// </summary>
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
        }
    }
}
