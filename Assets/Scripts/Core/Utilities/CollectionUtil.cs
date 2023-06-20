namespace ItsJackAnton.Utility
{
    using System;
    using System.Collections.Generic;

    public static class CollectionUtil
    {
        #region Helpers
        public static T[] Randomize<T>(this T[] array, Random rand = null)
        {
            rand ??= new Random();

            for (int i = 0; i < array.Length - 1; i++)
            {
                int randIndex = rand.Next(i, array.Length);
                (array[randIndex], array[i]) = (array[i], array[randIndex]);
            }
            return array;
        }
        public static bool IsValidIndex(this int index, int length) { return index < length && index > -1; }
        public static int Has<T>(this T[] array, T element) where T : class
        {
            int index = 0, _length = array.Length;

            for (; index < _length; index++)
            {
                if (array[index].Equals(element))
                {
                    return index;
                }
            }
            return -1;
        }
        #endregion

        #region IEnumerable<T>
        public static void Debug<T, DebugValue>(this IEnumerable<T> arr, Func<T, DebugValue> action)
        {
            int index = 0;
            foreach (T item in arr)
            {
                UnityEngine.Debug.Log($"arr ${arr.GetType().Name}'s element of  index: {index} - {action.Invoke(item)}");
                ++index;
            }
        }
        /// <summary>
        /// Count the elements that matches
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="arr"></param>
        /// <param name="match"></param>
        /// <returns></returns>
        public static int Count<T>(this IEnumerable<T> arr, Predicate<T> match)
        {
            int count = 0;
            foreach (T item in arr)
            {
                if (match.Invoke(item)) ++count;
            }
            return count;
        }
        /// <summary>
        /// Check if there is an element that matches the condition
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="arr"></param>
        /// <param name="match"></param>
        /// <returns></returns>
        public static bool Has<T>(this IEnumerable<T> arr, Predicate<T> match)
        {
            foreach (T item in arr)
            {
                if (match.Invoke(item)) return true;
            }

            return false;
        }
        public static bool Has<T>(this IEnumerable<T> arr, T match)
        {
            foreach (T item in arr)
            {
                if (item.Equals(match)) return true;
            }

            return false;
        }
        /// <summary>
        /// Self Descriptive
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="arr"></param>
        /// <param name="match"></param>
        /// <param name="rand"></param>
        /// <returns></returns>
        public static T GetRandom<T>(this IEnumerable<T> arr, Predicate<T> match = null, Random rand = null)
        {
            rand ??= new Random();

            LinkedList<T> linkedList = new();

            if(match != null)
            {
                foreach (T item in arr)
                {
                    if (match.Invoke(item)) linkedList.AddFirst(item);
                }
            }
            else
            {
                foreach (T item in arr) linkedList.AddFirst(item);
            }

            return linkedList.ElementAt(rand.Next(0, linkedList.Count));
        }

        /// <summary>
        /// Gives a list of filtered elements that returned true on the filter
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="arr"></param>
        /// <param name="match"></param>
        /// <returns></returns>
        public static IEnumerable<T> Filter<T>(this IEnumerable<T> arr, Predicate<T> match)
        {
            foreach (T item in arr)
            {
                if (match.Invoke(item)) yield return item;
            }
        }
        /// <summary>
        /// Separate elements that matches the condition from the one that doesn't match
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="arr"></param>
        /// <param name="match"></param>
        /// <returns></returns>
        public static (IEnumerable<T> a, IEnumerable<T> b) Separate<T>(this IEnumerable<T> arr, Predicate<T> match)
        {
            return (arr.Filter(match), arr.Filter(e=>!match.Invoke(e)));
        }
        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="arr"></param>
        /// <returns></returns>
        public static IEnumerable<T> Merge<T>(this IEnumerable<IEnumerable<T>> arr)
        {
            foreach (IEnumerable<T> subArr in arr)
            {
                foreach (T item in subArr)
                {
                    yield return (item);
                }
            }
        }
        public static IEnumerable<T> Merge<T>(this IEnumerable<T> arr1, IEnumerable<T> arr2)
        {
            foreach (T item in arr1)
            {
                yield return item;
            }

            foreach (T item in arr2)
            {
                yield return item;
            }
        }
        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="arr"></param>
        /// <returns></returns>
        public static IEnumerable<T> Collapse<T>(this IEnumerable<T> arr)
        {
            T prevElement = default;
            foreach (T item in arr)
            {
                if (prevElement.Equals(item) == false)
                {

                    prevElement = item;
                    yield return item;
                }
            }
        }
        /// <summary>
        /// Finds the first element of the list that return true on the filter
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="arr"></param>
        /// <param name="match"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static T Locate<T>(this IEnumerable<T> arr, Predicate<T> match, T defaultValue = default)
        {
            foreach (T item in arr)
            {
                if (match(item)) return item;

            }
            return defaultValue;
        }
        /// <summary>
        /// Try finds the first element of the list that return true on the filter
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="arr"></param>
        /// <param name="match"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static bool TryLocate<T>(this IEnumerable<T> arr, Predicate<T> match, out T returnValue)
        {
            foreach (T item in arr)
            {
                if (match(item))
                {
                    returnValue = item;
                    return true;
                }

            }
            returnValue = default;
            return false;
        }
        public static T LocateBest<T>(this IEnumerable<T> arr, Func<T, T, T> match)
        {
            var enumerator = arr.GetEnumerator();
            T _target = enumerator.Current;
            enumerator.MoveNext();

            while (enumerator.MoveNext())
            {
                _target = match.Invoke(_target, enumerator.Current);
            }

            return _target;
        }
        /// <summary>
        /// Find index of first element that matches
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="arr"></param>
        /// <param name="match"></param>
        /// <returns></returns>
        public static int LocateIndex<T>(this IEnumerable<T> arr, Predicate<T> match)
        {
            int index = -1;

            foreach (T e in arr)
            {
                ++index;
                if (match(e)) return index;
            }

            return index;
        }
        /// <summary>
        /// Just like a foreach
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="arr"></param>
        /// <param name="action"></param>
        public static void Iterate<T>(this IEnumerable<T> arr, Action<T> action)
        {
            foreach (T item in arr) action(item);
        }

        public static void Iterate<T>(this IEnumerable<T> arr, Action<T,int> action)
        {
            int index = 0;
            foreach (T _item in arr)
            {
                action(_item, index);
                ++index;
            }
        }
        public static void IterateInv<T>(this IEnumerable<T> arr, Action<T> action)
        {
            LinkedList<T> linkedList = new();
            foreach (T item in arr) linkedList.AddFirst(item);
            linkedList.Iterate(action);
        }
        public static void IterateInv<T>(this IEnumerable<T> arr, Action<T, int> action)
        {
            LinkedList<T> linkedList = new();
            foreach (T item in arr) linkedList.AddFirst(item);

            LinkedListNode<T> runner = linkedList.First;
            int index = linkedList.Count - 1;
            while (runner != null)
            {
                action(runner.Value, index);
                --index;
                runner = runner.Next;
            }
        }
        /// <summary>
        /// This is a conditional foreach
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="arr"></param>
        /// <param name="action"></param>
        /// <param name="match"></param>
        public static void Iterate<T>(this IEnumerable<T> arr, Action<T> action, Predicate<T> match)
        {
            foreach (T item in arr)
                if(match.Invoke(item))
                    action(item);
        }
        public static void Iterate<T>(this IEnumerable<T> arr, Action<T, int> action, Predicate<T> match)
        {
            int index = 0;

            foreach (T item in arr)
            {
                if (match.Invoke(item))
                    action(item, index);

                ++index;
            }
        }
        public static void IterateInv<T>(this IEnumerable<T> arr, Action<T> action, Predicate<T> match)
        {
            LinkedList<T> linkedList = new();
            foreach (T item in arr) linkedList.AddFirst(item);
            linkedList.Iterate(action, match);
        }
        public static void IterateInv<T>(this IEnumerable<T> arr, Action<T, int> action, Predicate<T> match)
        {
            LinkedList<T> linkedList = new();
            foreach (T item in arr) linkedList.AddFirst(item);

            LinkedListNode<T> runner = linkedList.First;
            int index = linkedList.Count - 1;
            while (runner != null)
            {
                if(match(runner.Value)) action(runner.Value, index);
                --index;
                runner = runner.Next;
            }
        }

        //

        public static void IterateUntil<T>(this IEnumerable<T> arr, Action<T> action, Predicate<T> match)
        {
            foreach (T item in arr)
            {
                if (match(item)) break;
                    action(item);
            }
        }
        public static void IterateUntilInv<T>(this IEnumerable<T> arr, Action<T> action, Predicate<T> match)
        {
            LinkedList<T> linkedList = new();
            foreach (T item in arr) linkedList.AddFirst(item);
            linkedList.IterateUntil(action, match);
        }
        /// <summary>
        /// Executes a function the first element it finds and returns the index for such element
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="arr"></param>
        /// <param name="action"></param>
        /// <param name="match"></param>
        public static int Do<T>(this IEnumerable<T> arr, Action<T> action, Predicate<T> match)
        {
            int index = -1;

            foreach (T item in arr)
            {
                if (match(item))
                {
                    action(item);
                    return index;
                }
                --index;
            }
            return index;
        }
        /// <summary>
        /// Map values
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="R"></typeparam>
        /// <param name="arr"></param>
        /// <param name="getter"></param>
        /// <returns></returns>
        public static IEnumerable<R> Map<T, R>(this IEnumerable<T> arr, Func<T, R> getter)
        {
            foreach (T item in arr) yield return getter(item);
        }
        /// <summary>
        /// Group elements that matches same key
        /// </summary>
        /// <typeparam name="Key"></typeparam>
        /// <typeparam name="T"></typeparam>
        /// <param name="arr"></param>
        /// <param name="getKey"></param>
        /// <returns></returns>
        public static IEnumerable<KeyValuePair<Key, IEnumerable<T>>> Group<Key, T>(this IEnumerable<T> arr, Func<T,Key> getKey)
        {
            Dictionary<Key, LinkedList<T>> dic = new();
            Dictionary<Key, IEnumerable<T>> dicEnum = new();
            foreach (T item in arr)
            {
                Key key = getKey(item);
                if (dic.TryAdd(key, new LinkedList<T>(new T[] { item })) == false)
                {
                    dic[key].AddLast(item);
                }
            }
            dic.Iterate(e => dicEnum.Add(e.Key, e.Value));

            return dicEnum;
        }

        /// <summary>
        /// Return true if all match condition
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="arr"></param>
        /// <param name="match"></param>
        /// <returns></returns>
        public static bool TrueForAll<T>(this IEnumerable<T> arr, Predicate<T> match)
        {
            int count = 0;
            int matchCount = 0;
            foreach (T item in arr)
            {
                ++count;
                if (match.Invoke(item))
                {
                    ++matchCount;
                    break;
                }
            }
            return count == matchCount;
        }
        public static bool TrueForAll<T>(this IEnumerable<T> arr, Predicate<T> match, out int matchCount)
        {
            int count = 0;
            matchCount = 0;
            foreach (T item in arr)
            {
                ++count;
                if (match.Invoke(item))
                {
                    ++matchCount;
                    break;
                }
            }
            return count == matchCount;
        }
        /// <summary>
        /// Return true if any match true
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="arr"></param>
        /// <param name="match"></param>
        /// <returns></returns>
        public static bool TrueForSome<T>(this IEnumerable<T> arr, Predicate<T> match)
        {

            foreach (T item in arr)
            {
                if (match.Invoke(item))
                {
                    return true;
                }
            }
            return false;
        }
        public static bool TrueForSome<T>(this IEnumerable<T> arr, Predicate<T> match, out int matchCount)
        {
            matchCount = 0;
            foreach (T item in arr)
            {
                if (match.Invoke(item))
                {
                    ++matchCount;
                }
            }
            return matchCount > 0;
        }

        public static int Reduce<T>(this IEnumerable<T> arr, Func<T, int> callback)
        {
            int result = 0;
            foreach (T item in arr) result += callback(item);
            return result;
        }
        public static float Reduce<T>(this IEnumerable<T> arr, Func<T, float> callback)
        {
            float result = 0;
            foreach (T item in arr) result += callback(item);
            return result;
        }
        public static string Reduce<T>(this IEnumerable<T> arr, Func<T, string> callback, string separator = ", ")
        {
            System.Text.StringBuilder builder = new();
            bool iterated = false;

            foreach (T item in arr)
            {
                builder.Append(callback(item));
                builder.Append(separator);
                iterated = true;
            }
            if (iterated) builder.Length -= separator.Length;

            return builder.ToString();
        }
        public static T First<T>(this IEnumerable<T> arr)
        {
            foreach (T item in arr) return item;
            return default;
        }
        public static T Last<T>(this IEnumerable<T> arr)
        {
            T last = default;
            foreach (T item in arr) last = item;
            return last;
        }
        #endregion

        #region LinkedList
        public static R ElementAt<R>(this LinkedList<R> linkedList, int targetIndex)
        {

            LinkedListNode<R> runner = linkedList.First;
            int index = 0;

            if (runner == null) return default;

            while (runner != null)
            {
                LinkedListNode<R> _next = runner.Next;
                runner = _next;

                if(index == targetIndex)
                {
                    break;
                }

                ++index;
            }
            return runner.Value;
        }
        public static LinkedList<R> Map<T, R>(this LinkedList<T> linkedList, Func<T, R> processor)
        {
            LinkedList<R> newLinkedList = new LinkedList<R>();

            LinkedListNode<T> runner = linkedList.First;

            while (runner != null)
            {
                LinkedListNode<T> _next = runner.Next;
                newLinkedList.AddLast(processor.Invoke(runner.Value));
                runner = _next;
            }
            return newLinkedList;
        }
        public static LinkedList<R> MapObj<T, R>(this LinkedList<T> linkedList, Func<T, R> processor) where T : UnityEngine.Object
        {
            LinkedList<R> newLinkedList = new LinkedList<R>();
            LinkedListNode<T> runner = linkedList.First;

            while (runner != null)
            {
                LinkedListNode<T> next = runner.Next;
                if (runner.Value)
                {
                    R processedValue = processor.Invoke(runner.Value);
                    if (processedValue != null) newLinkedList.AddLast(processedValue);
                }
                runner = next;
            }
            return newLinkedList;
        }
        public static void NullCleanUp<T>(this LinkedList<T> linkedList) where T : UnityEngine.Object
        {
            LinkedListNode<T> runner = linkedList.First;

            while (runner != null)
            {
                LinkedListNode<T> next = runner.Next;
                T currentValue = runner.Value;

                if (currentValue == null) linkedList.Remove(runner);

                runner = next;
            }
        }
        public static LinkedList<T> Copy<T>(this LinkedList<T> linkedList)
        {
            LinkedList<T> newCopy = new LinkedList<T>();
            LinkedListNode<T> runner = linkedList.First;

            while (runner != null)
            {
                LinkedListNode<T> next = runner.Next;
                newCopy.AddLast(runner.Value);
                runner = next;
            }
            return newCopy;
        }
        #endregion
    }
}
