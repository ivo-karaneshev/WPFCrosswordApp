using System;

namespace CrosswordApp.Common
{
    class Bindable2DArray<T>
    {
        private T[,] data;

        public Bindable2DArray(int sizeX, int sizeY)
        {
            data = new T[sizeX, sizeY];
        }

        public T this[int c1, int c2]
        {
            get { return data[c1, c2]; }
            set { data[c1, c2] = value; }
        }

        public string GetStringIndex(int c1, int c2)
        {
            return c1.ToString() + "-" + c2.ToString();
        }

        private void SplitIndex(string index, out int c1, out int c2)
        {
            var parts = index.Split('-');
            if (parts.Length != 2)
                throw new ArgumentException("The provided index is not valid");

            c1 = int.Parse(parts[0]);
            c2 = int.Parse(parts[1]);
        }

        public T this[string index]
        {
            get
            {
                int c1, c2;
                SplitIndex(index, out c1, out c2);
                return data[c1, c2];
            }
            set
            {
                int c1, c2;
                SplitIndex(index, out c1, out c2);
                data[c1, c2] = value;
            }
        }

        public static implicit operator T[,] (Bindable2DArray<T> a)
        {
            return a.data;
        }
    }
}
