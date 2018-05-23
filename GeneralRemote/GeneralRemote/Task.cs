using System;

namespace GeneralRemote
{
    /// <summary>
    /// Представляет задание для вычислительного узла в виде пары экземпляров TaskItem
    /// </summary>
    [Serializable]
    public class Task
    {
        private Tuple<TaskItem, TaskItem> task = null;

        /// <param name="ti1">Экземпляр TaskItem, соответствующий файлу с координатами узлов</param>
        /// <param name="ti2">Экземпляр TaskItem, соответствующий файлу с перемещениями узлов</param>
        public Task(TaskItem ti1, TaskItem ti2)
        {
            task = Tuple.Create<TaskItem, TaskItem>(ti1, ti2);
        }

        /// <param name="s1">Имя файла с координатами</param>
        /// <param name="s2">Имя файла с перемещениями</param>
        public Task(string s1, string s2)
        {
            var ti1 = new TaskItem(s1);
            var ti2 = new TaskItem(s2);
            task = Tuple.Create<TaskItem, TaskItem>(ti1, ti2);
        }

        /// <summary>
        /// Возвращает кортеж, описывающий задание
        /// </summary>
        /// <returns></returns>
        public Tuple<TaskItem, TaskItem> GetTask()
        {
            return task;
        }
    }
}
