using ItsJackAnton.UI;
using UnityEngine;

public class OpenWindowBehaviour : MonoBehaviour
{
    [SerializeField] string windowName;
    [SerializeField] bool openOnAwake;
    private Window window;

    private void Awake()
    {
        if (openOnAwake)
        {
            Open();
        }
    }
    private void OnDestroy()
    {
        if (openOnAwake)
        {
            Close();
        }
    }

    public void Open()
    {
        if (window) return;
        WindowGod.Instance.OpenWindow<Window>(windowName, null);
    }
    public void Close()
    {
        if (window == null) return;
        Destroy(window.gameObject);
    }
}
