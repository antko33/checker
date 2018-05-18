using System;
using System.IO;

namespace GeneralRemote
{
    public class GeneralRemoteClass : MarshalByRefObject
    {
        public void SendToServer(string message)
        {
            Console.WriteLine(message);
        }

        public void Send(object a)
        {
            ServerSettings.Result = a;
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
