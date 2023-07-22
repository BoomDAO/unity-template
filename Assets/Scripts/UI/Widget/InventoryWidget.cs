using Boom.Patterns.Broadcasts;
using Boom.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class InventoryWidget : Window
{
    TMP_Text text;
    public class WindowData
    {
        public string content;
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

        text = GetComponent<TMP_Text>();
        text.text = $"- {windowData.content}";
    }
}