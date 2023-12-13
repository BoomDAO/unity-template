namespace Boom.UI
{
    using Utility;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Events;
    using Boom.Patterns;
    using Boom.Patterns.Broadcasts;
    using System;

    public class WindowManager : Singleton<WindowManager>
    {
        readonly Dictionary<string, Window> openedWindows = new();
        [SerializeField, ShowOnly] private int unlockCursorWindowCount;
        public UnityEvent OnCloseAllWindows { get; private set; } = new();

        //hidden window and conflict windows
        private readonly Dictionary<string, List<string>> hiddenWindows = new();
        [field: SerializeField, ShowOnly] public bool CursorUnlockByDefault { get; private set; }
        [field: SerializeField, ShowOnly] public bool CursorUnlockByWindowGod { get; private set; }

        protected override void Awake()
        {
            var canvas = GetComponent<Canvas>();
            if (canvas) canvas.sortingOrder = 1000;
            base.Awake();
        }

        private void HideConflictWindows(Window window)
        {
            string typeName = window.GetType().Name;

            Type[] windows = window.GetConflictWindow();
            if (windows != null)
            {
                for (int i = 0; i < windows.Length; i++)
                {
                    string conflictWindowTypeName = windows[i].Name;

                    if (!openedWindows.TryGetValue(conflictWindowTypeName, out var conflictWindow))
                    {
                        continue;
                    }

                    conflictWindow.gameObject.SetActive(false);

                    if (!hiddenWindows.TryGetValue(conflictWindowTypeName, out var conflictWithList))
                    {
                        conflictWithList = new();
                        hiddenWindows.Add(conflictWindowTypeName, conflictWithList);
                    }

                    if (!conflictWithList.Contains(typeName)) conflictWithList.Add(typeName);
                }
            }
        }
        private void TryRemoveFromHiddenList(Window window)
        {
            string typeName = window.GetType().Name;
            if (hiddenWindows.ContainsKey(typeName)) hiddenWindows.Remove(typeName);
        }
        private void RemoveReferenceFromHiddenWindows(Window window)
        {
            var currentWindowType = window.GetType().Name;
            Type[] windows = window.GetConflictWindow();
            if (windows != null)
            {
                for (int i = 0; i < windows.Length; i++)
                {
                    string conflictWindowName = windows[i].Name;

                    if (hiddenWindows.TryGetValue(conflictWindowName, out var conflictWindowList))
                    {
                        if (conflictWindowList.Contains(currentWindowType))
                        {
                            conflictWindowList.Remove(currentWindowType);
                            if (conflictWindowList.Count == 0)
                            {
                                if (openedWindows.TryGetValue(conflictWindowName, out Window hiddenWindow))
                                {
                                    hiddenWindow.gameObject.SetActive(true);
                                }
                            }
                        }
                    }
                }
            }
        }

        public bool IsWindowOpen<T>() where T : Window
        {
            string typeName = typeof(T).Name;
            openedWindows.TryGetValue(typeName, out Window baseWindow);
            return baseWindow != null;
        }
        public T OpenWindow<T>(int? orderLayer = null) where T : Window
        {
            return (T)OpenWindow(typeof(T).Name, null, orderLayer);
        }
        public T OpenWindow<T>(object data, int? orderLayer = null) where T : Window
        {
            return (T) OpenWindow(typeof(T).Name, data, orderLayer);
        }
        public Window OpenWindow(string WindowName, object data, int? orderLayer = null)
        {
            void CheckIfCursorUnlockRequired(bool unlockCursor, bool countIfWindowFirstTime)
            {
                if (unlockCursor)
                {
                    if (unlockCursorWindowCount == 0)
                    {
                        if (MainUtil.IsCursorVisible())
                        {
                            CursorUnlockByDefault = true;
                            CursorUnlockByWindowGod = false;
                        }
                        else CursorUnlockByWindowGod = true;
                    }
                    else
                    {
                        if (CursorUnlockByDefault == false) CursorUnlockByWindowGod = true;
                        else CursorUnlockByWindowGod = false;
                    }

                    MainUtil.ShowCursor();
                    if (countIfWindowFirstTime) ++unlockCursorWindowCount;
                }
            }

            string typeName = WindowName;
            //Debug.Log($"Try Open Window of Type: {typeName}");

            if (openedWindows.TryGetValue(typeName, out Window baseWindow) == false)
            {
                baseWindow = Resources.Load<Window>($"Windows/{typeName}");

                if (baseWindow == null)
                {
                    $"Tried to open a window of name '{typeName}' but doesn't lives on Assets/Resources/Windows".Log<WindowManager>();
                    return null;
                }

                //Instantiate
                baseWindow = Instantiate(baseWindow, transform);
                baseWindow.transform.localPosition = Vector3.zero;
            }

            if (baseWindow == null)
            {
                $"Something went wrong trying to open window of type: {typeName}".Log<WindowManager>();
                return null;
            }

            var window = baseWindow;

            if(orderLayer != null)
            {
                var canvas = window.GetComponent<Canvas>();
                if (canvas)
                {
                    canvas.overrideSorting = true;

                    canvas.sortingOrder = orderLayer.Value;
                }
            }

            //Set it up
            window.Setup(data);
            //Add to Dictionary
            openedWindows.TryAdd(typeName, window);

            CheckIfCursorUnlockRequired(window.RequireUnlockCursor(), false);

            window.gameObject.SetActive(true);
            HideConflictWindows(window);

            return window;
        }
        public T AddWidgets<T>(object data, Transform parent, Vector3 offset = default) where T : Window
        {
            string typeNape = typeof(T).Name;

            if (parent == null)
            {
                $"Tried to add a widget of name '{typeNape}' but has no parent specified".Log<WindowManager>();
                return null;
            }

            T window = Resources.Load<T>($"Widgets/{typeNape}");

            if (window == null)
            {
                $"Tried to add a widget of name '{typeNape}' but doesn't lives on Assets/Resources/Widgets".Log<WindowManager>();
                return null;
            }

            T objInstance = Instantiate(window, parent);
            objInstance.transform.localPosition = offset;
            objInstance.Setup(data);

            return objInstance;
        }

        public void CloseWindow<T>() where T : Window
        {
            string typeName = typeof(T).Name;
            CloseWindow(typeName);
        }
        public void CloseWindow(string typeName)
        {
            if (openedWindows.ContainsKey(typeName))
            {
                Window objInstance = openedWindows[typeName];
                RemoveReferenceFromHiddenWindows(objInstance);
                TryRemoveFromHiddenList(objInstance);
                openedWindows.Remove(typeName);

                if (objInstance.RequireUnlockCursor()) --unlockCursorWindowCount;
                if (objInstance != null) Destroy(objInstance.gameObject);

                //OnWindowClose.Invoke(objInstance.GetType().FullName.ToHash16());

                if (unlockCursorWindowCount == 0)
                {
                    if (CursorUnlockByWindowGod && !CursorUnlockByDefault)
                    {
                        CursorUnlockByWindowGod = false;
                        MainUtil.HideCursor();
                    }
                    CursorUnlockByDefault = false;
                }
            }
        }
        public void CloseAllWindows()
        {
            var windows = openedWindows.GetEnumerator();

            while (windows.Current.Value != null)
            {
                Destroy(windows.Current.Value.gameObject);
                windows.MoveNext();
            }
            openedWindows.Clear();

            if (CursorUnlockByWindowGod && !CursorUnlockByDefault)
            {
                CursorUnlockByWindowGod = false;
                MainUtil.HideCursor();
            }
            OnCloseAllWindows.Invoke();
            hiddenWindows.Clear();
            CursorUnlockByDefault = false;
        }
    }
}
