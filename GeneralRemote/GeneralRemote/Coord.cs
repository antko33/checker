using System;

namespace GeneralRemote
{
    /// <summary>
    /// Класс, описывающий координаты узла
    /// </summary>
    [Serializable]
    public class Coord : NumbersSet
    {
        private const double COORD_TOLERANCE = 0.1; //Из конфига

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

            return (Math.Abs(n1.X - n2.X) <= COORD_TOLERANCE && //-V3115
                    Math.Abs(n1.Y - n2.Y) <= COORD_TOLERANCE &&
                    Math.Abs(n1.Z - n2.Z) <= COORD_TOLERANCE
            );
        }

        /// <summary>
        /// Проверка координат на неравенство
        /// </summary>
        public static bool operator !=(Coord n1, Coord n2)
        {
            if (n2 is null || n1 is null) return true;

            return (Math.Abs(n1.X - n2.X) > COORD_TOLERANCE || //-V3115
                    Math.Abs(n1.Y - n2.Y) > COORD_TOLERANCE ||
                    Math.Abs(n1.Z - n2.Z) > COORD_TOLERANCE
            );
        }
    }
}