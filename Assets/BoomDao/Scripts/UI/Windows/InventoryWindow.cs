using Boom.UI;
using Boom.Utility;
using System;
using TMPro;
using UnityEngine;
using Boom;
using Candid;

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
        UserUtil.AddListenerDataChangeSelf<DataTypes.Entity>(UpdateWindow, true);
    }

    private void OnDestroy()
    {
        UserUtil.RemoveListenerDataChangeSelf<DataTypes.Entity>(UpdateWindow);

    }

    private void UpdateWindow(Data<DataTypes.Entity> state)
    {
        foreach (Transform child in content.transform)
        {
            Destroy(child.gameObject);
        }
        if (UserUtil.IsDataValidSelf<DataTypes.Entity>() == false)
        {
            loadingText.text = "Nothing in your inventory";
            return;
        }

        if (CandidApiManager.Instance.BoomDaoGameType == CandidApiManager.GameType.SinglePlayer? UserUtil.IsDataLoadingSelf<DataTypes.Entity>() : UserUtil.IsDataValidSelf<DataTypes.Entity>() == false)
        {

            loadingText.text = "Loading...";
            return;
        }


        EntityUtil.TryGetAllEntitiesOf<DataTypes.Entity>(EntityUtil.Queries.ownItems, out var entities, e => e);


        int itemsCount = entities.Count;

        if (itemsCount == 0)
        {
            loadingText.text = "Nothing in your inventory";
            return;
        }


        loadingText.text = "";

        entities.Iterate(e =>
        {
            try
            {
                if(!ConfigUtil.GetConfigFieldAs<string>(e, "name", out var configName)) throw new Exception($"Config of id: {e.eid} doesn't have field \"name\"");
                if (!EntityUtil.GetFieldAsDouble(e, "quantity", out var currentQuantity)) throw new Exception($"Element of id : \"{e.GetKey()}\" doesn't have field \"quantity\"");

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
        });
    }
}
