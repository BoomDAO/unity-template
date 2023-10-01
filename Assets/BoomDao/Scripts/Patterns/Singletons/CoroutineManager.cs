using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class CoroutineManager : Singleton<CoroutineManager>
{
    [SerializeField, ShowOnly] int routineCount;

    [Serializable]
    public class Routine
    {
        public int SetupCount { get; private set; }
        float startTime;
        float endTime;
        bool kill;

        [field: SerializeField, ShowOnly] public Transform Source { get; private set; }
        [field: SerializeField, ShowOnly] public bool IsRunning { get; private set; }

        private readonly Action onTimeOut;
        private readonly Action onConditionMet;

        public float Perc
        {
            get
            {
                return Mathf.Clamp01((float)(Time.time - startTime) / (float)(endTime - startTime));
            }
        }
#if UNITY_EDITOR
        [field: SerializeField, ShowOnly] private float timesOutIn;
#endif

        public Routine(Transform source, Action onTimeOut = null, Action onConditionMet = null)
        {
            Source = source;
            this.onTimeOut = onTimeOut;
            this.onConditionMet = onConditionMet;
        }

        public void Kill()
        {
            kill = true;
        }
        public void StartRoutine(float duration = 60, bool debounce = false)
        {
            if (IsRunning && !debounce) return;

            ++SetupCount;
            IsRunning = true;

            startTime = Time.time;
            endTime = startTime + duration;

            Instance.RegisterRoutine(this);
        }
        public virtual void Execute() { }
        public virtual bool IsConditionMet() { return Source.gameObject.activeSelf == false || kill; }

        public bool IsReadyToDispose()
        {
            if (Source == null)
            {
                kill = false;
                IsRunning = false;
                return true;
            }
            else if (IsConditionMet())
            {
                kill = false;
                IsRunning = false;
                OnConditionMet();
                return true;
            }
            else
            {
                float now = Time.time;

                bool _readyToDispose = now >= endTime;
#if UNITY_EDITOR
                timesOutIn = endTime - now;
#endif
                if (IsRunning && _readyToDispose) OnTimeOut();
                IsRunning = !_readyToDispose;

                return _readyToDispose;
            }
        }

        protected virtual void OnTimeOut()
        {
            if (kill) return;
            onTimeOut?.Invoke();
        }
        protected virtual void OnConditionMet()
        {
            if (kill) return;

            onConditionMet?.Invoke();
        }
    }

    readonly LinkedList<Routine> _routines = new LinkedList<Routine>();

    private void Update()
    {
        LinkedListNode<Routine> _runner = _routines.First;

        while (_runner != null)
        {
            if (_runner.Value.Source != null)
            {
                if (_runner.Value.IsReadyToDispose() == false)
                {
                    _runner.Value.Execute();
                    _runner = _runner.Next;
                }
                else
                {
                    LinkedListNode<Routine> _runnerToDispose = _runner;
                    _runner = _runner.Next;
                    UnregisterRoutine(_runnerToDispose.Value);
                }
            }
            else
            {
                LinkedListNode<Routine> _runnerToDispose = _runner;
                _runner = _runner.Next;
                UnregisterRoutine(_runnerToDispose.Value);
            }
        }
    }

    void RegisterRoutine(Routine routine)
    {
        if (routine != null)
        {
            _routines.AddLast(routine);
            ++routineCount;
        }
    }
    void UnregisterRoutine(Routine routine)
    {
        if (routine != null)
        {
            if (_routines.Remove(routine)) --routineCount;
        }
    }

    //
    public Coroutine DownloadImage(RawImage image, string imageUrl, System.Action<bool> onComplete = null)
    {
        return StartCoroutine(DownloadImages_Routine(image, imageUrl, onComplete));
    }
    private IEnumerator DownloadImages_Routine(RawImage image, string imageUrl, System.Action<bool> onComplete)
    {
        //Debug.Log("Load Image of URL " + imageUrl);
        if (string.IsNullOrEmpty(imageUrl) == false)
        {
            image.enabled = false;

            UnityWebRequest request = UnityWebRequestTexture.GetTexture(imageUrl, true);
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("> > > Error Loading Offer Image: " + request.result + "\nURL: " + imageUrl);
                onComplete?.Invoke(false);
            }
            else
            {
                if (image)
                {
                    image.enabled = true;

                    Texture2D texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
                    //Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * .5f);
                    image.texture = texture;

                    onComplete?.Invoke(true);
                }
                else
                {
                    onComplete?.Invoke(false);
                }
            }
        }
    }
}
public static class CoroutineManagerUtil
{
    public static Coroutine DownloadImage(this RawImage image, string imageUrl, System.Action<bool> onComplete = null)
    {
        return CoroutineManager.Instance.DownloadImage(image, imageUrl, onComplete);
    }

    public static Coroutine Start(this IEnumerator coroutine)
    {
        return CoroutineManager.Instance.StartCoroutine(coroutine);
    }
    public static void Stop(this Coroutine coroutine)
    {
        CoroutineManager.Instance.StopCoroutine(coroutine);
    }
    public static void StopAll()
    {
        CoroutineManager.Instance.StopAllCoroutines();
    }

    public static CoroutineManager.Routine DelayAction(this Action action, float duration, Transform source)
    {
        var routine = new CoroutineManager.Routine(source, action);
        routine.StartRoutine(duration);
        return routine;
    }
}