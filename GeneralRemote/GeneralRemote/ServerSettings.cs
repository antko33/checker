using System.Collections.Generic;
using System.Threading;

namespace GeneralRemote
{
    /// <summary>
    /// Предтавляет набор свойств приложения сервера
    /// </summary>
    public static class ServerSettings
    {
        /// <summary>
        /// Порт, на котором запущено приложение
        /// </summary>
        public static int Port { get; set; }
        /// <summary>
        /// URI хорошо известного типа
        /// </summary>
        public static string RemName { get; set; }
        /// <summary>
        /// Количество проектных единиц
        /// </summary>
        public static int ProjectUnits { get; set; }
        /// <summary>
        /// Путь к общей папке с файлами для проверки
        /// </summary>
        public static string PathToFiles { get; set; }
        /// <summary>
        /// Очередь заданий для распределения меду вычисительными узлами
        /// </summary>
        public static Queue<Task> Tasks { get; set; } = new Queue<Task>();
        /// <summary>
        /// Представление модели
        /// </summary>
        public static Task Model { get; set; } = null;
        /// <summary>
        /// Представление резуьлтата работы подсистемы
        /// </summary>
        public static List<NodePair> Result { get; set; } = new List<NodePair>();
        /// <summary>
        /// Имя файла отчёта
        /// </summary>
        public static string Report { get; set; }

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
        public static SemaphoreSlim sem { get; set; } = new SemaphoreSlim(0);

        /// <summary>
        /// Представляет экземпляр потока выполнения серверного приложения
        /// </summary>
        public static Thread serverThread { get; set; }

        /// <summary>
        /// Указывает на необходимость вызова исключения в серверном приложении при обрыве связи с клиентом
        /// </summary>
        public static bool NeedToRaiseException { get; set; }

        // Погрешности (в %)
        /// <summary>
        /// Погрешность для сравнения координат узлов
        /// </summary>
        public static double CoordEpsilon { get; set; }

        /// <summary>
        /// Погрешность для сравнения перемещений узлов
        /// </summary>
        public static double DeltaEpsilon { get; set; }
    }
}
