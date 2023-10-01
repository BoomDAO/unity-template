using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    protected static T instance;
    public static bool Exist { get; private set; }
    public static T Instance
    {
        get
        {
            if (instance) return instance;
            else
            {
                instance = new GameObject(typeof(T).Name).AddComponent<T>();
                Exist = true;
                return instance;
            }
        }
    }

    protected virtual void Awake()
    {
        if (instance == null)
        {
            //#if UNITY_EDITOR
            //                Debug.Log("I'm the only instance");
            //#endif
            Exist = true;
            instance = this as T;
        }
        else
        {
            if (instance != this)
            {
                //#if UNITY_EDITOR
                //                    Debug.Log($"A older instance already exist, so instance in {gameObject.name} will be destroyed");
                //#endif
                Destroy(gameObject);
            }
        }
    }
}