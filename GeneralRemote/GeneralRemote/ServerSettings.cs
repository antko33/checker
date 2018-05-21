using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace GeneralRemote
{
    /// <summary>
    /// Предтавляет набор свойств приложения сервера
    /// </summary>
    public class ServerSettings
    {
        public static int Port;
        public static string RemName;
        public static int ProjectUnits;
        public static string PathToFiles;
        public static Queue<Task> Tasks = new Queue<Task>();
        public static Task Model = null;
        public static List<NodePair> Result = new List<NodePair>();

        private static int done = 0;

        public static void CheckIfAllDone()
        {
            if (++done == ProjectUnits)
                sem.Release();
        }

        public static SemaphoreSlim sem = new SemaphoreSlim(0);
    }
}
