namespace GeneralRemote
{
    /// <summary>
    /// Абстрактный класс, предтавляющий некоторый набор чисел
    /// </summary>
    [System.Serializable]
    public abstract class NumbersSet
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }

        public NumbersSet(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }
    }
}