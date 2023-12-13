using System.Threading;
using System;
using UnityEngine;
using EdjCase.ICP.Agent;
using EdjCase.ICP.Candid.Mapping;
using EdjCase.ICP.Agent.Agents;
using EdjCase.ICP.Candid.Models;
using EdjCase.ICP.WebSockets;
using Candid;

public class WebSocketManager : MonoBehaviour
{

    public class AppMessage
    {
        [CandidName("text")]
        public string Text { get; set; }

        [CandidName("timestamp")]
        public ulong Timestamp { get; set; }
    }

    [SerializeField] string gatewayUri = "wss://gateway.icws.io";//"ws://127.0.0.1:8080";

    private IWebSocketAgent<AppMessage> agent;
    private CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

    async void Start()
    {
        Principal canisterId = Principal.FromText(CandidApiManager.Instance.WORLD_CANISTER_ID);
        Uri _gatewayUri = new(gatewayUri);

        var builder = new WebSocketBuilder<AppMessage>(canisterId, _gatewayUri)
            .OnMessage(this.OnMessage)
            .OnOpen(this.OnOpen)
            .OnError(this.OnError)
            .OnClose(this.OnClose);

        //if (development)
        //{
        //    // Set the root key as the dev network key
        //    SubjectPublicKeyInfo devRootKey = await new HttpAgent(
        //        httpBoundryNodeUrl: new(devBoundryNodeUri)
        //    ).GetRootKeyAsync();
        //    builder = builder.WithRootKey(devRootKey);
        //}

        this.agent = await builder.BuildAsync(cancellationToken: cancellationTokenSource.Token);
        await this.agent.ReceiveAllAsync(cancellationTokenSource.Token);
    }

    void OnOpen()
    {
        Debug.Log("Open");
    }
    async void OnMessage(AppMessage message)
    {
        Debug.Log("Received Message: " + message.Text);
        //ICTimestamp.Now().NanoSeconds.TryToUInt64(out ulong now);
        //var replyMessage = new AppMessage
        //{
        //    Text = "pong",
        //    Timestamp = now
        //};
        //await this.agent.SendAsync(replyMessage, cancellationTokenSource.Token);
        //Debug.Log("Sent Message: " + replyMessage.Text);
    }
    void OnError(Exception ex)
    {
        Debug.Log("Error: " + ex);
    }
    void OnClose()
    {
        Debug.Log("Close");
    }

    async void OnDestroy()
    {
        cancellationTokenSource.Cancel(); // Cancel any ongoing operations
        if (this.agent != null)
        {
            if (this.agent.State == System.Net.WebSockets.WebSocketState.Aborted) return;
            await this.agent.DisposeAsync();
        }
    }
}