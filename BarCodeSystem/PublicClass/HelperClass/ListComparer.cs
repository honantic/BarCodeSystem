using System.Collections.Generic;

namespace BarCodeSystem.PublicClass.HelperClass
{
    public class ListComparer<T> : IEqualityComparer<T>, IComparer<T>
    {
        public delegate bool EqualsCompare(T x, T y);

        public EqualsCompare equalsCompare;

        public delegate int IntCompare(T x, T y);

        public IntCompare compare;

        /// <summary>
        /// IEqualityComparer构造函数
        /// </summary>
        /// <param name="_equalsCompare"></param>
        public ListComparer(EqualsCompare _equalsCompare)
        {
            this.equalsCompare = _equalsCompare;
        }

        /// <summary>
        /// IComparer构造函数
        /// </summary>
        /// <param name="_compare"></param>
        public ListComparer(IntCompare _compare)
        {
            this.compare = _compare;
        }

        /// <summary>
        /// IEqualityComparer比较
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        bool IEqualityComparer<T>.Equals(T x, T y)
        {
            if (null != equalsCompare)
            {
                return equalsCompare(x, y);
            }
            else
            {
                return false;
            }
        }

        int IEqualityComparer<T>.GetHashCode(T obj)
        {
            return obj.ToString().GetHashCode();
        }

        /// <summary>
        /// IComparer比较
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public int Compare(T x, T y)
        {
            if (null != compare)
            {
                return compare(x, y);
            }
            else
            {
                return -1;
            }
        }
    }
}
