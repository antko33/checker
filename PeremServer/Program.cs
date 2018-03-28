using System;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Http;
using GeneralRemote;
using System.Collections.Generic;

/**
 * Главный класс сервера
 */
namespace PeremServer
{
    public static class ServerSettings
    {
        public static List<string> queue = new List<string>();
    }

    class Program
    {
        const int PORT = 32321;
        const string SOAP_NAME = "checker.soap";

        static void Main(string[] args)
        {
            Console.WriteLine("STARTED");
            ServerSettings.queue.Add("1.a");

            HttpChannel httpChannel = new HttpChannel(PORT);
            ChannelServices.RegisterChannel(httpChannel, false);

            RemotingConfiguration.RegisterWellKnownServiceType(
                typeof(GeneralRemoteClass),
                SOAP_NAME,
                WellKnownObjectMode.Singleton);

            Console.Read();
        }
    }
}
