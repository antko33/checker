using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace GeneralRemote
{
    [Serializable]
    public class TaskItem
    {
        private string path;
        FileStream file;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path">Сетевой адрес файла</param>
        public TaskItem(string path)
        {
            this.path = path;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fs">Файловый поток необходимого файла</param>
        public TaskItem(FileStream fs)
        {
            file = fs;
        }

        /// <summary>
        /// Возвращает предтавляемый файл в виде файлового потока
        /// </summary>
        /// <returns></returns>
        public FileStream GetFileStream()
        {
            if (file != null) return file;
            file = new FileStream(path, FileMode.Open);
            return file;
        }
    }
}
