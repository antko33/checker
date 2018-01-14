using System;

namespace PeremClient.Class
{
    /*
     * Класс, описывающий перемещения узла
     */
    public class Delta : NumbersSet
    {
        private const double DELTA_TOLERANCE = 0.01; //Из конфига

        public double UX;
        public double UY;
        public double UZ;

        public Delta(double x, double y, double z, double ux, double uy, double uz)
            : base(x, y, z)
        {
            UX = ux;
            UY = uy;
            UZ = uz;
        }

        public override bool isZero()
        {
            return X == 0 && Y == 0 && Z == 0 && UX == 0 && UY == 0 && UZ == 0;
        }

        //Проверка перемещений на совпадение и несовпадение
        public static bool operator ==(Delta n1, Delta n2)
        {
            return (Math.Abs(n1.X - n2.X) <= DELTA_TOLERANCE &&
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

        private bool Equals(Delta other)
        {
            return UX.Equals(other.UX) && UY.Equals(other.UY) && UZ.Equals(other.UZ);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == this.GetType() && Equals((Delta)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = UX.GetHashCode();
                hashCode = (hashCode * 397) ^ UY.GetHashCode();
                hashCode = (hashCode * 397) ^ UZ.GetHashCode();
                return hashCode;
            }
        }
    }
}