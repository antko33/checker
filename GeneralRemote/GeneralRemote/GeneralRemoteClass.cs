using System;
using System.Collections.Generic;

namespace GeneralRemote
{
    /// <summary>
    /// Главный класс разделяемой сборки
    /// </summary>
    public class GeneralRemoteClass : MarshalByRefObject
    {

        /// <summary>
        /// Отправка текстового сообщения на сервер
        /// </summary>
        /// <param name="message">Сообщение</param>
        public void SendToServer(string message)
        {
            Console.WriteLine(message);
        }

        /// <summary>
        /// Отправка на сервер результатов проверки
        /// </summary>
        /// <param name="a">Результат</param>
        public void Send(List<NodePair> a)
        {
            ServerSettings.Result.AddRange(a);
            ServerSettings.CheckIfAllDone();
        }

        /// <summary>
        /// Получение с сервера задания
        /// </summary>
        /// <returns>Задание</returns>
        public Task GetTaskFromServer()
        {
#if !DEBUG
            return ServerSettings.Tasks.Peek();
#else
            return ServerSettings.Tasks.Dequeue();
#endif
        }

        /// <summary>
        /// Получение модели с сервера
        /// </summary>
        /// <returns>Представление модели</returns>
        public Task GetModelFromServer()
        {
            return ServerSettings.Model;
        }
    }
}
