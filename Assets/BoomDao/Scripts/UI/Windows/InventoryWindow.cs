using Boom.UI;
using Boom.Utility;
using Boom.Values;
using System;
using TMPro;
using UnityEngine;
using Boom;

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
        UserUtil.RegisterToDataChange<DataTypes.Entity>(UpdateWindow, true);
    }

    private void OnDestroy()
    {
        UserUtil.UnregisterToDataChange<DataTypes.Entity>(UpdateWindow);

    }

    private void UpdateWindow(DataState<Data<DataTypes.Entity>> state)
    {
        foreach (Transform child in content.transform)
        {
            Destroy(child.gameObject);
        }

        if (state.IsNull())
        {
            loadingText.text = "Nothing in your inventory";
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
            loadingText.text = "Nothing in your inventory";
            return;
        }


        loadingText.text = "";

        state.data.elements.Iterate(e =>
        {
            if(e.Value.gid == "item")
            {
                try
                {
                    if (!EntityUtil.GetConfigFieldAs<string>(e.Value.GetConfigId(), "name", out var configName)) throw new Exception($"Element of id : \"{e.Value.GetConfigId()}\" doesn't have field \"item\"");
                    if (!EntityUtil.GetFieldAs<double>(e.Value, "quantity", out var currentQuantity)) throw new Exception($"Element of id : \"{e.Value.GetKey()}\" doesn't have field \"quantity\"");

                    if (currentQuantity > 0)
                    {
                        WindowManager.Instance.AddWidgets<InventoryWidget>(new InventoryWidget.WindowData()
                        { content = $"{configName} x {currentQuantity}" }, content);
                    }
                }
                catch (Exception err)
                {
                    Debug.LogError(err.Message);
                }
            }
        });
    }
}
