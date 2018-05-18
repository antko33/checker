namespace GeneralRemote
{
    /// <summary>
    /// Абстрактный класс, предтавляющий некоторый набор чисел
    /// </summary>
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