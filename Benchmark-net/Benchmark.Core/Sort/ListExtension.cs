using System;
using System.Collections.Generic;

namespace Benchmark.Core.Sort
{
    public static class ListExtension
    {
        public static void QuickSort<T>(this IList<T> items, IComparer<T> comparer)
        {
            if (items == null || items.Count < 1 || comparer == null)
                return;

            QuickSort(items, 0, items.Count - 1, comparer);
        }

        static void QuickSort<T>(IList<T> items, int startIndex, int endIndex, IComparer<T> comparer)
        {
            Stack<int> bounds = new Stack<int>();
            do
            {
                if (bounds.Count != 0)
                {
                    endIndex = bounds.Pop();
                    startIndex = bounds.Pop();
                }

                T pivot = items[startIndex];
                int pivotIndex = startIndex;

                for (int i = startIndex + 1; i <= endIndex; i++)
                {
                    if (comparer.Compare(pivot, items[i]) > 0)
                    {
                        pivotIndex++;
                        if (pivotIndex != i)
                        {
                            items.Swap(pivotIndex, i);
                        }
                    }
                }

                if (startIndex != pivotIndex)
                {
                    items.Swap(startIndex, pivotIndex);
                }

                if (pivotIndex + 1 < endIndex)
                {
                    bounds.Push(pivotIndex + 1);
                    bounds.Push(endIndex);
                }

                if (startIndex < pivotIndex - 1)
                {
                    bounds.Push(startIndex);
                    bounds.Push(pivotIndex - 1);
                }

            } while (bounds.Count != 0);
        }

        static void Swap<T>(this IList<T> items, int i, int j)
        {
            T temp = items[i];
            items[i] = items[j];
            items[j] = temp;
        }

        public static void Heapsort<T>(this IList<T> items, IComparer<T> comparer)
        {
           if (items == null || items.Count < 1 || comparer == null)
                return;

            Heapsort(items, 0, items.Count - 1, comparer);
        }

        static void Heapsort<T>(IList<T> keys, int lo, int hi, IComparer<T> comparer)
        {
            int n = hi - lo + 1;
            for (int i = n / 2; i >= 1; i = i - 1)
            {
                DownHeap(keys, i, n, lo, comparer);
            }
            for (int i = n; i > 1; i = i - 1)
            {
                Swap(keys, lo, lo + i - 1);
                DownHeap(keys, 1, i - 1, lo, comparer);
            }
        }

        static void DownHeap<T>(IList<T> keys, int i, int n, int lo, IComparer<T> comparer)
        {
            T d = keys[lo + i - 1];
            int child;
            while (i <= n / 2)
            {
                child = 2 * i;
                if (child < n && comparer.Compare(keys[lo + child - 1], keys[lo + child]) < 0)
                {
                    child++;
                }
                if (!(comparer.Compare(d, keys[lo + child - 1]) < 0))
                    break;
                keys[lo + i - 1] = keys[lo + child - 1];
                i = child;
            }
            keys[lo + i - 1] = d;
        }
    }
}
