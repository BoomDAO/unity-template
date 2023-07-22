using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class CoroutineManager : Singleton<CoroutineManager>
{
    public Coroutine DownloadImage(RawImage image, string imageUrl, System.Action<bool> onComplete = null)
    {
        return StartCoroutine(DownloadImages_Routine(image, imageUrl, onComplete));
    }

    public IEnumerator DownloadImages_Routine(RawImage image, string imageUrl, System.Action<bool> onComplete)
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
    public static Coroutine Start(this IEnumerator coroutine)
    {
        return CoroutineManager.Instance.StartCoroutine(coroutine);
    }
    public static Coroutine DownloadImage(this RawImage image, string imageUrl, System.Action<bool> onComplete = null)
    {
        return CoroutineManager.Instance.DownloadImage(image, imageUrl, onComplete);
    }
    public static void Stop(this Coroutine coroutine)
    {
        CoroutineManager.Instance.StopCoroutine(coroutine);
    }
}