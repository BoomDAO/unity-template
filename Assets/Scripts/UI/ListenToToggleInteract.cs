using Boom.Patterns.Broadcasts;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ListenToToggleInteract : MonoBehaviour
{
    private Button btn;

    bool forceDisable;

    public Button Btn { get { return btn; } }

    private void Awake()
    {
        btn = GetComponent<Button>();

        BroadcastState.Register<WaitingForResponse>(AllowButtonInteractionHandler, true);
    }

    private void OnDestroy()
    {
        BroadcastState.Unregister<WaitingForResponse>(AllowButtonInteractionHandler);
    }

    private void AllowButtonInteractionHandler(WaitingForResponse interaction)
    {
        if (btn) btn.interactable = !interaction.value && !forceDisable;
    }

    public void ToggleForceDisable(bool val)
    {
        forceDisable = val;
        btn.interactable = !val;
    }
    public void ToggleForceDisable()
    {
        ToggleForceDisable(!forceDisable);
    }
}
