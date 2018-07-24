using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ScreenViewHandler : MonoBehaviour {

    public GameObject StartScreen;
    public Button EnterMapMenu;
    public GameObject MapViewGameObject;
    public GameObject EvilScreenGameObject;

    void Start () {
        StartScreen.SetActive(true);
        EnterMapMenu.onClick.AddListener(() =>
        {
            MapViewGameObject.SetActive(true);
            StartScreen.SetActive(false);
        });
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void PersonChanged()
    {
        MapViewGameObject.SetActive(false);
        EvilScreenGameObject.SetActive(true);
    }
}
