﻿using System.Collections.Generic;
using PeremClient.Class;
using GeneralRemote;

namespace PeremClient
{
    static partial class Program
    {
        //private const byte OK = 0;
        private const byte WRONG_DELTAS = 0;
        private const byte WRONG_COORDS = 1;

        private static List<Coord> CoordsMain, CoordsPU;
        private static List<List<Delta>> DeltasMain, DeltasPU;
        private static List<NodePair> WrongNodes = new List<NodePair>();

        /// <summary>
        /// Метод, осуществляющий проверку точности решения задачи ПС МКР
        /// </summary>
        private static void Check()
        {
            int indexInModel, code;
            for (int indexInPU = 1; indexInPU < CoordsPU.Count; indexInPU++)    //Очередная координата узла ПЕ
            {
                indexInModel = 0;

                //Ищем такую же координату в модели
                for (int i = 1; i < CoordsMain.Count; i++)
                {
                    if (CoordsPU[indexInPU] != CoordsMain[i]) continue;
                    indexInModel = i;
                    break;
                }

                code = indexInModel == 0 ? WRONG_COORDS : WRONG_DELTAS; //1, если нет узла с такими ко-тами; иначе 0

                //Если координаты различаются, добавить в WrongNodes
                if (code == WRONG_COORDS)
                {
                    var parameters = new Dictionary<string, int> {{"Index", indexInPU}};
                    var pair = new NodePair(null, null, null, CoordsPU[indexInPU], null, parameters, WRONG_COORDS);
                    WrongNodes.Add(pair);
                    continue;
                }

                //Если координаты не различаются, определить перемещения
                for (int i = 1; i < DeltasMain.Count; i++)  //i - номер загружения
                {
                    if (DeltasPU[i][indexInPU] == DeltasMain[i][indexInModel]) continue;
                    
                    //Если перемещения различаются, записать в WrongNodes
                    var mainParams = new Dictionary<string, int> {{"Index", indexInModel}, {"Load", i}};

                    var puParams = new Dictionary<string, int> {{"Index", indexInPU}, {"Load", i}};

                    var pair = new NodePair(CoordsMain[indexInModel], DeltasMain[i][indexInModel], mainParams,
                                            CoordsPU[indexInPU], DeltasPU[i][indexInPU], puParams);
                    WrongNodes.Add(pair);
                }

                //Удаляем узел (для увеличения производительности)
                // Если код WRONG_DELTAS, то с этим узлом мы уже разобрались
                // и смело можем его удалять.
                // Если WRONG_COORDS, то этот узел може ещё пригодиться
                if (code == WRONG_DELTAS)
                {
                    CoordsMain.RemoveAt(indexInModel);
                    for (int i = 1; i < DeltasMain.Count; i++)
                        DeltasMain[i].RemoveAt(indexInModel);
                }
            }
        }
    }
}