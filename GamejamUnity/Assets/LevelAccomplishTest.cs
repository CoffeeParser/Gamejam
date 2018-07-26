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
        test.onClick.AddListener(PartFaiLevel);

    }

    void PartFaiLevel()
    {
        List<EvilAction> evilActios = GameState.instance.CurrentPerson.EvilAction;
        foreach (EvilAction ea in evilActios)
        {
            GameState.instance.CurrentPerson.SolvedActions.Add(ea);
            break;
        }
        evilActios.RemoveAt(0);
        ScreenViewHandler.instance.FinalizeLevel(true);
    }

    void AccomplishLevel()
    {
        List<EvilAction> evilActios = GameState.instance.CurrentPerson.EvilAction;
        foreach (EvilAction ea in evilActios)
        {
            GameState.instance.CurrentPerson.SolvedActions.Add(ea);
        }
        GameState.instance.CurrentPerson.EvilAction.Clear();
        ScreenViewHandler.instance.FinalizeLevel(true);
    }

    // Update is called once per frame
    void Update () {
       
	}
}

// Bool Event

class LevelIsOver : UnityEvent<bool>
{

}
