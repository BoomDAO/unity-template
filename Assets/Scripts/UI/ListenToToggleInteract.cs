using ItsJackAnton.Patterns.Broadcasts;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ListenToToggleInteract : MonoBehaviour
{
    private Button btn;

    private void Awake()
    {
        btn = GetComponent<Button>();

        BroadcastState.Register<DisableButtonInteraction>(AllowButtonInteractionHandler, true);
    }

    private void OnDestroy()
    {
        BroadcastState.Unregister<DisableButtonInteraction>(AllowButtonInteractionHandler);
    }

    private void AllowButtonInteractionHandler(DisableButtonInteraction interaction)
    {
        if (btn) btn.interactable = !interaction.disable;
    }
}
