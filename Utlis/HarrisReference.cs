using System;
using System.Threading;

namespace Utlis
{
    public class SavedData<T>
    {
        public T data;
        public int isMarked;

        public SavedData(T value, bool isMarked)
        {
            data = value;
            this.isMarked = isMarked ? 1 : 0;
        }
    }

    public class HarrisReference<T> where T : class?
    {
        SavedData<T> savedData;
        public T Data
        {
            get { return savedData.data; }
            set { savedData.data = value; }
        }

		public bool IsMarked
        {
            get { return savedData.isMarked == 1; }
            set { savedData.isMarked = value ? 1 : 0; }
        }

		public HarrisReference(T data, bool marked = false)
		{
            savedData = new SavedData<T>(data, marked);
		}

        public bool CAS(T compared, T exchange)
        {
            return compared == Interlocked.CompareExchange(ref savedData.data, exchange, compared);
        }

        public bool CAS(T newVal, bool newMarked, T comparandVal, bool comparandMarked)
        {
            var initialRef = savedData;

            if (initialRef.data != comparandVal)
            {
                return false;
            }

            if ((initialRef.isMarked == 1) != comparandMarked)
            {
                return false;
            }

            return initialRef == Interlocked.CompareExchange(ref savedData, new SavedData<T>(newVal, newMarked), initialRef);
        }
    }
}
