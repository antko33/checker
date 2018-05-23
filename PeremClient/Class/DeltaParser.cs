using System;
using System.Collections.Generic;
using System.IO;
using GeneralRemote;

namespace PeremClient.Class
{
    /// <summary>
    /// Статический класс, содержащий методы для считывания данных из файлов координат и перемещений
    /// </summary>
    public static partial class Parser
    {
        private const int NODES_IN_LINE = 9;    //Количество узлов в строке
        private const int MIN_INTERESTING_LENGTH = 7;
        private const int SYMBOLS_PER_NUMBER = 8;

        /// <summary>
        /// Считывает перемещения узлов из fic-файла
        /// </summary>
        /// <param name="path">Путь к файлу с перемещениями</param>
        /// <returns>Список перемещений в зависимости от номера узла и загружения</returns>
        public static List<List<Delta>> ParseDeltas(string path)
        {
            string sep = System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
            string line;
            var reader = File.OpenText(path);
            var list = new List<List<Delta>>();
            int load = 0;
            var numbersOfNodes = new int[NODES_IN_LINE];
            list.Add(null);

            int a = FindStartLine(reader);
            reader.Close(); reader = File.OpenText(path);   //Перезапуск reader
            int loads = CalculateLoads(reader, a);
            reader.Close(); reader = File.OpenText(path);   //Перезапуск reader
            list.Capacity = loads + 1;
            list.AddRange(new List<Delta>[loads]);
            for (int i = 1; i <= loads; i++)
            {
                list[i] = new List<Delta>();
                list[i].Add(null);
            }

            for (int i = -1 * a; (line = reader.ReadLine()) != null; i++)
            {
                if (i < 0) continue;
                if (line.Length <= MIN_INTERESTING_LENGTH) continue;
                line = line.Replace('|', ' ').Replace('\0', ' ');

                //Условие строка !="-------------", делать
                if (line[17] == '-' || line == "\f") continue;
                //Убираем лишнее
                if ((line[7] == '-' && line[2] == ' ') || (line[2] == ' ' && line[3] != ' '))
                    line = line.Remove(0, 1);

                if (line[6] == '-' && line[7] == ' ')
                {
                    //Если 6 символ строки тире и всё, то она содержит номер загружения
                    //Тогда выделяем подстроку с 3 и 4 символом и рожаем искомое
                    load = Convert.ToInt32(line.Substring(3, 2));
                }

                //Если же 2 символ == букве, то в этой строке полно интересного
                else if (line[2] >= 'U' && line[2] <= 'Z')
                {
                    var dirDelta = line.Substring(2, 2);
                    line = line.Remove(0, 6);
                    for (int j = 0; j < numbersOfNodes.GetLength(0); j++)
                    {
                        double readNum;
                        try
                        {
                            readNum = Convert.ToDouble(line.Substring(0, SYMBOLS_PER_NUMBER).Replace(".", sep).Replace(",", sep));
                        }
                        catch
                        {
                            readNum = 0;
                        }
                        line = line.Remove(0, SYMBOLS_PER_NUMBER);
                        switch (dirDelta)
                        {
                            case "X ":
                                list[load][numbersOfNodes[j]].X = readNum; break;
                            case "Y ":
                                list[load][numbersOfNodes[j]].Y = readNum; break;
                            case "Z ":
                                list[load][numbersOfNodes[j]].Z = readNum; break;
                            case "UX":
                                list[load][numbersOfNodes[j]].UX = readNum; break;
                            case "UY":
                                list[load][numbersOfNodes[j]].UY = readNum; break;
                            case "UZ":
                                list[load][numbersOfNodes[j]].UZ = readNum; break;
                        }
                    }
                }
                else //Чтение номеров узлов, try-catch д/игнора конца файла
                {
                    try
                    {
                        int minNumber = numbersOfNodes[numbersOfNodes.Length - 1];
                        numbersOfNodes = Array.ConvertAll(line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries), Convert.ToInt32);
                        int maxNumber = numbersOfNodes[numbersOfNodes.Length - 1];
                        for (int j = 1; j <= loads; j++)
                        {
                            list[j].AddRange(new Delta[maxNumber - minNumber]);
                            for (int k = minNumber + 1; k <= maxNumber; k++)
                                list[j][k] = new Delta(0, 0, 0, 0, 0, 0);
                        }
                    }
                    catch
                    {
                        // ignored
                    }
                }
            }
            return list;
        }
    }
}