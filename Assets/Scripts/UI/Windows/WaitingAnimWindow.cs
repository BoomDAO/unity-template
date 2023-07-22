using Boom.UI;
using System.Collections;
using TMPro;
using UnityEngine;
public class WaitingAnimWindow : Window
{
    public class WindowData
    {
        public string waitingMessage;

        public WindowData(string waitingMessage)
        {
            this.waitingMessage = waitingMessage;
        }
    }

    [SerializeField] TextMeshProUGUI loadingText;
    string waitingMessageTemplate ="Waiting";
    string waitingMessage;
    readonly WaitForSeconds framePerSec = new WaitForSeconds(.5f);
    public override bool RequireUnlockCursor()
    {
        return false;
    }

    public override void Setup(object data)
    {
        if (data is WaitingAnimWindow.WindowData windowData)
        {
            waitingMessage = windowData.waitingMessage;
            if (string.IsNullOrEmpty(waitingMessage))
            {
                waitingMessage = waitingMessageTemplate;
            }
        }
        else
        {
            waitingMessage = waitingMessageTemplate;
        }

        loadingText.text = $"{waitingMessage}...";
        StartCoroutine(PlayAnimation());
    }

    public override void Close()
    {
        base.Close();

        StopAllCoroutines();
    }

    IEnumerator PlayAnimation()
    {
        loadingText.text = $"{waitingMessage}...";

        yield return framePerSec;

        loadingText.text = $"{waitingMessage}..";

        yield return framePerSec;

        loadingText.text = $"{waitingMessage}.";

        yield return framePerSec;

        loadingText.text = $"{waitingMessage}";

        yield return framePerSec;

        StartCoroutine(PlayAnimation());
    }
}
