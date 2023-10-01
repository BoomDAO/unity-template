namespace Boom.Utility
{
    using UnityEngine;

    public static class DebugUtil
    {
        public static void Log(this string value, string source = "")
        {
//#if UNITY_EDITOR
            Debug.Log($"> Message = from: {source}, content: {value}");
            //#endif
        }
        public static void Warning(this string value, string source = "")
        {
            //#if UNITY_EDITOR
            Debug.LogWarning($"> Warning = from: {source}, content: {value}");
            //#endif
        }
        public static void Error(this string value, string source = "")
        {
            //#if UNITY_EDITOR
            Debug.LogError($"> Error = from: {source}, content: {value}");
            //#endif
        }
        public static void Log<Source>(this string value)
        {
            value.Log(typeof(Source).Name);
        }
        public static void Warning<Source>(this string value)
        {
            value.Warning(typeof(Source).Name);
        }
        public static void Error<Source>(this string value)
        {
            value.Error(nameof(Source));
        }
    }
}