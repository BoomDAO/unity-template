using Candid.World.Models;
using ItsJackAnton.Patterns.Broadcasts;
using ItsJackAnton.UI;
using ItsJackAnton.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InventoryWindow : Window
{
    [SerializeField] TMP_Text loadingText;
    [SerializeField] Transform content;

    public class WindowData
    {
        //DATA
    }
    public override bool RequireUnlockCursor()
    {
        return false;//or true
    }

    public override void Setup(object data)
    {
        //WindowData windowData = (WindowData)data;
        //if (windowData == null)
        //{
        //    Debug.Log($"Window of name {gameObject.name}, requires data, data cannot be null");
        //    return;
        //}
        BroadcastState.Register<DataState<UserNodeData>>(UpdateWindow, true);

    }

    private void OnDestroy()
    {
        BroadcastState.Unregister<DataState<UserNodeData>>(UpdateWindow);

    }

    private void UpdateWindow(DataState<UserNodeData> obj)
    {
        foreach (Transform child in content.transform)
        {
            Destroy(child.gameObject);
        }

        if (obj.IsNull())
        {
            loadingText.text = "None";
            return;
        }

        if (obj.IsLoading())
        {
            loadingText.text = "Loading...";
            return;
        }


        int itemsCount = obj.data.itemEntities.Count;

        if (itemsCount == 0 && obj.IsReady())
        {
            loadingText.text = "None";
            return;
        }


        loadingText.text = "";

        obj.data.itemEntities.Iterate(e =>
        {
            if (UserUtil.TryGetEntityConfigData(e.Key, out var configDataType))
            {
                if(configDataType.Tag.Contains("items"))
                {
                    WindowGod.Instance.AddWidgets<BasicInventoryWidget>(new BasicInventoryWidget.WindowData()
                    { content = $"{configDataType.Name.ValueOrDefault} x {e.Value.quantity}" }, content);
                }
                else
                {
                    Debug.Log($"Element of id : \"{e.Key}\" is not of type Item");
                }
            }
            else
            {
                WindowGod.Instance.AddWidgets<BasicInventoryWidget>(new BasicInventoryWidget.WindowData()
                { content = $"{e.Value.id} x {e.Value.quantity}" }, content);
            }
        });
    }
}
