using Boom.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MultiplayerActionWindow : Window
{
    public class WindowData
    {
        public string title;
        public IEnumerable<ActionWidgetTwo.WindowData> actions;

        public WindowData(string title, IEnumerable<ActionWidgetTwo.WindowData> actions)
        {
            this.title = title;
            this.actions = actions;
        }
    }

    [SerializeField] TMP_Text title;
    [SerializeField] Transform content;

    public override bool RequireUnlockCursor()
    {
        return true;
    }

    public override void Setup(object data)
    {
        if (data is not WindowData _data) return;

        title.text = _data.title;

        foreach (Transform child in content.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (var item in _data.actions)
        {
            WindowManager.Instance.AddWidgets<ActionWidgetTwo>(item, content);
        }
    }
}
