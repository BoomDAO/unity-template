namespace Boom.UI
{
    using Boom.Patterns.Broadcasts;
    using Boom;
    using UnityEngine;
    using UnityEngine.UI;

    [RequireComponent(typeof(Button))]
    public class LoginButton : MonoBehaviour
    {
        [SerializeField] Button button;
        [SerializeField, ShowOnly] bool noneInteractable;

        //Register to events
        private void Awake()
        {
            button.onClick.AddListener(Handler);

            BroadcastState.Register<WaitingForResponse>(AllowButtonInteractionHandler, true);

            UserUtil.AddListenerMainDataChange<MainDataTypes.LoginData>(EnableButtonHandler, true);
        }

        //Unregister from events
        private void OnDestroy()
        {
            button.onClick.RemoveListener(Handler);

            BroadcastState.Unregister<WaitingForResponse>(AllowButtonInteractionHandler);

            UserUtil.RemoveListenerMainDataChange<MainDataTypes.LoginData>(EnableButtonHandler);
        }

        //Handle whether or not the button must be interactable
        private void AllowButtonInteractionHandler(WaitingForResponse response)
        {
            noneInteractable = response.value;
            if (button) button.interactable = !response.value;
        }
        //Handle whether or not the button must be disabled
        private void EnableButtonHandler(MainDataTypes.LoginData data)
        {
            button.gameObject.SetActive(data.state != MainDataTypes.LoginData.State.LoggedIn);
        }

        //Execute Login Request
        public void Handler()
        {
            Broadcast.Invoke<UserLoginRequest>();
        }
    }
}
