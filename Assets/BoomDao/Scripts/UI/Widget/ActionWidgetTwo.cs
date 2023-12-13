using System;
using System.Collections;
using System.Collections.Generic;
using Boom.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ActionWidgetTwo : Window
{
    public class WindowData
    {
        public string title;
        public string buttonTitle;
        public Action<object> action;
        public object data;

        public WindowData(string title, string buttonTitle, Action<object> action, object data)
        {
            this.title = title;
            this.buttonTitle = buttonTitle;
            this.action = action;
            this.data = data;
        }
    }

    [SerializeField] TMP_Text title;
    [SerializeField] TMP_Text buttonText;
    [SerializeField] Button actionButton;

    Action<object> action;
    public object data;

    public override bool RequireUnlockCursor()
    {
        return true;
    }
    public override void Setup(object data)
    {
        if (data is not WindowData windowData) return;

        title.text = windowData.title;
        buttonText.text = windowData.buttonTitle;

        action = windowData.action;
        this.data = windowData.data;

        actionButton.onClick.AddListener(ExecuteAction);
    }

    private void ExecuteAction()
    {
        action.Invoke(data);
    }

}