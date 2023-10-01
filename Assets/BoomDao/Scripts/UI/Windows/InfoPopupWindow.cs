using Boom.UI;
using Boom.Values;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InfoPopupWindow : Window
{
    public class WindowData
    {
        public string title;
        public string description;
        public UOption<(string buttonTitle, Action action)> actionButtonSettings;

        public WindowData(string title, string description, UOption<(string buttonTitle, Action action)> actionButtonSettings = null)
        {
            this.title = title;
            this.description = description;
            this.actionButtonSettings = actionButtonSettings ?? new();
        }
    }


    [SerializeField] TextMeshProUGUI titleText;
    [SerializeField] TextMeshProUGUI descriptionText;

    [SerializeField] Button actionButton;
    [SerializeField] TextMeshProUGUI actionButtonText;



    public override bool RequireUnlockCursor()
    {
        return false;
    }

    public override void Setup(object data)
    {
        WindowData windowData = (WindowData)data;
        if (windowData == null) return;

        titleText.text = windowData.title;
        descriptionText.text = windowData.description;

        if (windowData.actionButtonSettings.HasValue)
        {
            (string buttonTitle, Action action) = windowData.actionButtonSettings.Value;

            actionButton.gameObject.SetActive(true);
            actionButtonText.text = buttonTitle;
            actionButton.onClick.AddListener(() =>
            {
                action?.Invoke();
            });
        }
        else
        {
            actionButton.gameObject.SetActive(false);
        }
    }
}
