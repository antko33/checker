using System;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Windows.Forms;
using GeneralRemote;

namespace ClientMonitor
{
    static class Program
    {
        static GeneralRemoteClass remote = null;
        static string host = String.Empty;

        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
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

            //Console.Write("Connecting to server... ");
            TcpChannel tcpChannel = new TcpChannel();
            ChannelServices.RegisterChannel(tcpChannel, false);

            try
            {
                remote =
                    (GeneralRemoteClass)Activator.GetObject(
                        typeof(GeneralRemoteClass),
                        String.Format("tcp://{0}:{1}/{2}", param[0], param[1], param[2]));

                host = System.Net.Dns.GetHostName();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Application.Exit();
                return;
            }
            Application.Run();
        }

        private static void Proc_Exited(object sender, EventArgs e)
        {
            remote.OnClientExit(host, null);
            Application.Exit();
        }
    }
}
