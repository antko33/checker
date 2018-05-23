using System.Collections.Generic;
using System.Threading;

namespace GeneralRemote
{
    /// <summary>
    /// Предтавляет набор свойств приложения сервера
    /// </summary>
    public class ServerSettings
    {
        /// <summary>
        /// Порт, на котором запущено приложение
        /// </summary>
        public static int Port;
        /// <summary>
        /// URI хорошо известного типа
        /// </summary>
        public static string RemName;
        /// <summary>
        /// Количество проектных единиц
        /// </summary>
        public static int ProjectUnits;
        /// <summary>
        /// Путь к общей папке с файлами для проверки
        /// </summary>
        public static string PathToFiles;
        /// <summary>
        /// Очередь заданий для распределения меду вычисительными узлами
        /// </summary>
        public static Queue<Task> Tasks = new Queue<Task>();
        /// <summary>
        /// Представление модели
        /// </summary>
        public static Task Model = null;
        /// <summary>
        /// Представление резуьлтата работы подсистемы
        /// </summary>
        public static List<NodePair> Result = new List<NodePair>();
        /// <summary>
        /// Имя файла отчёта
        /// </summary>
        public static string Report;

        private static int done = 0;

        /// <summary>
        /// Проверяет, все ли узлы закончили проверку, и освобождает семафор, если да
        /// </summary>
        public static void CheckIfAllDone()
        {
            if (++done == ProjectUnits)
                sem.Release();
        }

        /// <summary>
        /// Семафор, останавливающий работу серверного приложения до окончания проверки всеми вычислительными узлами
        /// </summary>
        public static SemaphoreSlim sem = new SemaphoreSlim(0);
    }
}
