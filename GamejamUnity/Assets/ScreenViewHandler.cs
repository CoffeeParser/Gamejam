using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScreenViewHandler : MonoBehaviour {

    public GameObject StartScreen;
    public Button EnterMapMenu;


    public GameObject MapViewGameObject;
    public GameObject EvilScreenGameObject;
    public GameObject PatientScreenGameObject;
    public GameObject NightScreen;
    

    public GameObject DialogField;
    public Text dialogFieldtext;

    private GameState _gameState;

    public Button SkipDialogBtn;
    public Button SkipNightScreenBtn;

    public string LoadLevelString = "GyroTestScene";
    
    


    private int dialogIndex = 0;



    void Awake () {

        

        dialogIndex = 0;

        _gameState = GameObject.FindGameObjectWithTag("GlobalLifeTime").GetComponent<GameState>();

        StartScreen.SetActive(true);
        EnterMapMenu.onClick.AddListener(() =>
        {
            MapViewGameObject.SetActive(true);
            StartScreen.SetActive(false);
        });

        SkipDialogBtn.onClick.AddListener(SkipEvilDialogScreen);
        SkipNightScreenBtn.onClick.AddListener(SkipNightScreen);
    }


    private void SkipEvilDialogScreen()
    {
        EvilScreenGameObject.SetActive(false);
        PatientScreenGameObject.SetActive(true);
        SetPatientDialogText();
    }

    // Update is called once per frame
    void Update () {

    }

    void PersonChanged()
    {
        MapViewGameObject.SetActive(false);
        EvilScreenGameObject.SetActive(true);
        DialogField.SetActive(true);
        dialogFieldtext.text = _gameState.Begruessung;
    }

    public void SetPatientDialogText()
    {
        dialogIndex++;
        Debug.Log(dialogIndex);

        if (dialogIndex >= _gameState.CurrentPerson.TherapyStory.Count) // max
        {

            // hier ende, lade szene raum
            Debug.Log("End");
            DialogField.SetActive(false);
            PatientScreenGameObject.SetActive(false);
            NightScreen.SetActive(true);
            UIController.MenuisActive = false;
            return;
        }
        else
        {
            dialogFieldtext.text = (_gameState.CurrentPerson.TherapyStory[dialogIndex].Message);
        }

    }

    public void SkipNightScreen()
    {
        // Set MainMenu Activity false
        UIController.MenuisActive = false;
        // Load Level And wait until
        StartCoroutine( LoadAsynchonusly(LoadLevelString));
        
    }

    public IEnumerator LoadAsynchonusly(string sceneString)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneString);

        NightScreen.SetActive(true);

        while (!operation.isDone)
        {
            SkipNightScreenBtn.enabled = false;
            yield return null;
        }
        Debug.Log("LevelLoad is finish");
        NightScreen.SetActive(false);
    }


}
