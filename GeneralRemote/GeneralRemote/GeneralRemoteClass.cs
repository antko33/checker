using System;

namespace GeneralRemote
{
    public class GeneralRemoteClass : MarshalByRefObject
    {
        public void SendToServer(string message)
        {
            Console.WriteLine(message);
        }

        public void ReplyFromServer()
        {
            Console.WriteLine("message");
        }
    }
}
