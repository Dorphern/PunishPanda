using System;
using System.Text;
using UnityEngine;
using System.Collections.Generic;

namespace InAudio.ExtensionMethods
{
    public static class ArrayExtension
    {
        public static T[] TakeNonNulls<T>(this T[] arr) where T : UnityEngine.Object
        {
            int nullCount = 0;
            for (int i = 0; i < arr.Length; ++i)
            {
                if (arr[i] == null)
                {
                    nullCount += 1;
                }
            }

            if (nullCount > 0)
            {
                T[] nonNull = new T[arr.Length - nullCount];
                int lastIndex = 0;
                for (int i = 0; i < arr.Length; ++i)
                {
                    if (arr[i] != null)
                    {
                        nonNull[lastIndex] = arr[i];
                        lastIndex++;
                    }
                }
                return nonNull;
            }

            else
                return arr;
        }

        public static void ForEach<T>(this T[] source, Action<T> action)
        {
            for (int i = 0; i < source.Length; ++i)
            {
                action(source[i]);
            }
        }
    }

    public static class IListExtension
    {
        public static bool TrueForAll<T>(this IList<T> list, Func<T, bool> trueForElement)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (!trueForElement(list[i]))
                    return false;
            }
            return true;
        }
    }
    
    public static class ListExtension
    {
        public static int FindIndex<T>(this List<T> list, T toFind)
        {
            for (int i = 0; i < list.Count; ++i)
            {
                if(list[i].Equals(toFind))
                    return i;
            }

            return -1;
        }

        public static void RemoveLast<T>(this List<T> list)
        {
            list.RemoveAt(list.Count - 1);
        }

        public static void SwapRemoveAt<T>(this List<T> list, int index)
        {
            list[index] = list[list.Count - 1];
            list.RemoveAt(list.Count - 1);
        }

        /// <summary>
        /// Finds an element in a list, removes it by swapping with the last element and decreases the size of the list
        /// </summary>
        /// <param name="list">The list to work on</param>
        /// <param name="toFind">The object to find in the list</param>
        /// <returns>Returns true if an element was removed</returns>
        public static bool FindSwapRemove<T>(this List<T> list, T toFind)
        {
            int index = FindIndex(list, toFind);
            if(index == -1)
                return false;
            list[index] = list[list.Count - 1];
            list.RemoveAt(list.Count - 1);
            return true;
        }

        public static List<T> SwapAtIndexes<T>(this List<T> list, int i, int j)
        {
            var temp = list[i];
            list[i] = list[j];
            list[j] = temp;
            return list;
        }

        public static List<RuntimeInfo> SwapAtIndexes(this List<RuntimeInfo> list, int i, int j)
        {
            list[i].ListIndex = j;
            list[j].ListIndex = i;
            var temp = list[i];
            list[i] = list[j];
            list[j] = temp;


            return list;
        }

        public static T TryGet<T>(this List<T> list, int index)
        {
            if (index < list.Count)
                return list[index];
            return default(T);
        }

        //public static void ForEach<T>(this EventList<T> source, Action<T> action)
        //{
        //    for (int i = 0; i < source.Count; ++i)
        //    {
        //        action(source[i]);
        //    }
        //}


        public static void ThrowIfNull(this UnityEngine.Object obj)
        {
            if(obj == null)
                throw new NullReferenceException("");
        }

        public static void ThrowIfNull<T>(this T obj) where T : class
        {
            if (obj == null)
                throw new NullReferenceException(typeof(T).FullName);
        }

        public static List<U> ConvertList<T, U>(this List<T> toConvert) where T : UnityEngine.Object where U : class
        {
            List<U> newList = new List<U>(toConvert.Count);
            for (int i = 0; i < toConvert.Count; ++i)
            {
                newList.Add(toConvert[i] as U);
            }
            return newList;
        }
    }

    public static class EnumUtil
    {
        public static IEnumerable<T> GetValues<T>()
        {
            return (T[])Enum.GetValues(typeof(T));
        }

        public static string FormatedName(this Enum someEnum)
        {
            return someEnum.ToString().AddSpacesToSentence();
        }
    }

    public static class StringUtil
    {
        public static string AddSpacesToSentence(this string text, bool preserveAcronyms = false)
        {
            StringBuilder newText = new StringBuilder(text.Length*2);
            newText.Append(text[0]);
            for (int i = 1; i < text.Length; i++)
            {
                if (char.IsUpper(text[i]))
                    if ((text[i - 1] != ' ' && !char.IsUpper(text[i - 1])) ||
                        (preserveAcronyms && char.IsUpper(text[i - 1]) &&
                         i < text.Length - 1 && !char.IsUpper(text[i + 1])))
                        newText.Append(' ');
                newText.Append(text[i]);
            }
            return newText.ToString();
        }
    }

    public static class EventUtil
    {
        public static bool IsDragging(this Event unityEvent)
        {
            return unityEvent.type == EventType.DragUpdated || unityEvent.type == EventType.DragPerform;
        }

        public static bool ClickedWithin(this Event unityEvent, Rect area)
        {
            return unityEvent.type == EventType.MouseDown && area.Contains(unityEvent.mousePosition) ;
        }

        public static bool IsKeyDown(this Event unityEvent, KeyCode code)
        {
            if (unityEvent == null)
                return false;
            return Event.current.type == EventType.keyDown && Event.current.keyCode == code;
        }
    }

    public static class RectUtil
    {
        public static bool Intersect(this Rect a, Rect b)
        {
            FlipNegative( ref a );
            FlipNegative( ref b );
            bool c1 = a.xMin < b.xMax;
            bool c2 = a.xMax > b.xMin;
            bool c3 = a.yMin < b.yMax;
            bool c4 = a.yMax > b.yMin;
            return c1 && c2 && c3 && c4;
        }

        public static bool WithinWidth(this Rect a, Vector2 pos)
        {
            if (pos.x > a.x && pos.x < a.x + a.width)
                return true;
            return false;
        }

        public static void FlipNegative(ref Rect r)
        {
            if (r.width < 0)
                r.x -= (r.width *= -1);
            if (r.height < 0)
                r.y -= (r.height *= -1);
        }
    }


}
