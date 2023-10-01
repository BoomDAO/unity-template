using Boom.Patterns.Broadcasts;
using Boom.UI;
using Boom;
using UnityEngine;

public class OpenWaitingAnimWindow : MonoBehaviour
{
    WaitingAnimWindow window;

    private void Awake()
    {
        BroadcastState.Register<WaitingForResponse>(Loading);
    }

    private void OnDestroy()
    {
        BroadcastState.Unregister<WaitingForResponse>(Loading);
    }
    private void Loading(WaitingForResponse arg)
    {
        if (arg.value)
        {
            window = WindowManager.Instance.OpenWindow<WaitingAnimWindow>(new WaitingAnimWindow.WindowData(arg.waitingMessage), 200);

        }
        else
        {
            if (window) window.Close();
        }
    }
}
