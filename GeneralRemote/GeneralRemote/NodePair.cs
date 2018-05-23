using System;

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

        /// <summary>
        /// Координаты узлов модели
        /// </summary>
        public Coord CoordMain => _coordMain;
        /// <summary>
        /// Коорднаты узлов ПЕ
        /// </summary>
        public Coord CoordPU => _coordPU;

        /// <summary>
        /// Перемещения в узлах модели
        /// </summary>
        public Delta DeltaMain => _deltaMain;
        /// <summary>
        /// Перемещения в узлах ПЕ
        /// </summary>
        public Delta DeltaPU => _deltaPU;

        /// <summary>
        /// 0 - ошибка координат, 1 - ошибка перемещений
        /// </summary>
        public int Code => _code;

        /// <summary>
        /// Параметры узла в модели
        /// </summary>
        public Tuple<int, int> MainParams => _mainParams;
        /// <summary>
        /// Параметры узла в ПЕ
        /// </summary>
        public Tuple<int, int, int> PuParams => _puParams;

        public static bool operator==(NodePair i1, NodePair i2)
        {
            return
                i1.CoordMain == i2.CoordMain && //-V3115
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