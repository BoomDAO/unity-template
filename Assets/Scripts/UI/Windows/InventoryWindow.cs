using Candid.World.Models;
using ItsJackAnton.Patterns.Broadcasts;
using ItsJackAnton.UI;
using ItsJackAnton.Utility;
using ItsJackAnton.Values;
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
        UserUtil.RegisterToDataChange<DataTypes.Item>(UpdateWindow, true);
    }

    private void OnDestroy()
    {
        UserUtil.UnregisterToDataChange<DataTypes.Item>(UpdateWindow);

    }

    private void UpdateWindow(DataState<Data<DataTypes.Item>> state)
    {
        foreach (Transform child in content.transform)
        {
            Destroy(child.gameObject);
        }

        if (state.IsNull())
        {
            loadingText.text = "None";
            return;
        }

        if (state.IsLoading())
        {
            loadingText.text = "Loading...";
            return;
        }


        int itemsCount = state.data.elements.Count;

        if (itemsCount == 0 && state.IsReady())
        {
            loadingText.text = "None";
            return;
        }


        loadingText.text = "";

        state.data.elements.Iterate(e =>
        {

            var actionConfigResonse = UserUtil.GetDataElementOfType<DataTypes.EntityConfig>(e.Key);
            if (actionConfigResonse.Tag == UResultTag.Err)
            {
                WindowGod.Instance.AddWidgets<BasicInventoryWidget>(new BasicInventoryWidget.WindowData()
                { content = $"{e.Value.id} x {e.Value.quantity}" }, content);
                return;
            }

            var configDataType = actionConfigResonse.AsOk();
            if (configDataType.Tag.Contains("item"))
            {
                WindowGod.Instance.AddWidgets<BasicInventoryWidget>(new BasicInventoryWidget.WindowData()
                { content = $"{configDataType.Name.ValueOrDefault} x {e.Value.quantity}" }, content);
            }
            else
            {
                Debug.Log($"Element of id : \"{e.Key}\" is not of type Item");
            }
        });
    }
}
