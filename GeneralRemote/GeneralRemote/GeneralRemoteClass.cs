using System;
using PeremServer;
using System.IO;

namespace GeneralRemote
{
    public class GeneralRemoteClass : MarshalByRefObject
    {
        public void SendToServer(string message)
        {
            Console.WriteLine(message);
        }

        public Task GetTaskFromServer()
        {
#if DEBUG
            return ServerSettings.Tasks.Peek();
#else
            return ServerSettings.Tasks.Dequeue();
#endif
        }

        public Task GetModelFromServer()
        {
            return ServerSettings.Model;
        }
    }
}
