using System.Collections.Generic;
using PeremClient.Class;

namespace PeremClient
{
    static partial class Program
    {
        private const byte WRONG_DELTAS = 0;
        private const byte WRONG_COORDS = 1;

        private static List<Coord> coordsMain, coordsPU;
        private static List<List<Delta>> deltasMain, deltasPU;
        private static List<NodePair> wrongNodes;

        private static void Check()
        {
            int indexInModel, code;
            for (int indexInPU = 1; indexInPU < coordsPU.Count; indexInPU++)    //Очередная координата узла ПЕ
            {
                indexInModel = 0;

                //Ищем такую же координату в модели
                for (int i = 1; i < coordsMain.Count; i++)
                {
                    if (coordsPU[indexInPU] != coordsMain[i]) continue;
                    indexInModel = i;
                    break;
                }

                code = indexInModel == 0 ? WRONG_COORDS : WRONG_DELTAS; //1, если нет узла с такими ко-тами; иначе 0

                //Если координаты различаются, добавить в WrongNodes
                if (code == WRONG_COORDS)
                {
                    var parameters = new Dictionary<string, int> {{"Index", indexInPU}};
                    var pair = new NodePair(null, null, null, coordsPU[indexInPU], null, parameters, WRONG_COORDS);
                    wrongNodes.Add(pair);
                    continue;
                }

                //Если координаты не различаются, определить перемещения
                for (int i = 1; i < deltasPU.Count; i++)  //i - номер загружения
                {
                    if (deltasPU[i][indexInPU] == deltasMain[i][indexInModel]) continue;
                    
                    //Если перемещения различаются, записать в WrongNodes
                    var mainParams = new Dictionary<string, int> {{"Index", indexInModel}, {"Load", i}};

                    var puParams = new Dictionary<string, int> {{"Index", indexInPU}, {"Load", i}};

                    var pair = new NodePair(coordsMain[indexInModel], deltasMain[i][indexInModel], mainParams,
                                            coordsPU[indexInPU], deltasPU[i][indexInPU], puParams);
                    wrongNodes.Add(pair);
                }
            }
        }
    }
}