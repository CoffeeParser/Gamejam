using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class LevelAccomplishTest : MonoBehaviour {

    public static LevelAccomplishTest instance;
    public Button test;

    public UnityEvent<bool> onLevelIsOver;


    // Use this for initialization
    void Awake () {
        instance = this;
        onLevelIsOver = new LevelIsOver();

        test = GetComponentInChildren<Button>();
        if (test != null)
        {
            AddEventListener();
        }
    }

    private void Start()
    {


        //Debug.Log(ScreenViewHandler.instance == null);
        //test.onClick.AddListener(delegate () { onLevelIsOver.Invoke(true); });
    }

    void AddEventListener()
    {
        //Debug.Log("CALL IT FUCKING BITCH!!!");
        test.onClick.AddListener(delegate () { ScreenViewHandler.instance.isLevelAccomplished(true); });

    }

    // Update is called once per frame
    void Update () {
       
	}
}

// Bool Event

class LevelIsOver : UnityEvent<bool>
{

}
