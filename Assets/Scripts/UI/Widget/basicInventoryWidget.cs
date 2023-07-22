using Boom.UI;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class BasicInventoryWidget : Window
{
    TextMeshProUGUI text;
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

        text = GetComponent<TextMeshProUGUI>();
        text.text = $"- {windowData.content}";
    }
}