using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace DrEvil.Visuals
{
    public class UI_Graphic_Rescaler : MonoBehaviour
    {

        public float widthPercentage;
        public float heightPercentage;
        public float offsetY_percentage;
        public float offsetX_percentage;
        private Image image;
        private RectTransform rect;
        public bool looseAspectRatio;
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
            image = GetComponent<Image>();
            rect = GetComponent<RectTransform>();
        }

        /// <summary>
        ///Rescale the ui element depending on the height value
        /// </summary>
        public void RescaleImageBasedOnHeight()
        {
            float newHeight = GetPercentageHeight(heightPercentage);
            rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, newHeight);

            if (!looseAspectRatio)
            {
                float newWidth = newHeight * GetAspectRatioHeight();
                rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, newWidth);
            }
        }

        /// <summary>
        /// Rescale the ui element depending on the width value
        /// </summary>
        public void rescaleImageBasedOnWidth()
        {
            float newWidth = GetPercentageWidth(widthPercentage);
            rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, newWidth);

            if (!looseAspectRatio)
            {
                float newHeight = newWidth * GetAspectRatioWidth();
                rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, newHeight);
            }
        }

        /// <summary>
        /// Apply the percante offset to the ui element
        /// </summary>
        public void setPercentageOffset()
        {
            Vector3 newPos = new Vector3(GetPercentageWidth(offsetX_percentage), GetPercentageHeight(offsetY_percentage), 0);
            rect.SetPositionAndRotation(newPos, Quaternion.identity);
        }

        /// <summary>
        /// Get Aspect ratio depending on height od the device size
        /// </summary>
        /// <returns></returns>
        public float GetAspectRatioHeight()
        {
            return image.sprite.rect.width / image.sprite.rect.height;
        }

        /// <summary>
        /// Get Aspect ratio depending on width od the device size
        /// </summary>
        /// <returns></returns>
        public float GetAspectRatioWidth()
        {
            return image.sprite.rect.height / image.sprite.rect.width;
        }

        /// <summary>
        /// Percentage widht depending on screenwidth
        /// </summary>
        /// <param name="percentage"></param>
        /// <returns></returns>
        public float GetPercentageWidth(float percentage)
        {
            return Camera.main.pixelWidth * percentage;
        }

        /// <summary>
        /// Percentage height depending on screenheight
        /// </summary>
        /// <param name="percentage"></param>
        /// <returns></returns>
        public float GetPercentageHeight(float percentage)
        {
            return Camera.main.pixelHeight * percentage;
        }

        private void OnEnable()
        {
            if (!isInit && !isMagementMaster)
            {
                Init();
                if (looseAspectRatio)
                    rescaleImageBasedOnWidth();
                RescaleImageBasedOnHeight();
                setPercentageOffset();
            }
        }

        /// <summary>
        /// Apply all rescalable objects in the child of this object
        /// </summary>
        public void ApplyAllResizeObjs()
        {
            UI_Graphic_Rescaler[] resizeObjs = GetComponentsInChildren<UI_Graphic_Rescaler>();
            foreach (UI_Graphic_Rescaler resizeObj in resizeObjs)
            {
                if (resizeObj != this)
                {
                    resizeObj.Init();
                    if (resizeObj.looseAspectRatio)
                        resizeObj.rescaleImageBasedOnWidth();
                    resizeObj.RescaleImageBasedOnHeight();
                    resizeObj.setPercentageOffset();
                }
            }
        }
    }

#if UNITY_EDITOR
    /// <summary>
    /// Custom editor for accessing the rescale methods in editor runtime
    /// </summary>
    [CustomEditor(typeof(UI_Graphic_Rescaler))]
    public class UI_Graphic_Rescaler_customEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            UI_Graphic_Rescaler resizer = (UI_Graphic_Rescaler)target;
            if (GUILayout.Button("Resize depending width"))
            {
                resizer.Init();
                resizer.rescaleImageBasedOnWidth();
            }
            if (GUILayout.Button("Resize depending height"))
            {
                resizer.Init();
                resizer.RescaleImageBasedOnHeight();
            }
            if (GUILayout.Button("Set percentage Offset"))
            {
                resizer.Init();
                resizer.setPercentageOffset();
            }
            if (GUILayout.Button("Apply all resize Objs"))
            {
                resizer.ApplyAllResizeObjs();
            }
        }
    }
#endif
}
