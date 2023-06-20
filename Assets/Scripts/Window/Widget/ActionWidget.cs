using ItsJackAnton.Patterns.Broadcasts;
using ItsJackAnton.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ActionWidget : Window
{
    [SerializeField] TMP_Text text;
    [SerializeField] TMP_Text buttonText;

    [SerializeField] Button button;
    [SerializeField, ShowOnly] string id;
    public class WindowData
    {
        public string id;
        public string content;
        public string textButtonContent;
        public UnityAction<string> action;
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

        Debug.Log("Setup ActionWidget");
        id = windowData.id;
        text.text = $"- {windowData.content}";
        buttonText.text = $"{(string.IsNullOrEmpty(windowData.textButtonContent) ? "Action" : windowData.textButtonContent)}";
        button.onClick.AddListener(() =>
        {
            windowData.action?.Invoke(id);
        });

        BroadcastState.Register<ToggleActionWidgetState>(OnToggleHandler);
    }

    private void OnDestroy()
    {
        BroadcastState.Unregister<ToggleActionWidgetState>(OnToggleHandler);
    }
    private void OnToggleHandler(ToggleActionWidgetState obj)
    {
        button.enabled = obj.enable;
    }
}