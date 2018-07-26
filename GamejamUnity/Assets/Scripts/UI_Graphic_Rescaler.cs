using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

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

    public void rescaleImageBasedOnHeight()
    {
        float newHeight = GetPercentageHeight(heightPercentage);
        rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, newHeight);

        if (!looseAspectRatio)
        {
            float newWidth = newHeight * GetAspectRatioHeight();
            rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, newWidth);
        }
    }

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

    public void setPercentageOffset()
    {
        Vector3 newPos = new Vector3(GetPercentageWidth(offsetX_percentage), GetPercentageHeight(offsetY_percentage), 0);
        rect.SetPositionAndRotation(newPos, Quaternion.identity);
    }

    public float GetAspectRatioHeight()
    {
        return image.sprite.rect.width / image.sprite.rect.height;
    }

    public float GetAspectRatioWidth()
    {
        return image.sprite.rect.height / image.sprite.rect.width;
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
            if (looseAspectRatio)
                rescaleImageBasedOnWidth();
            rescaleImageBasedOnHeight();
            setPercentageOffset();
        }
    }

    public void ApplyAllResizeObjs()
    {
        UI_Graphic_Rescaler[] resizeObjs = GetComponentsInChildren<UI_Graphic_Rescaler>();
        Debug.Log(resizeObjs.Length);
        foreach (UI_Graphic_Rescaler resizeObj in resizeObjs)
        {
            if (resizeObj != this)
            {
                resizeObj.Init();
                if (resizeObj.looseAspectRatio)
                    resizeObj.rescaleImageBasedOnWidth();
                resizeObj.rescaleImageBasedOnHeight();
                resizeObj.setPercentageOffset();
            }
        }
    }
}


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
            resizer.rescaleImageBasedOnHeight();
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