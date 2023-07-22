using UnityEngine;
using System;
using WebSocketSharp.Server;
using WebSocketSharp;

namespace Candid
{
    public class LoginManager : MonoBehaviour
    {
        public static LoginManager  Instance;
        private Action<string> callback = null;

        [SerializeField]
        string url = "https://7p3gx-jaaaa-aaaal-acbda-cai.raw.ic0.app/";

        void Awake()
        {
            Instance = this;
        }

        /// <summary>
        /// This is the login flow using localstorage for WebGL
        /// </summary>
        public void StartLoginFlowWebGl(Action<string> _callback = null)
        {
            Debug.Log("Starting WebGL Login Flow");
            callback = _callback;
            BrowserUtils.ToggleLoginIframe(true);
        }

        public void ExecuteCallbackWithJson(string identityJson)
        {
            callback?.Invoke(identityJson);
            callback = null;
            BrowserUtils.ToggleLoginIframe(false);
        }

        public void CancelLogin()
        {
            BrowserUtils.ToggleLoginIframe(false);
            if (wssv != null)
            {
                wssv.Stop();
                wssv = null;
            }
        }

        /// <summary>
        /// This is the login flow using websockets for PC, Mac, iOS, and Android
        /// </summary>
        public void StartLoginFlow(Action<string> _callback = null)
        {
            callback = _callback;
            StartSocket();

            Application.OpenURL(url);
        }

        WebSocketServer wssv;

        private void StartSocket()
        {
            wssv = new WebSocketServer("ws://127.0.0.1:8080");
            wssv.AddWebSocketService<Data>("/Data");
            wssv.Start();
        }

        public void CloseSocket(string identity)
        {
            Debug.Log("CloseWebSocket");

            wssv.Stop();
            wssv = null;

            ExecuteCallbackWithJson(identity);
        }
    }

    public class Data : WebSocketBehavior
    {
        protected override void OnMessage(MessageEventArgs e)
        {
            Debug.Log("Websocket Message Received: " + e.Data);

            LoginManager.Instance.CloseSocket(e.Data);
        }
    }

}