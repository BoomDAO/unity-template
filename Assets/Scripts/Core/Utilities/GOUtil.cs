namespace ItsJackAnton.Utility
{
    using System.Collections.Generic;
    using UnityEngine;

    public static class GOUtil
    {
        /// <summary>
        /// If a MonoBehaviour of type T is attached to this GameObject,
        /// remove it. Otherwise do nothing.
        public static void Remove<Component>(this GameObject gameObject)
            where Component : UnityEngine.Component
        {
            Object component = gameObject.GetComponent<Component>();
            if (component == null)
                return;

            if (Application.isPlaying)
                Object.Destroy(component);
            else
                Object.DestroyImmediate(component);
        }

        /// <summary>
        /// If this GameObject has a MonoBehaviour of type T,
        /// return it. Otherwise, add a new MonoBehaviour of type T
        /// and return that instead.
        /// </summary>
        public static Component GetOrAdd<Component>(this GameObject gameObject)
            where Component : UnityEngine.Component
        {
            Component result = gameObject.GetComponent<Component>();
            if (result == null)
                result = gameObject.AddComponent<Component>();
            return result;
        }

        public static T Instatiate<T>(this T gameObject, Vector3 pos, Quaternion rot) where T : Object
        {
            return Object.Instantiate(gameObject, pos, rot);
        }
        public static void SetActive(this IEnumerable<GameObject> gos, bool on)
        {
            gos.Iterate(e => e.SetActive(on));
        }
        public static void SetActiveOnChildren(this Transform source, bool on)
        {
            try
            {
                int index = 0;

                while (true)
                {
                    Transform child = source.GetChild(index);
                    child.gameObject.SetActive(on);
                    ++index;
                }
            }
            catch { }
        }

        public static void Destroy(this Object reference)
        {
            GameObject.Destroy(reference);
        }
        public static void Destroy(this IEnumerable<GameObject> gos)
        {
            gos.Iterate(e => e.Destroy());
        }
        public static void DestroyChildren(this Transform source)
        {
            try
            {
                int childrenCount = source.childCount;
                bool hasAnyChild = childrenCount > 0;
                int index = childrenCount - 1;

                while (hasAnyChild)
                {
                    Transform child = source.GetChild(index);
                    GameObject.Destroy(child.gameObject);
                    --index;
                }
            }
            catch { }
        }
    }
}