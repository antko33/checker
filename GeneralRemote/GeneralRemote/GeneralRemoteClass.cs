using System;
using System.Collections.Generic;
using System.IO;

namespace GeneralRemote
{
    public class GeneralRemoteClass : MarshalByRefObject
    {

        public void SendToServer(string message)
        {
            Console.WriteLine(message);
        }

        public void Send(List<NodePair> a)
        {
            ServerSettings.Result.AddRange(a);
            ServerSettings.CheckIfAllDone();
        }

        public Task GetTaskFromServer()
        {
#if !DEBUG
            return ServerSettings.Tasks.Peek();
#else
            return ServerSettings.Tasks.Dequeue();
#endif
        }

        public Task GetModelFromServer()
        {
            return ServerSettings.Model;
        }

        public void OnClientExit(string host)
        {
            Console.WriteLine($"{host} finished incorrectly");
        }
    }
}
