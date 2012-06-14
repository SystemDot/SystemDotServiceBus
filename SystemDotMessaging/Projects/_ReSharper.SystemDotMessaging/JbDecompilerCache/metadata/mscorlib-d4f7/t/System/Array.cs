// Type: System.Array
// Assembly: mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\mscorlib.dll

using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security;

namespace System
{
    [ComVisible(true)]
    [Serializable]
    public abstract class Array : ICloneable, IList, ICollection, IEnumerable, IStructuralComparable, IStructuralEquatable
    {
        public int Length { [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success), SecuritySafeCritical, MethodImpl(MethodImplOptions.InternalCall)] get; }

        [ComVisible(false)]
        public long LongLength { [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)] get; }

        public int Rank { [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success), SecuritySafeCritical, MethodImpl(MethodImplOptions.InternalCall)] get; }

        #region ICloneable Members

        [SecuritySafeCritical]
        public object Clone();

        #endregion

        #region IList Members

        int IList.Add(object value);
        bool IList.Contains(object value);
        void IList.Clear();
        int IList.IndexOf(object value);
        void IList.Insert(int index, object value);
        void IList.Remove(object value);
        void IList.RemoveAt(int index);
        public void CopyTo(Array array, int index);
        public IEnumerator GetEnumerator();
        int ICollection.Count { get; }
        public object SyncRoot { get; }
        public bool IsReadOnly { get; }
        public bool IsFixedSize { get; }
        public bool IsSynchronized { get; }
        object IList.this[int index] { get; set; }

        #endregion

        #region IStructuralComparable Members

        int IStructuralComparable.CompareTo(object other, IComparer comparer);

        #endregion

        #region IStructuralEquatable Members

        bool IStructuralEquatable.Equals(object other, IEqualityComparer comparer);
        int IStructuralEquatable.GetHashCode(IEqualityComparer comparer);

        #endregion

        public static ReadOnlyCollection<T> AsReadOnly<T>(T[] array);

        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
        public static void Resize<T>(ref T[] array, int newSize);

        [SecuritySafeCritical]
        public static Array CreateInstance(Type elementType, int length);

        [SecuritySafeCritical]
        public static Array CreateInstance(Type elementType, int length1, int length2);

        [SecuritySafeCritical]
        public static Array CreateInstance(Type elementType, int length1, int length2, int length3);

        [SecuritySafeCritical]
        public static Array CreateInstance(Type elementType, params int[] lengths);

        public static Array CreateInstance(Type elementType, params long[] lengths);

        [SecuritySafeCritical]
        public static Array CreateInstance(Type elementType, int[] lengths, int[] lowerBounds);

        [ReliabilityContract(Consistency.MayCorruptInstance, Cer.MayFail)]
        [SecuritySafeCritical]
        public static void Copy(Array sourceArray, Array destinationArray, int length);

        [SecuritySafeCritical]
        [ReliabilityContract(Consistency.MayCorruptInstance, Cer.MayFail)]
        public static void Copy(Array sourceArray, int sourceIndex, Array destinationArray, int destinationIndex, int length);

        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        [SecuritySafeCritical]
        public static void ConstrainedCopy(Array sourceArray, int sourceIndex, Array destinationArray, int destinationIndex, int length);

        [ReliabilityContract(Consistency.MayCorruptInstance, Cer.MayFail)]
        public static void Copy(Array sourceArray, Array destinationArray, long length);

        [ReliabilityContract(Consistency.MayCorruptInstance, Cer.MayFail)]
        public static void Copy(Array sourceArray, long sourceIndex, Array destinationArray, long destinationIndex, long length);

        [SecuritySafeCritical]
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public static void Clear(Array array, int index, int length);

        [SecuritySafeCritical]
        public object GetValue(params int[] indices);

        [SecuritySafeCritical]
        public object GetValue(int index);

        [SecuritySafeCritical]
        public object GetValue(int index1, int index2);

        [SecuritySafeCritical]
        public object GetValue(int index1, int index2, int index3);

        [ComVisible(false)]
        public object GetValue(long index);

        [ComVisible(false)]
        public object GetValue(long index1, long index2);

        [ComVisible(false)]
        public object GetValue(long index1, long index2, long index3);

        [ComVisible(false)]
        public object GetValue(params long[] indices);

        [SecuritySafeCritical]
        public void SetValue(object value, int index);

        [SecuritySafeCritical]
        public void SetValue(object value, int index1, int index2);

        [SecuritySafeCritical]
        public void SetValue(object value, int index1, int index2, int index3);

        [SecuritySafeCritical]
        public void SetValue(object value, params int[] indices);

        [ComVisible(false)]
        public void SetValue(object value, long index);

        [ComVisible(false)]
        public void SetValue(object value, long index1, long index2);

        [ComVisible(false)]
        public void SetValue(object value, long index1, long index2, long index3);

        [ComVisible(false)]
        public void SetValue(object value, params long[] indices);

        [SecuritySafeCritical]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public int GetLength(int dimension);

        [ComVisible(false)]
        public long GetLongLength(int dimension);

        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        [SecuritySafeCritical]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public int GetUpperBound(int dimension);

        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        [SecuritySafeCritical]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public int GetLowerBound(int dimension);

        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
        public static int BinarySearch(Array array, object value);

        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
        public static int BinarySearch(Array array, int index, int length, object value);

        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
        public static int BinarySearch(Array array, object value, IComparer comparer);

        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
        [SecuritySafeCritical]
        public static int BinarySearch(Array array, int index, int length, object value, IComparer comparer);

        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
        public static int BinarySearch<T>(T[] array, T value);

        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
        public static int BinarySearch<T>(T[] array, T value, IComparer<T> comparer);

        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
        public static int BinarySearch<T>(T[] array, int index, int length, T value);

        [SecuritySafeCritical]
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
        public static int BinarySearch<T>(T[] array, int index, int length, T value, IComparer<T> comparer);

        public static TOutput[] ConvertAll<TInput, TOutput>(TInput[] array, Converter<TInput, TOutput> converter);

        [ComVisible(false)]
        public void CopyTo(Array array, long index);

        public static bool Exists<T>(T[] array, Predicate<T> match);
        public static T Find<T>(T[] array, Predicate<T> match);
        public static T[] FindAll<T>(T[] array, Predicate<T> match);
        public static int FindIndex<T>(T[] array, Predicate<T> match);
        public static int FindIndex<T>(T[] array, int startIndex, Predicate<T> match);
        public static int FindIndex<T>(T[] array, int startIndex, int count, Predicate<T> match);
        public static T FindLast<T>(T[] array, Predicate<T> match);
        public static int FindLastIndex<T>(T[] array, Predicate<T> match);
        public static int FindLastIndex<T>(T[] array, int startIndex, Predicate<T> match);
        public static int FindLastIndex<T>(T[] array, int startIndex, int count, Predicate<T> match);
        public static void ForEach<T>(T[] array, Action<T> action);

        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
        public static int IndexOf(Array array, object value);

        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
        public static int IndexOf(Array array, object value, int startIndex);

        [SecuritySafeCritical]
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
        public static int IndexOf(Array array, object value, int startIndex, int count);

        public static int IndexOf<T>(T[] array, T value);
        public static int IndexOf<T>(T[] array, T value, int startIndex);
        public static int IndexOf<T>(T[] array, T value, int startIndex, int count);

        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
        public static int LastIndexOf(Array array, object value);

        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
        public static int LastIndexOf(Array array, object value, int startIndex);

        [SecuritySafeCritical]
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
        public static int LastIndexOf(Array array, object value, int startIndex, int count);

        public static int LastIndexOf<T>(T[] array, T value);
        public static int LastIndexOf<T>(T[] array, T value, int startIndex);
        public static int LastIndexOf<T>(T[] array, T value, int startIndex, int count);

        [ReliabilityContract(Consistency.MayCorruptInstance, Cer.MayFail)]
        public static void Reverse(Array array);

        [ReliabilityContract(Consistency.MayCorruptInstance, Cer.MayFail)]
        [SecuritySafeCritical]
        public static void Reverse(Array array, int index, int length);

        [ReliabilityContract(Consistency.MayCorruptInstance, Cer.MayFail)]
        public static void Sort(Array array);

        [ReliabilityContract(Consistency.MayCorruptInstance, Cer.MayFail)]
        public static void Sort(Array keys, Array items);

        [ReliabilityContract(Consistency.MayCorruptInstance, Cer.MayFail)]
        public static void Sort(Array array, int index, int length);

        [ReliabilityContract(Consistency.MayCorruptInstance, Cer.MayFail)]
        public static void Sort(Array keys, Array items, int index, int length);

        [ReliabilityContract(Consistency.MayCorruptInstance, Cer.MayFail)]
        public static void Sort(Array array, IComparer comparer);

        [ReliabilityContract(Consistency.MayCorruptInstance, Cer.MayFail)]
        public static void Sort(Array keys, Array items, IComparer comparer);

        [ReliabilityContract(Consistency.MayCorruptInstance, Cer.MayFail)]
        public static void Sort(Array array, int index, int length, IComparer comparer);

        [SecuritySafeCritical]
        [ReliabilityContract(Consistency.MayCorruptInstance, Cer.MayFail)]
        public static void Sort(Array keys, Array items, int index, int length, IComparer comparer);

        [ReliabilityContract(Consistency.MayCorruptInstance, Cer.MayFail)]
        public static void Sort<T>(T[] array);

        [ReliabilityContract(Consistency.MayCorruptInstance, Cer.MayFail)]
        public static void Sort<TKey, TValue>(TKey[] keys, TValue[] items);

        [ReliabilityContract(Consistency.MayCorruptInstance, Cer.MayFail)]
        public static void Sort<T>(T[] array, int index, int length);

        [ReliabilityContract(Consistency.MayCorruptInstance, Cer.MayFail)]
        public static void Sort<TKey, TValue>(TKey[] keys, TValue[] items, int index, int length);

        [ReliabilityContract(Consistency.MayCorruptInstance, Cer.MayFail)]
        public static void Sort<T>(T[] array, IComparer<T> comparer);

        [ReliabilityContract(Consistency.MayCorruptInstance, Cer.MayFail)]
        public static void Sort<TKey, TValue>(TKey[] keys, TValue[] items, IComparer<TKey> comparer);

        [ReliabilityContract(Consistency.MayCorruptInstance, Cer.MayFail)]
        [SecuritySafeCritical]
        public static void Sort<T>(T[] array, int index, int length, IComparer<T> comparer);

        [SecuritySafeCritical]
        [ReliabilityContract(Consistency.MayCorruptInstance, Cer.MayFail)]
        public static void Sort<TKey, TValue>(TKey[] keys, TValue[] items, int index, int length, IComparer<TKey> comparer);

        public static void Sort<T>(T[] array, Comparison<T> comparison);
        public static bool TrueForAll<T>(T[] array, Predicate<T> match);

        [SecuritySafeCritical]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public void Initialize();
    }
}
