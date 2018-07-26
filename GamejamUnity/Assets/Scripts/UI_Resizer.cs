using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class UI_Resizer : MonoBehaviour {

    public float widthPercentage;
    public float heightPercentage;
    private Image image;

	// Use this for initialization
	void Start () {
        image = GetComponent<Image>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void rescaleImageBasedOnHeight()
    {

    }

    void rescaleImageBasedOnWidth()
    {

    }
}
