namespace Boom.Mono
{
    using Boom.Utility;
    using UnityEngine;

    public class HideCursor : MonoBehaviour
    {
        [SerializeField] bool show;
        private void Start()
        {
            if(show) MainUtil.ShowCursor();
            else MainUtil.HideCursor();

        }
    }

}