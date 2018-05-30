using System;

namespace GeneralRemote
{
    /// <summary>
    /// Класс, описывающий координаты узла
    /// </summary>
    [Serializable]
    public class Coord : NumbersSet
    {
        /// <summary>
        /// Погрешность для сравнения координат 2 узлов
        /// </summary>
        public static double Epsilon { get; set; }

        /// <param name="x">Коордната узла по оси X</param>
        /// <param name="y">Коордната узла по оси Y</param>
        /// <param name="z">Коордната узла по оси Z</param>
        public Coord(double x, double y, double z) : base(x, y, z) { }

        /// <summary>
        /// Проверка координат узлов на совпадение и несовпадение
        /// </summary>
        public static bool operator ==(Coord n1, Coord n2)
        {
            if (n2 is null || n1 is null) return false;

            return (Math.Abs(n1.X - n2.X) <= Epsilon && //-V3115
                    Math.Abs(n1.Y - n2.Y) <= Epsilon &&
                    Math.Abs(n1.Z - n2.Z) <= Epsilon);

            //return (Math.Abs(n1.X - n2.X) <= Math.Abs(Math.Min(n1.X, n2.X) * Epsilon) && //-V3115
            //        Math.Abs(n1.Y - n2.Y) <= Math.Abs(Math.Min(n1.Y, n2.Y) * Epsilon) &&
            //        Math.Abs(n1.Z - n2.Z) <= Math.Abs(Math.Min(n1.Z, n2.Z) * Epsilon)
            //);
        }

        /// <summary>
        /// Проверка координат на неравенство
        /// </summary>
        public static bool operator !=(Coord n1, Coord n2)
        {
            if (n2 is null || n1 is null) return true;

            return !(n1 == n2);
        }
    }
}