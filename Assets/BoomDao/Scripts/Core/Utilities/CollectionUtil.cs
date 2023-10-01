namespace Boom.Utility
{
    using Boom.Values;
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

        /// Just like a foreach
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="arr"></param>
        /// <param name="action"></param>
        public static void Iterate<T>(this IEnumerable<T> arr, Action<T> action)
        {
            foreach (T item in arr) action(item);
        }

        /// <summary>
        /// Executes a function the first element it finds and returns the index for such element
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="arr"></param>
        /// <param name="action"></param>
        /// <param name="match"></param>
        public static void Once<T>(this IEnumerable<T> arr, Action<T> action)
        {
            foreach (T item in arr)
            {
                action(item);
            }
        }
        /// <summary>
        /// Executes a function the first element it match and returns the index for such element
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="arr"></param>
        /// <param name="action"></param>
        /// <param name="match"></param>
        public static int Once<T>(this IEnumerable<T> arr, Action<T> action, Predicate<T> match)
        {
            int index = 0;

            foreach (T item in arr)
            {
                if (match(item))
                {
                    action(item);
                    return index;
                }
                ++index;
            }
            return -1;
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
        public static IEnumerable<KeyValue<K,R>> Merge<K,R>(this Dictionary<K, IEnumerable<R>> dictionary)
        {
            foreach (var keyValue in dictionary)
            {
                foreach (var element in keyValue.Value)
                {
                    yield return new KeyValue<K, R>(keyValue.Key, element);
                }
            }
        }
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
        #endregion
    }
}
