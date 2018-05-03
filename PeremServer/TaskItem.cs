using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace PeremServer
{
    [Serializable]
    public class TaskItem
    {
        private string path;
        FileStream file;

        public TaskItem(string path)
        {
            this.path = path;
        }

        public TaskItem(FileStream fs)
        {
            file = fs;
        }

        public FileStream GetFileStream()
        {
            if (file != null) return file;
            file = new FileStream(path, FileMode.Open);
            return file;
        }
    }
}
