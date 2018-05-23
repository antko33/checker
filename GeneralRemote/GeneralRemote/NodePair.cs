using System;
using System.Collections.Generic;
using System.Collections;

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

        private Tuple<int, int> _mainParams;
        private Tuple<int /* pu */, int /* load */, int /* node */> _puParams;

        public NodePair(Coord cm, Delta dm, Tuple<int, int> mainInts,
                        Coord cpu, Delta dpu, Tuple<int, int, int> puInts,
                        int code = 0)
        {
            _coordMain = cm; _deltaMain = dm; _mainParams = mainInts;
            _coordPU = cpu; _deltaPU = dpu; _puParams = puInts;
            _code = code;
        }

        public NodePair(Coord cpu, Tuple<int, int, int> puInts, int code = 1)
        {
            _coordPU = cpu;
            _puParams = puInts;
            _code = code;
        }

        public Coord CoordMain => _coordMain;
        public Coord CoordPU => _coordPU;

        public Delta DeltaMain => _deltaMain;
        public Delta DeltaPU => _deltaPU;

        public int Code => _code;

        public Tuple<int, int> MainParams => _mainParams;
        public Tuple<int, int, int> PuParams => _puParams;

        public static bool operator==(NodePair i1, NodePair i2)
        {
            return
                i1.CoordMain == i2.CoordMain &&
                i1.CoordPU == i2.CoordPU &&
                i1.DeltaMain == i2.DeltaMain &&
                i1.DeltaPU == i2.DeltaPU &&
                i1.MainParams.Item1 == i2.MainParams.Item1 &&
                i1.MainParams.Item2 == i2.MainParams.Item2 &&
                i1.PuParams.Item1 == i2.PuParams.Item1 &&
                i1.PuParams.Item2 == i2.PuParams.Item2 &&
                i1.PuParams.Item3 == i2.PuParams.Item3;
        }

        public static bool operator!=(NodePair i1, NodePair i2)
        {
            return !(i1 == i2);
        }
    }
}