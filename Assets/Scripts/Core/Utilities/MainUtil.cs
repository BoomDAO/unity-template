namespace Boom.Utility
{
    using UnityEngine;

    public static class MainUtil
    {
        public static long Now()
        {
            return System.DateTimeOffset.Now.ToUnixTimeMilliseconds();
        }
        public static long MilliToSeconds(this long milli)
        {
            return milli / 1_000;
        }
        public static long SecondsToMilli(this float seconds)
        {
            return (long)(seconds * 1_000);
        }
        public static long NanoToMilliseconds(this ulong nanoSec)
        {
            return (long)nanoSec / 1_000_000;
        }
        public static ulong MilliToNano(this long nanoSec)
        {
            return (ulong)nanoSec * 1_000_000;
        }
        public static string AddressToShort(this string value)
        {
            string newString = "";

            int index = 0;
            char runner = value[index];
            while (runner != '-')
            {
                newString += runner;
                ++index;

                if (index == value.Length)
                {
                    return newString;
                }
                runner = value[index];
            }

            newString += "...";

            index = value.Length - 1;
            runner = value[index];
            while (runner != '-')
            {
                newString += runner;
                --index;
                runner = value[index];
            }

            return newString;
        }

        public static string NotScientificNotation(this double val)
        {
            return val.ToString("0." + new string('#', 339));
        }

        #region World_UI
        public static TextMesh CreateWorldText(this string text, Vector3 localPosition = default(Vector3), Transform parent = null, int scale = 1, Color? color = null, TextAnchor textAnchor = TextAnchor.MiddleCenter, TextAlignment textAlignment = TextAlignment.Center, int sortingOrder = 5000)
        {
            if (color == null) color = Color.white;

            GameObject gameObject = new GameObject("World_Text", typeof(TextMesh));
            Transform transform = gameObject.transform;
            transform.SetParent(parent, false);
            transform.localPosition = localPosition;
            TextMesh textMesh = gameObject.GetComponent<TextMesh>();
            textMesh.anchor = textAnchor;
            textMesh.alignment = textAlignment;
            textMesh.text = text;
            textMesh.characterSize = .05f * scale;
            textMesh.fontSize = 50;
            textMesh.color = (Color)color;
            textMesh.GetComponent<MeshRenderer>().sortingOrder = sortingOrder;
            return textMesh;
        }
        #endregion

        #region Pause Unpause
        public static void Pause() => Time.timeScale = 0;
        public static void Unpause() => Time.timeScale = 1;
        public static bool IsPaused() => Time.timeScale == 0;
        #endregion

        #region Cursor
        public static void HideCursor()
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        public static void ShowCursor()
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        public static bool IsCursorVisible()
        {
            return Cursor.visible;
        }
        #endregion
    }
}