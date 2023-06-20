namespace ItsJackAnton.Utility
{
    using UnityEngine;
    using UnityEngine.SceneManagement;

    public static class MainUtil
    {
        public static void Loop(this int count, System.Action action)
        {
            for (int i = 0; i < count; i++)
            {
                action();
            }
        }
        public static void Loop(this int count, System.Action<int> action)
        {
            for (int i = 0; i < count; i++)
            {
                action(i);
            }
        }

        #region World_UI
        public static TextMesh CreateWorldText(this string text, Vector3 localPosition = default(Vector3), Transform parent = null, int scale = 1, Color? color = null, TextAnchor textAnchor = TextAnchor.MiddleCenter, TextAlignment textAlignment = TextAlignment.Center, int sortingOrder = 5000)
        {
            if (color == null) color = Color.white;

            GameObject gameObject = new GameObject("World_Text", typeof(TextMesh));
            Transform transform = gameObject.transform;
            transform.SetParent(parent, false);
            transform.localPosition = localPosition;
            TextMesh textMesh = gameObject.GetComponent<TextMesh>();
            textMesh.anchor = textAnchor;
            textMesh.alignment = textAlignment;
            textMesh.text = text;
            textMesh.characterSize = .05f * scale;
            textMesh.fontSize = 50;
            textMesh.color = (Color)color;
            textMesh.GetComponent<MeshRenderer>().sortingOrder = sortingOrder;
            return textMesh;
        }
        #endregion

        #region Pause Unpause
        public static void Pause() => Time.timeScale = 0;
        public static void Unpause() => Time.timeScale = 1;
        public static bool IsPaused() => Time.timeScale == 0;
        #endregion

        #region Cursor
        public static void HideCursor()
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        public static void ShowCursor()
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        public static bool IsCursorVisible()
        {
            return Cursor.visible;
        }
        #endregion

        #region Layers
        public static LayerMask SumLayers(this LayerMask a, LayerMask b)
        {
            return a.value | b.value;
        }
        public static bool HasLayerOf(this GameObject target, int layerMask)
        {
            return (layerMask & (1 << target.layer)) != 0;
        }
        #endregion

        #region On Value Change
        public static T OnChange<T>(this T oldVal, T newVal, System.Action<T> onChange) where T : struct
        {
            if (newVal.Equals(oldVal) == false) onChange.Invoke(newVal);
            return newVal;
        }
        public static T OnObjChange<T>(this T oldVal, T newVal, System.Action<T> onChange) where T : Object
        {
            if (newVal != oldVal) onChange.Invoke(newVal);
            return newVal;
        }

        public static bool OnTrigger(this bool oldVal, bool newVal, System.Action onChange, bool ifNewValIsTrue = true)
        {
            if (newVal.Equals(oldVal) == false)
            {
                if (newVal == ifNewValIsTrue) onChange.Invoke();
            }
            return newVal;
        }
        #endregion

        #region GO TO SCENE
        public static void GoToScene(int index)
        {
            if (index < 0 || index >= SceneManager.sceneCountInBuildSettings)
            {
                ("Invalid Scene Index: " + index + ". Scene count: " + SceneManager.sceneCountInBuildSettings).Warning(nameof(MainUtil));
                return;
            }
            SceneManager.LoadScene(index);
        }
        public static void GoToScene(string name)
        {
            int _sceneIndex = SceneUtility.GetBuildIndexByScenePath(name);

            if (_sceneIndex > -1) GoToScene(_sceneIndex);
            else ("Invalid Scene Name: " + name + " is not valid").Warning(nameof(MainUtil));

        }
        #endregion
    }
}