using Boom.UI;
using Boom.Utility;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryPopupWindow : Window
{
    public class WindowData
    {
        public string title;
        public List<string> elements;

        public WindowData(string title, List<string> elements)
        {
            this.title = title;
            this.elements = elements ?? new();
        }
    }


    [SerializeField] TextMeshProUGUI titleText;
    [SerializeField] TextMeshProUGUI content;

    public override bool RequireUnlockCursor()
    {
        return false;
    }

    public override void Setup(object data)
    {
        WindowData windowData = (WindowData)data;
        if (windowData == null) return;

        titleText.text = windowData.title;
        content.text = $"{windowData.elements.Reduce(e => $"{e}\n")}";
    }
}
