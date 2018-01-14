using System;
using System.IO;
using System.Text.RegularExpressions;

namespace PeremClient.Class
{
    //Парсеры работают
    public static partial class Parser
    {
        /**
         * Поиск точки начала чтения
         */
        private static int FindStartLine(StreamReader reader)
        {
            int begin = 0;  //Возвращаемое значение
            string line;

            Regex loadNumberRegExp = new Regex(@"^\s+\d+\s-{1}\s+");  //Регулярка проверяет строку на наличие номера загружения
            for (int i = 0; (line = reader.ReadLine()) != null; i++)
            {
                if (line.Length < 7) continue;
                if (line[0] == '\0') line = line.Remove(0, 1);  //Удаляем 1 символ, ибо он почему-то можеь быть '\0'
                //if (line.Substring(5, 2) != " -") continue;
                if (!loadNumberRegExp.IsMatch(line)) continue;
                begin = i - 2;
                break;
            }

            return begin;
        }

        /**
         * Подсчёт количества загружений
         */
        private static int CalculateLoads(StreamReader reader, int begin)
        {
            string line;
            int maxCalc = 0;
            bool six = false; //Тире в 6 символе?
            for (int i = -1 * begin - 1; (line = reader.ReadLine()) != null; i++)
            {
                if (i <= 0) continue;
                line = line.Replace('\0', ' ');
                if (line[17] == '-')
                    break;
                if (line[6] == '-' && !six)
                    six = true;
                if (line[6] == '-' && six)
                    maxCalc = Convert.ToInt16(line.Substring(3, 2));
                else if (line[7] == '-' && !six)
                {
                    line = line.Remove(0, 1);
                    maxCalc = Convert.ToInt32(line.Substring(3, 2));
                }
            }

            return maxCalc;
        }
    }
}