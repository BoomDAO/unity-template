using Boom.Patterns.Broadcasts;
using Boom.UI;
using Candid.World.Models;
using Candid;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System.IO;

public static class ImageContentType
{
    public abstract class Base { public string content;

        protected Base(string content)
        {
            this.content = content;
        }
    }
    public class Url : Base
    {
        public Url(string content) : base(content)
        {
        }
    }
    public class Base64Encoding : Base
    {
        public Base64Encoding(string content) : base(content)
        {
        }
    }
}
public class ActionWidget : Window
{
    [SerializeField] TMP_Text text;
    [SerializeField] TMP_Text buttonText;
    [SerializeField] RawImage image;

    [SerializeField] Button actionButton;
    [SerializeField] Button infoButton;

    [SerializeField, ShowOnly] string id;
    public object customData;


    public class WindowData
    {
        public string id;
        public string content;
        public string textButtonContent;
        public UnityAction<string, object> action;
        public object customData;
        public ImageContentType.Base imageContentType;
        public InfoPopupWindow.WindowData infoWindowData;
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

        id = windowData.id;

        customData = windowData.customData;
        text.text = $"- {windowData.content}";
        buttonText.text = $"{(string.IsNullOrEmpty(windowData.textButtonContent) ? "Action" : windowData.textButtonContent)}";

        actionButton.onClick.AddListener(() =>
        {
            windowData.action?.Invoke(id, customData);
        });

        if(windowData.infoWindowData != null)
        {
            infoButton.gameObject.SetActive(true);
            infoButton.onClick.AddListener(() =>
            {
                WindowManager.Instance.OpenWindow<InfoPopupWindow>(windowData.infoWindowData, 2);
            });
        }
        else
        {
            infoButton.gameObject.SetActive(false);
        }

        image.gameObject.SetActive(windowData.imageContentType != null);
        if (windowData.imageContentType == null) return;

        //Load Image
        switch (windowData.imageContentType)
        {
            case ImageContentType.Url imageContentType:

                //Debug.Log("Load Image Url: " + imageContentType.content);
                CoroutineManager.Instance.DownloadImage(image, imageContentType.content);

                break;
            case ImageContentType.Base64Encoding imageContentType:

                string encoding = imageContentType.content.Split(',')[1];
                //Debug.Log("Load Base64 Encoded Image: " + encoding);

                byte[] imageBytes = Convert.FromBase64String(encoding);
                Texture2D tex = new(2, 2);
                tex.LoadImage(imageBytes);
                //Sprite sprite = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);
                image.texture = tex;
                break;
        }
    }
}