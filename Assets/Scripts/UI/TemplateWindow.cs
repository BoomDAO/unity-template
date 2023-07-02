namespace ItsJackAnton.UI
{
    using UnityEngine;

    public class TemplateWindow : Window
    {
        public class WindowData
        {
            //DATA
        }
        public override bool RequireUnlockCursor()
        {
            return false;//or true
        }

        public override void Setup(object data)
        {
            WindowData windowData = (WindowData)data;
            if (windowData == null)
            {
                Debug.Log($"Window of name {gameObject.name}, requires data, data cannot be null");
                return;
            }
        }
    }
}