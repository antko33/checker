﻿using System;
using System.Collections.Generic;

namespace GeneralRemote
{
    /// <summary>
    /// Класс, описывающий пару "неверных" узлов
    /// </summary>
    [Serializable]
    public class NodePair
    {
        private Coord _coordMain, _coordPU;
        private Delta _deltaMain, _deltaPU;

        private int _code;
        /* Возможные варианты:
         * 0 - не совпадают перемещения;
         * 1 - в модели нет узла с подходящими координатами;
         */

        private Dictionary<string, int> _mainParams, _puParams;

        public NodePair(Coord cm, Delta dm, Dictionary<string, int> mainInts,
                        Coord cpu, Delta dpu, Dictionary<string, int>puInts,
                        int code = 0)
        {
            _coordMain = cm; _deltaMain = dm; _mainParams = mainInts;
            _coordPU = cpu; _deltaPU = dpu; _puParams = puInts;
            _code = code;
        }

        public Coord coordMain => _coordMain;
        public Coord coordPU => _coordPU;

        public Delta deltaMain => _deltaMain;
        public Delta deltaPU => _deltaPU;

        public int code => _code;

        public Dictionary<string, int> mainParams => _mainParams;
        public Dictionary<string, int> puParams => _puParams;

        public static bool operator==(NodePair i1, NodePair i2)
        {
            return
                i1.coordMain == i2.coordMain &&
                i1.coordPU == i2.coordPU &&
                i1.deltaMain == i2.deltaMain &&
                i1.deltaPU == i2.deltaPU &&
                i1.mainParams == i2.mainParams &&
                i1.puParams == i2.puParams;
        }

        public static bool operator!=(NodePair i1, NodePair i2)
        {
            return !(i1 == i2);
        }
    }
}