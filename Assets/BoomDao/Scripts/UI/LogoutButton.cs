using Boom.Patterns.Broadcasts;
using Boom;
namespace Boom.UI
{
    using UnityEngine;
    using UnityEngine.UI;

    [RequireComponent(typeof(Button))]
    public class LogoutButton : MonoBehaviour
    {
        [SerializeField] Button button;

        //Register to events
        private void Awake()
        {
            button.onClick.AddListener(Handler);

            BroadcastState.Register<WaitingForResponse>(AllowButtonInteractionHandler, true);

            UserUtil.AddListenerMainDataChange<MainDataTypes.LoginData>(AllowButtonInteractionHandler, true);
        }

        //Unregister from events
        private void OnDestroy()
        {
            button.onClick.RemoveListener(Handler);

            BroadcastState.Unregister<WaitingForResponse>(AllowButtonInteractionHandler);

            UserUtil.RemoveListenerMainDataChange<MainDataTypes.LoginData>(AllowButtonInteractionHandler);
        }

        //Handle whether or not the button must be interactable
        private void AllowButtonInteractionHandler(WaitingForResponse response)
        {
            if (button) button.interactable = !response.value;
        }
        //Handle whether or not the button must be disabled
        private void AllowButtonInteractionHandler(MainDataTypes.LoginData data)
        {
            button.gameObject.SetActive(data.state == MainDataTypes.LoginData.State.LoggedIn);
        }

        //Execute Logout
        public void Handler()
        {
            Broadcast.Invoke<UserLogout>();
        }
    }
}
