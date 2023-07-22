using Boom.UI;
using UnityEngine;

public class OpenWindowBehaviour : MonoBehaviour
{
    [SerializeField] string windowName;
    [SerializeField] int sortingOrder = 0;
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
        WindowManager.Instance.OpenWindow(windowName, null, sortingOrder);
    }
    public void Close()
    {
        if (window == null) return;
        Destroy(window.gameObject);
    }
}
