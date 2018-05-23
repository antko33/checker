using System;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Collections.Generic;
using System.IO;
using GeneralRemote;
using System.Threading;

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
            Console.WriteLine("STARTED");

            TcpChannel tcpChannel = new TcpChannel(ServerSettings.Port);
            ChannelServices.RegisterChannel(tcpChannel, false);

            RemotingConfiguration.RegisterWellKnownServiceType(
                typeof(GeneralRemoteClass),
                ServerSettings.RemName,
                WellKnownObjectMode.Singleton);

            ServerSettings.sem.Wait();
            Console.WriteLine("Generating report...");
            Report report = new Report(ServerSettings.Report, ServerSettings.Result);
            report.GenerateReport();
            Console.WriteLine("Report generarted");
            Console.WriteLine("FINISH");

            Console.Read();
        }

        /// <summary>
        /// Чтение настроек из ini-файла
        /// </summary>
        private static void Init()
        {
            Ini ini = new Ini("settings.ini");
            ServerSettings.Port = Convert.ToInt32(ini.GetValue("Port", "General"));
            ServerSettings.RemName = ini.GetValue("RemName", "General");
            ServerSettings.ProjectUnits = Convert.ToInt32(ini.GetValue("ProjectUnits", "General"));
            ServerSettings.PathToFiles = ini.GetValue("PathToFiles", "General");

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
