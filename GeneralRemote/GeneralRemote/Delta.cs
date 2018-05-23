using System;

namespace GeneralRemote
{
    /// <summary>
    /// Класс, описывающий перемещения в узле
    /// </summary>
    [Serializable]
    public class Delta : NumbersSet
    {
        private const double DELTA_TOLERANCE = 0.2; //Из конфига

        /// <summary>
        /// Угловое перемещение отнсительно оси X
        /// </summary>
        public double UX;
        /// <summary>
        /// Угловое перемещение отнсительно оси Y
        /// </summary>
        public double UY;
        /// <summary>
        /// Угловое перемещение отнсительно оси Z
        /// </summary>
        public double UZ;

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
            return (Math.Abs(n1.X - n2.X) <= DELTA_TOLERANCE && //-V3115
                    Math.Abs(n1.Y - n2.Y) <= DELTA_TOLERANCE &&
                    Math.Abs(n1.Z - n2.Z) <= DELTA_TOLERANCE &&
                    Math.Abs(n1.UX - n2.UX) <= DELTA_TOLERANCE &&
                    Math.Abs(n1.UY - n2.UY) <= DELTA_TOLERANCE &&
                    Math.Abs(n1.UZ - n2.UZ) <= DELTA_TOLERANCE
                    );
        }

        public static bool operator !=(Delta d1, Delta d2)
        {
            return !(d1 == d2);
        }
    }
}