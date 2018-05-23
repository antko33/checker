﻿using System.IO;
using System.Collections.Generic;
using GeneralRemote;
using System;

namespace PeremServer
{
    internal class Report
    {
        private const int WRONG_COORDS = 0;
        private const int WRONG_Delta = 1;

        private StreamWriter _sw;
        private int _countWrongCoords = 0;
        private int _countWrongDelta = 0;

        public string File { get; set; }
        public List<NodePair> Result { get; set; }

        public Report() { }

        public Report(string file, List<NodePair> result)
        {
            File = file;
            Result = result;
        }

        public void GenerateReport()
        {
            using (_sw = new StreamWriter(File))
            {
                WriteHeader();

                foreach (var np in Result)
                {
                    if (np.Code == WRONG_COORDS)
                    {
                        WriteWrongCoords(np);
                    }
                    else
                    {
                        WriteWrongDelta(np);
                    }
                }

                WriteFooter();
            }
        }

        private void WriteFooter()
        {
            _sw.WriteLine($"Всего обработано узлов: {null}");
            _sw.WriteLine($"Количество ПЕ: {null}");
            _sw.WriteLine($"Ошибок координат: {_countWrongCoords}");
            _sw.WriteLine($"Ошибок перемещений: {_countWrongDelta}");
        }

        private void WriteHeader()
        {
            _sw.WriteLine("Отчёт о проверке решения задачи методом РПЕ");
            _sw.WriteLine($"Дата: {DateTime.Now.ToString()}");
            _sw.WriteLine();
        }

        private void WriteWrongDelta(NodePair np)
        {
            _sw.WriteLine("Перемещения в узле в мoдели и в ПЕ различны");
            _sw.WriteLine("\tМодель \tПЕ \tОтклонение");
            _sw.WriteLine($"\tX: {np.DeltaMain.X} \t{np.DeltaPU.X} \t{Math.Abs(np.DeltaMain.X - np.DeltaPU.X)}");
            _sw.WriteLine($"\tY: {np.DeltaMain.Y} \t{np.DeltaPU.Y} \t{Math.Abs(np.DeltaMain.Y - np.DeltaPU.Y)}");
            _sw.WriteLine($"\tZ: {np.DeltaMain.Z} \t{np.DeltaPU.Z} \t{Math.Abs(np.DeltaMain.Z - np.DeltaPU.Z)}");
            _sw.WriteLine($"\tUX: {np.DeltaMain.UX} \t{np.DeltaPU.UX} \t{Math.Abs(np.DeltaMain.UX - np.DeltaPU.UX)}");
            _sw.WriteLine($"\tUY: {np.DeltaMain.UY} \t{np.DeltaPU.UY} \t{Math.Abs(np.DeltaMain.UY - np.DeltaPU.UY)}");
            _sw.WriteLine($"\tUZ: {np.DeltaMain.UZ} \t{np.DeltaPU.UZ} \t{Math.Abs(np.DeltaMain.UZ - np.DeltaPU.UZ)}");
            _sw.WriteLine($"ПЕ № {np.PuParams.Item1}, загружение № {np.PuParams.Item2}, узел № {np.PuParams.Item3}");
            _sw.WriteLine($"В модели загружение № {np.MainParams.Item1}, узел № {np.MainParams.Item2}");
            _sw.WriteLine("--------------------------------------------------------\n");
            _countWrongDelta++;
        }

        private void WriteWrongCoords(NodePair np)
        {
            _sw.WriteLine("Узел с указанными координатами не найден в модели:");
            _sw.WriteLine($"\tX: {np.CoordPU.X}");
            _sw.WriteLine($"\tY: {np.CoordPU.Y}");
            _sw.WriteLine($"\tZ: {np.CoordPU.Z}");
            _sw.WriteLine($"ПЕ № {np.PuParams.Item1}, узел № {np.PuParams.Item3}");
            _sw.WriteLine("--------------------------------------------------------\n");
            _countWrongCoords++;
        }
    }
}
