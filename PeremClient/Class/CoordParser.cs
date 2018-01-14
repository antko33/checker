using System.Collections.Generic;
using System.IO;

namespace PeremClient.Class
{
    public static partial class Parser
    {
        /**
         * Считывает координаты узлов из csv-файла
         */
        public static List<Coord> ParseCoords(string path)
        {
            string line;
            double x, y, z;
            var reader = File.OpenText(path);
            var list = new List<Coord> {null};
            //Для того, чтобы индексы координат узлов начинались с 1

            while ((line = reader.ReadLine()) != null)
            {
                if (line[0] < '0' || line[0] > '9') continue; //Продолжить, если строка начинается не с цифры
                string[] parts = line.Split(';');
                //Преобразуем подстроки, полученные при разбиении line, в тип double
                string sep = System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
                x = double.Parse((sep == ".") ? parts[1].Replace(",", ".") : parts[1].Replace(".", ","));
                y = double.Parse((sep == ".") ? parts[2].Replace(",", ".") : parts[2].Replace(".", ","));
                z = double.Parse((sep == ".") ? parts[3].Replace(",", ".") : parts[3].Replace(".", ","));
                var coord = new Coord(x, y, z);
                list.Add(coord);
            }

            return list;
        }
    }
}