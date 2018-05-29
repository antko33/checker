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

            return (Math.Abs(n1.X - n2.X) / Math.Min(n1.X, n2.X) <= ServerSettings.DeltaEpsilon && //-V3115
                    Math.Abs(n1.Y - n2.Y) / Math.Min(n1.Y, n2.Y) <= ServerSettings.DeltaEpsilon &&
                    Math.Abs(n1.Z - n2.Z) / Math.Min(n1.Z, n2.Z) <= ServerSettings.DeltaEpsilon &&
                    Math.Abs(n1.UX - n2.UX) / Math.Min(n1.UX, n2.UX) <= ServerSettings.DeltaEpsilon &&
                    Math.Abs(n1.UY - n2.UY) / Math.Min(n1.UY, n2.UY) <= ServerSettings.DeltaEpsilon &&
                    Math.Abs(n1.UZ - n2.UZ) / Math.Min(n1.UZ, n2.UZ) <= ServerSettings.DeltaEpsilon
                    );
        }

        public static bool operator !=(Delta n1, Delta n2)
        {
            if (n2 is null || n1 is null) return true;

            return !(n1 == n2);
        }
    }
}