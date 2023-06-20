using Cysharp.Threading.Tasks;
using EdjCase.ICP.Agent.Agents.Http;
using System;
using System.Net;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class UnityHttpClient : IHttpClient
{
    public async Task<HttpResponse> GetAsync(string url)
    {
        using (UnityWebRequest request = UnityWebRequest.Get(GetUri(url)))
        {
            CancellationToken token = CancellationToken.None;
            await request.SendWebRequest();
            return ParseResponse(request);
        }
    }

    public async Task<HttpResponse> PostAsync(string url, byte[] cborBody)
    {
        using (UnityWebRequest request = new())
        {
            request.method = "POST";
            request.uri = GetUri(url);
            request.downloadHandler = new DownloadHandlerBuffer();

            request.uploadHandler = new UploadHandlerRaw(cborBody);
            request.uploadHandler.contentType = "application/cbor";
            await request.SendWebRequest();
            return ParseResponse(request);
        }
    }

    private static Uri GetUri(string path)
    {
        if (!path.StartsWith("/"))
        {
            path = "/" + path;
        }
        return new Uri("https://ic0.app" + path);
    }

    private static HttpResponse ParseResponse(UnityWebRequest request)
    {
        if (request.result != UnityWebRequest.Result.Success)
        {
            throw new Exception("Failed UnityWebRequest: " + request.error);
        }
        HttpStatusCode statusCode = (HttpStatusCode)request.responseCode;
        byte[] data = request.downloadHandler.data;
        return new HttpResponse(statusCode, () => Task.FromResult(data));
    }
}

