using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PeremServer
{
    /// <summary>
    /// Представляет задание для вычислительного узла в виде пары экземпляров TaskItem
    /// </summary>
    [Serializable]
    public class Task
    {
        private Tuple<TaskItem, TaskItem> task = null;

        public Task(TaskItem ti1, TaskItem ti2)
        {
            task = Tuple.Create<TaskItem, TaskItem>(ti1, ti2);
        }

        public Task(string s1, string s2)
        {
            var ti1 = new TaskItem(s1);
            var ti2 = new TaskItem(s2);
            task = Tuple.Create<TaskItem, TaskItem>(ti1, ti2);
        }

        public Tuple<TaskItem, TaskItem> GetTask()
        {
            return task;
        }
    }
}
