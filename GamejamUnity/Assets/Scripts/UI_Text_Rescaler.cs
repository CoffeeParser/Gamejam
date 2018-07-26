﻿using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class UI_Text_Rescaler : MonoBehaviour
{

    public float percentageTextScale;
    private RectTransform rect;
    private Text text;

    private void Start()
    {
        Init();
    }

    // Use this for initialization
    public void Init()
    {
        rect = GetComponent<RectTransform>();
        text = GetComponent<Text>();
    }

    public void RescaleTextSize()
    {
        text.fontSize = (int)GetPercentageHeight(percentageTextScale);
        text.lineSpacing = (text.fontSize / 45.0f) + 0.5f;
    }

    public float GetPercentageWidth(float percentage)
    {
        return Camera.main.pixelWidth * percentage;
    }

    public float GetPercentageHeight(float percentage)
    {
        return Camera.main.pixelHeight * percentage;
    }

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