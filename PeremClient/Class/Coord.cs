using System;

namespace PeremClient.Class
{
    /// <summary>
    /// Класс, описывающий координаты узла
    /// </summary>
    public class Coord : NumbersSet
    {
        private const double COORD_TOLERANCE = 0.1; //Из конфига

        public Coord(double x, double y, double z) : base(x, y, z) { }

        public override bool isZero() => X == 0 && Y == 0 && Z == 0;

        //Проверка координат узлов на совпадение и несовпадение
        public static bool operator ==(Coord n1, Coord n2)
        {
            return (Math.Abs(n1.X - n2.X) <= COORD_TOLERANCE &&
                    Math.Abs(n1.Y - n2.Y) <= COORD_TOLERANCE &&
                    Math.Abs(n1.Z - n2.Z) <= COORD_TOLERANCE
            );
        }

        public static bool operator !=(Coord n1, Coord n2)
        {
            return !(n1 == n2);
        }

        private static bool Equals(Coord other)
        {
            throw new NotImplementedException();
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((Coord)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = X.GetHashCode();
                hashCode = (hashCode * 397) ^ Y.GetHashCode();
                hashCode = (hashCode * 397) ^ Z.GetHashCode();
                return hashCode;
            }
        }
    }
}