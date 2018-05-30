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
        /// <param name="n">Номер ПЕ</param>
        /// <returns>Задание</returns>
        public Task GetTaskFromServer(out int n)
        {
            n = ServerSettings.ProjectUnits - ServerSettings.Tasks.Count + 1;
            return ServerSettings.Tasks.Dequeue();
        }

        /// <summary>
        /// Получение модели с сервера
        /// </summary>
        /// <returns>Представление модели</returns>
        public Task GetModelFromServer()
        {
            return ServerSettings.Model;
        }

        /// <summary>
        /// Выполняется при отключении клиентского приложения
        /// </summary>
        public void OnClientExit(object sender, EventArgs e)
        {
            if (ServerSettings.NeedToRaiseException)
                ServerSettings.serverThread.Abort(sender);
        }

        /// <summary>
        /// Получение погрешности для сравнения координат
        /// </summary>
        /// <returns>Искомая погрешность</returns>
        public double GetCoordEpsilon()
        {
            return ServerSettings.CoordEpsilon;
        }

        /// <summary>
        /// Получение погрешности для сравнения координат
        /// </summary>
        /// <returns>Искомая погрешность</returns>
        public double GetDeltaEpsilon()
        {
            return ServerSettings.DeltaEpsilon;
        }

        /// <summary>
        /// <see langword="true"/>, если погрешности заданы,
        /// <see langword="false"/> в противном случае
        /// </summary>
        public bool IsEpsilonsSet { get; set; } = false;

        /// <param name="n">Количество добавляемых узлов</param>
        public void AddNodesForReport(int n)
        {
            ServerSettings.TotalNodes += n;
        }
    }
}
