using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;

public class ScreenViewHandler : MonoBehaviour
{
    public static ScreenViewHandler instance;

    public GameObject StartScreen;
    public Button EnterMapMenu;


    public GameObject MapViewGameObject;
    public GameObject EvilScreenGameObject;
    public GameObject PatientScreenGameObject;
    public GameObject NightScreen;

    public GameObject LeaveFinishScreen;
    public Button LeaveFinishVBtn;

    public GameObject LeaveUnFinishScreen;
    public Button LeaveUnFinishVBtn;

    public GameObject achievementScreen;
    public Button achievementBtn;

    public GameObject DialogField;
    public Text dialogFieldtext;

    private GameState _gameState;

    public Button SkipDialogBtn;
    public Button SkipNightScreenBtn;

    public string LoadLevelString = "GyroTestScene";
    private int dialogIndex = -1;

    private bool acc;

    void Awake()
    {

        if (instance == null)
            instance = this;

        // -1 da der Dr auch immer eine Satz sagt 
        dialogIndex = -1;

        //To read Gamestate
        _gameState = GameObject.FindGameObjectWithTag("GlobalLifeTime").GetComponent<GameState>();

        // First of all set Start Screen true
        StartScreen.SetActive(true);

        // After Click show MapMenu
        EnterMapMenu.onClick.AddListener(() =>
        {
            MapViewGameObject.SetActive(true);
            StartScreen.SetActive(false);
        });

        //Add Listeners to Skip Evil Dialog
        SkipDialogBtn.onClick.AddListener(SkipEvilDialogScreen);

        //Add Listeners to Skip Evil Dialog
        SkipNightScreenBtn.onClick.AddListener(SkipNightScreen);

        // Add Listener to AchievementScreen
        achievementBtn.onClick.AddListener(SkipNightScreen);
    }

    // Hier beginnt alles 
    void PersonChanged()
    {
        dialogIndex = -1;
        MapViewGameObject.SetActive(false);
        EvilScreenGameObject.SetActive(true);
        DialogField.SetActive(true);
        dialogFieldtext.text = _gameState.Begruessung;
    }

    // On EvilDialog Skip, Evil Screen = false // setze 
    private void SkipEvilDialogScreen()
    {
        EvilScreenGameObject.SetActive(false);
        PatientScreenGameObject.SetActive(true);
        SetPatientDialogText();
    }

    public void SetPatientDialogText()
    {
        // Check witch Action is Solved 
        // If Solved Actions is empty write TherapieStroy
        // Is Solved Actions is NOT Empty write
    


        //Debug.Log(_gameState.CurrentPerson.SolvedActions.Count);
        //Debug.Log(_gameState.CurrentPerson.EvilAction.Count);

        dialogIndex++;
        //Debug.Log(dialogIndex);

        // How many Actions are to do
        int evilActions = _gameState.CurrentPerson.EvilAction.Count;

        // How many Actions are solved
        int solvedActions = _gameState.CurrentPerson.SolvedActions.Count;


        // Zeigt ThearpieStory
        if (solvedActions == 0)
        {
            ShowTherapieDialog();
        }

        // All Actions Are Solved
        else if (evilActions == 0)
        {
            ShowSolvedDialog();
        }

        // Show PartiallSolved Story
        else if (evilActions != 0 && solvedActions !=0)
        {
            ShowPartiallySolvedDialog();
        }     
    }

    public void ShowTherapieDialog()
    {
        // Show TherapieStory again
        if (dialogIndex >= _gameState.CurrentPerson.TherapyStory.Count) // max
        {

            //hier ende, lade szene raum
            //Debug.Log("End");
            NightScreen.SetActive(true);
            DialogField.SetActive(false);
            PatientScreenGameObject.SetActive(false);
            StartCoroutine(LoadAsynchonusly(LoadLevelString));

            dialogIndex = -1;
            return;
        }
        else
        {
            dialogFieldtext.text = (_gameState.CurrentPerson.TherapyStory[dialogIndex].Message);
        }
    }
    public void ShowSolvedDialog()
    {
        if (dialogIndex >= _gameState.CurrentPerson.ReviewStory.Count)
        {
            achievementScreen.SetActive(true);
            DialogField.SetActive(false);
            PatientScreenGameObject.SetActive(false);
            dialogIndex = -1;
            return;
            //später MiniMap ZUrück
        }
        else
        {
            dialogFieldtext.text = _gameState.CurrentPerson.ReviewStory[dialogIndex].DialogType.Equals("Story") ? (_gameState.CurrentPerson.ReviewStory[dialogIndex].Message) : (_gameState.CurrentPerson.ReviewStory[dialogIndex].SolvedMessage);
        }
    }


    public void ShowPartiallySolvedDialog()
    {
        if (dialogIndex >= _gameState.CurrentPerson.ReviewStory.Count)
        {
            NightScreen.SetActive(true);
            DialogField.SetActive(false);
            PatientScreenGameObject.SetActive(false);
            dialogIndex = -1;
            return;
        }
        else
        {
            ReviewStory currentReviewStory = _gameState.CurrentPerson.ReviewStory[dialogIndex];
            if (currentReviewStory.DialogType.Equals("Story"))
            {
                dialogFieldtext.text = currentReviewStory.Message;
            }
            else if (currentReviewStory.DialogType.Equals("ActionStatus")) // DialogType = ActionStatus
            {
                if (_gameState.CurrentPerson.SolvedActions.First(b => b.ActionType.Equals(currentReviewStory.ActionType)) != null)
                {
                    dialogFieldtext.text = currentReviewStory.SolvedMessage;
                }
                else
                {
                    dialogFieldtext.text = currentReviewStory.FailedMessage;
                }
            }
        }
    }

    public void SkipAchievementScreen()
    {
        achievementScreen.SetActive(false);
    }


    public void SkipNightScreen()
    {
        NightScreen.SetActive(false);
    }


    public IEnumerator LoadAsynchonusly(string sceneString)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneString, LoadSceneMode.Additive);

        Debug.Log("LoadLevel");

        NightScreen.SetActive(true);

        while (!operation.isDone)
        {
            Debug.Log(".");
            SkipNightScreenBtn.enabled = false;
            yield return null;
        }
        SkipNightScreenBtn.enabled = true;
        //Debug.Log("LevelLoad is finish");
        //NightScreen.SetActive(false);
        //LevelAccomplishTest.instance.onLevelIsOver.AddListener(isLevelAccomplished);
    }

    public void isLevelAccomplished(bool isAccomplished)
    {
        // UnLoadlevel after ever Session
        SceneManager.UnloadSceneAsync(LoadLevelString);


        // Obsolet maybe for alternativ loadScreen
        if (isAccomplished)
        {
            // Finish leave Screen, only skipable when Level is Unload      
            LeaveFinishScreen.SetActive(true);
            LeaveFinishVBtn.onClick.AddListener(LoadSecondDialog);
        }
        else 
        {
            // Unfinsih LeaveScreen Only Skipable wenn Level is unloaded 
            LeaveUnFinishScreen.SetActive(true);
            LeaveUnFinishVBtn.onClick.AddListener(LoadSecondDialog);
        }
    }


    public void LoadSecondDialog()
    {
        // Wenn nichts gelöst TherapiheStroy von vorne 


        // Wenn teile gelöst dann Review Story anhand der:
        // Solved actions

        //  Wenn alle Gelöst Solved actins count = Actions to do Count  
        //    EinweisungsScreen 
        // Dann minimap (muss dann umgeschaltet sein)


        LeaveFinishScreen.SetActive(false);
        LeaveUnFinishScreen.SetActive(false);
        PersonChanged();
    }
}