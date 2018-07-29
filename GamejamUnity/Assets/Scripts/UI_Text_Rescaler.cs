using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

namespace DrEvil.Visuals
{
    /// <summary>
    /// Simple class to rescale text font size depending on screen size
    /// </summary>
    public class UI_Text_Rescaler : MonoBehaviour
    {

        public float percentageTextScale;
        private RectTransform rect;
        private Text text;
        private bool isInit;
        public bool isMagementMaster;

        private void Start()
        {
            Init();
        }

        // Use this for initialization
        public void Init()
        {
            isInit = true;
            rect = GetComponent<RectTransform>();
            text = GetComponent<Text>();
        }

        public void RescaleTextSize()
        {
            text.fontSize = (int)GetPercentageHeight(percentageTextScale);
            text.lineSpacing = (text.fontSize / 60.0f) + 0.2f;
        }

        public float GetPercentageWidth(float percentage)
        {
            return Camera.main.pixelWidth * percentage;
        }

        public float GetPercentageHeight(float percentage)
        {
            return Camera.main.pixelHeight * percentage;
        }

        private void OnEnable()
        {
            if (!isInit && !isMagementMaster)
            {
                Init();
                RescaleTextSize();
            }
        }
        /// <summary>
        /// Apply all changes in all child objects
        /// </summary>
        public void ApplyAllResizeObjs()
        {
            UI_Text_Rescaler[] resizeObjs = GetComponentsInChildren<UI_Text_Rescaler>();

            foreach (UI_Text_Rescaler resizeObj in resizeObjs)
            {
                if (resizeObj != this)
                {
                    resizeObj.Init();
                    resizeObj.RescaleTextSize();
                }
            }
        }
    }

#if UNITY_EDITOR
    /// <summary>
    /// Simple Class for accessing the control buttons to editor runtime
    /// </summary>
    [CustomEditor(typeof(UI_Text_Rescaler))]
    public class UI_Text_Rescaler_customEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            UI_Text_Rescaler resizer = (UI_Text_Rescaler)target;
            if (GUILayout.Button("Scale Text"))
            {
                resizer.Init();
                resizer.RescaleTextSize();
            }

            if (GUILayout.Button("Apply All Text scale"))
            {
                resizer.Init();
                resizer.ApplyAllResizeObjs();
            }
        }
    }

#endif
}