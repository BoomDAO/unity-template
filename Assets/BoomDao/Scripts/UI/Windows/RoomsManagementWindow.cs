using System;
using System.Collections;
using System.Collections.Generic;
using Boom;
using Boom.Patterns.Broadcasts;
using Boom.UI;
using Boom.Utility;
using Boom.Values;
using Candid;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class RoomsManagementWindow : Window
{
    public class WindowData
    {
    }

    [SerializeField] GameObject roomsPanel;
    [SerializeField] GameObject emptyText;
    [SerializeField] Transform content;

    [SerializeField] Button createRoom;


    public override bool RequireUnlockCursor()
    {
        return true;
    }

    public override void Setup(object data)
    {
        UserUtil.AddListenerMainDataChange<MainDataTypes.AllRoomData>(UpdateWindow, true);

        createRoom.onClick.AddListener(CreateRoomHandler);
        roomsPanel.SetActive(false);
        emptyText.SetActive(false);
    }
    private void OnDestroy()
    {
        UserUtil.RemoveListenerMainDataChange<MainDataTypes.AllRoomData>(UpdateWindow);
        BroadcastState.Invoke(new WaitingForResponse(false));
    }

    private void UpdateWindow(MainDataTypes.AllRoomData data)
    {

        foreach (Transform child in content.transform)
        {
            Destroy(child.gameObject);
        }


        bool isRoomDataValid = UserUtil.IsMainDataValid<MainDataTypes.AllRoomData>();
        bool isSelfEntityDataLoading = UserUtil.IsDataLoadingSelf<DataTypes.Entity>();

        if (isRoomDataValid && !isSelfEntityDataLoading)
        {
            var allRoomDataResult = UserUtil.GetMainData<MainDataTypes.AllRoomData>();

            if (allRoomDataResult.IsErr)
            {
                Debug.LogError(allRoomDataResult.AsErr());
                return;
            }


            var allRoomData = allRoomDataResult.AsOk();

            emptyText.SetActive(allRoomData.rooms.Count == 0);

            //CHECK IF USER IS ALREADY IN A ROOM TO AUTOMATICALLY OPEN THE ROOM WINDOW AND CLOSE THIS ONE
            if (allRoomData.inRoom)
            {
                WindowManager.Instance.OpenWindow<RoomWindow>(2);
                Close();
                return;
            }


            allRoomData.rooms.Iterate((e, i) =>
            {
                var room = e.Value;
                WindowManager.Instance.AddWidgets<ActionWidgetTwo>(new ActionWidgetTwo.WindowData($"- ROOM #{i}, User Count: {room.userCount}", "Join Room", JoinRoomHandler, room.roomId), content);
            });
        }

        roomsPanel.SetActive(isRoomDataValid && !isSelfEntityDataLoading);
    }

    private async void JoinRoomHandler(object roomId)
    {
        BroadcastState.Invoke(new WaitingForResponse(true));//Set to false on destroy
        Debug.Log($"Join Room Start: {roomId}");

        var testJoinRoomArgs = new List<Candid.World.Models.Field>() { new Candid.World.Models.Field("roomId", roomId.ToString()) };
        var response = await ActionUtil.ProcessAction("TestJoinRoom", testJoinRoomArgs);
        Debug.Log($"Join Room End: {roomId}");

        if (response.IsErr)
        {
            Debug.LogError(response.AsErr());
            return;
        }

        WindowManager.Instance.OpenWindow<RoomWindow>(2);
        Close();
    }

    private async void CreateRoomHandler()
    {
        BroadcastState.Invoke(new WaitingForResponse(true));//Set to false on destroy
        Debug.Log($"Create Room");

        var response = await ActionUtil.ProcessAction("TestCreateRoom");

        if (response.IsErr)
        {
            Debug.LogError(response.AsErr());
            return;
        }

        WindowManager.Instance.OpenWindow<RoomWindow>(2);
        Close();
    }
}
