﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels.Tcp;
using GeneralRemote;
using System.Runtime.Remoting.Channels;

namespace ClientMonitor
{
    class Program
    {
        static GeneralRemoteClass remote = null;
        static string host = String.Empty;

        static void Main(string[] args)
        {
            var ps = System.Diagnostics.Process.GetProcesses();
            System.Diagnostics.Process proc = null;

            var q =
                from p in ps
                where p.ProcessName.ToLower() == "consoleapplication"
                select p;

            if (q != null) proc = q.First();
            proc.EnableRaisingEvents = true;
            proc.Exited += Proc_Exited;

            var param = args[0].Split(new string[] { "\\//" }, StringSplitOptions.RemoveEmptyEntries);

            Console.Write("Connecting to server... ");
            TcpChannel tcpChannel = new TcpChannel();
            ChannelServices.RegisterChannel(tcpChannel, false);
            
            try
            {
                remote =
                  (GeneralRemoteClass)Activator.GetObject(
                      typeof(GeneralRemoteClass),
                      String.Format("tcp://{0}:{1}/{2}", param[0], param[1], param[2]));

                host = System.Net.Dns.GetHostName();
                Console.WriteLine("Success");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Console.Read();
        }

        private static void Proc_Exited(object sender, EventArgs e)
        {
            Console.WriteLine("done");
            remote.OnClientExit(host, null);
        }
    }
}