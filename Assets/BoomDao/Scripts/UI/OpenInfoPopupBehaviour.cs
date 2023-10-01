using Boom.UI;
using UnityEngine;
using UnityEngine.UI;

public class OpenInfoPopupBehaviour : MonoBehaviour
{
    [SerializeField] bool tryBindToButton;

    [SerializeField] string title;
    [SerializeField ,TextArea(5,10)] string description;

    InfoPopupWindow window;

    private void Awake()
    {
        if (tryBindToButton)
        {
            var bttn = GetComponent<Button>();

            bttn.onClick.AddListener(Open);
        }
    }
    public void Open()
    {
        if (window != null) return;

        window = WindowManager.Instance.OpenWindow<InfoPopupWindow>(new InfoPopupWindow.WindowData(title, description),100);
    }
}
