using System;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using GeneralRemote;
using System.Collections.Generic;

/**
 * Главный класс сервера
 */
namespace PeremServer
{
    public static class ServerSettings
    {
        public static Queue<Task> tasks = new Queue<Task>();
    }

    class Program
    {
        /// <TODO>
        /// хардкод недопустим, исправить
        /// </TODO>
        const int PORT = 32321;
        const string REM_NAME = "checker.rem";
        static List<List<string>> files = new List<List<string>> { new List<string> { @"Z:\1.txt", @"Z:\2.txt" } };

        static void Main(string[] args)
        {
            Console.WriteLine("STARTED");

            CreateTaskQueue();

            TcpChannel tcpChannel = new TcpChannel(PORT);
            ChannelServices.RegisterChannel(tcpChannel, false);

            RemotingConfiguration.RegisterWellKnownServiceType(
                typeof(GeneralRemoteClass),
                REM_NAME,
                WellKnownObjectMode.Singleton);

            Console.Read();
        }

        private static void CreateTaskQueue()
        {
            foreach(var l in files)
            {
                ServerSettings.tasks.Enqueue(new Task(l[0], l[1]));
            }
        }
    }
}
