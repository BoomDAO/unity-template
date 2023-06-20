using System;
using System.Runtime.InteropServices;

public static class BrowserUtilsInternal
{
    [DllImport("__Internal")]
    public static extern bool IsMobileBrowser();

    [DllImport("__Internal")]
    public static extern void ToggleLoginIframe(int show);
}

public static class BrowserUtils
{
    public static void ToggleLoginIframe(bool show)
    {
#if UNITY_WEBGL && !UNITY_EDITOR
    BrowserUtilsInternal.ToggleLoginIframe(Convert.ToInt32(show));
#endif
    }

    public static bool IsMobileBrowser()
    {
#if UNITY_EDITOR
        return false;
#elif UNITY_WEBGL
        return BrowserUtilsInternal.IsMobileBrowser(); // value based on the current browser
#else
        return false; // value for builds other than WebGL
#endif
    }
}
