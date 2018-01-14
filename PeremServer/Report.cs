using System.IO;

namespace PeremServer
{
    class Report
    {
        private FileStream[] _files; //Промежуточные отчёты
        private string _resultPath;  //Результирующий отчёт

        public Report(FileStream[] files, string resultPath)
        {
            this._files = files;
            this._resultPath = resultPath;
        }

        /**
		 * Создание результирующего отчёта на основе промежуточных
		 */
        public void MakeReport()
        {
            FileStream result;
            StreamReader reader;
            string line;

            //Очистка файла, если он существует
            if (File.Exists(_resultPath))
            {
                result = new FileStream(_resultPath, FileMode.Create);
                result.Close();
            }

            result = new FileStream(_resultPath, FileMode.Append);
            StreamWriter writer = new StreamWriter(result);

            // Объединение промежуточных отчётов в результирующий
            foreach (FileStream file in _files)
            {
                reader = new StreamReader(file);
                while ((line = reader.ReadLine()) != null)
                {
                    writer.WriteLine(line);
                }
            }

            writer.Close(); //Обязательно! Иначе изменения не сохранятся
        }
    }
}
