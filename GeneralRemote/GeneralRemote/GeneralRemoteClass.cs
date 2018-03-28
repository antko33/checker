﻿using System;
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

        public FileInfo ReplyFromServer()
        {
            return new FileInfo(ServerSettings.queue[0]);
        }
    }
}
