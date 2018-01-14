namespace PeremClient.Class
{
    /**
     * Абстрактный класс, который унаследуют классы Coord и Delta
     */

    public abstract class NumbersSet
    {
        public double X;
        public double Y;
        public double Z;

        public NumbersSet(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public abstract bool isZero(); //true, если все свойства равны 0
    }
}