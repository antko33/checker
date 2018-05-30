using System;

namespace GeneralRemote
{
    /// <summary>
    /// Класс, описывающий перемещения в узле
    /// </summary>
    [Serializable]
    public class Delta : NumbersSet
    {
        /// <summary>
        /// Погрешность для сравнения перемещений в 2 узлах
        /// </summary>
        public static double Epsilon { get; set; }

        /// <summary>
        /// Угловое перемещение отнсительно оси X
        /// </summary>
        public double UX { get; set; }
        /// <summary>
        /// Угловое перемещение отнсительно оси Y
        /// </summary>
        public double UY { get; set; }
        /// <summary>
        /// Угловое перемещение отнсительно оси Z
        /// </summary>
        public double UZ { get; set; }

        public Delta(double x, double y, double z, double ux, double uy, double uz)
            : base(x, y, z)
        {
            UX = ux;
            UY = uy;
            UZ = uz;
        }

        /// <summary>
        /// Проверка перемещений на совпадение и несовпадение
        /// </summary>
        public static bool operator ==(Delta n1, Delta n2)
        {
            if (n2 is null || n1 is null) return false;

            return (Math.Abs(n1.X - n2.X) <= Epsilon && //-V3115
                    Math.Abs(n1.Y - n2.Y) <= Epsilon &&
                    Math.Abs(n1.Z - n2.Z) <= Epsilon &&
                    Math.Abs(n1.UX - n2.UX) <= Epsilon && //-V3115
                    Math.Abs(n1.UY - n2.UY) <= Epsilon &&
                    Math.Abs(n1.UZ - n2.UZ) <= Epsilon
                    );

            //return (Math.Abs(n1.X - n2.X) <= Math.Abs(Math.Min(n1.X, n2.X) * Epsilon) && //-V3115
            //        Math.Abs(n1.Y - n2.Y) <= Math.Abs(Math.Min(n1.Y, n2.Y) * Epsilon) &&
            //        Math.Abs(n1.Z - n2.Z) <= Math.Abs(Math.Min(n1.Z, n2.Z) * Epsilon) &&
            //        Math.Abs(n1.UX - n2.UX) <= Math.Abs(Math.Min(n1.UX, n2.UX) * Epsilon) && //-V3115
            //        Math.Abs(n1.UY - n2.UY) <= Math.Abs(Math.Min(n1.UY, n2.UY) * Epsilon) &&
            //        Math.Abs(n1.UZ - n2.UZ) <= Math.Abs(Math.Min(n1.UZ, n2.UZ) * Epsilon)
            //        );
        }

        public static bool operator !=(Delta n1, Delta n2)
        {
            if (n2 is null || n1 is null) return true;

            return !(n1 == n2);
        }
    }
}