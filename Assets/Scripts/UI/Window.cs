namespace ItsJackAnton.UI
{
    using ItsJackAnton.Patterns;
    using System;
    using UnityEngine;

    public abstract class Window : MonoBehaviour
    {
        public RectTransform RectTransform { get; private set; }

        protected virtual void Awake()
        {
            RectTransform = GetComponent<RectTransform>();
            var canvas = GetComponent<Canvas>();
            if(canvas) canvas.overrideSorting = true;
        }
        //public class WindowData { }

        public abstract void Setup(object data);
        //WindowData _data = (WindowData)data;
        //if(_data == null)
        //{
        //    Debug.Log($"Window of name {gameObject.name}, requires data, data cannot be null");
        //    return;
        //}

        public abstract bool RequireUnlockCursor();

        public void Close()
        {
            WindowGod.Instance.CloseWindow(GetType().Name);
        }

        public virtual Type[] GetConflictWindow()
        {
            return null;
        }
    }
}