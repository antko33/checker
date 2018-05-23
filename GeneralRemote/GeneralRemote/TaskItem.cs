using System;
using System.IO;

namespace GeneralRemote
{
    /// <summary>
    /// Экземпляр задания в виде 1 файла
    /// </summary>
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
        /// Возвращает предтавляемый файл в виде файлового потока
        /// </summary>
        /// <returns>Файловый поток соответствующего файла</returns>
        public FileStream GetFileStream()
        {
            if (file != null) return file;
            file = new FileStream(path, FileMode.Open);
            return file;
        }
    }
}
