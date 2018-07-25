using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;

public class ScreenViewHandler : MonoBehaviour
{
    public static ScreenViewHandler instance;

    public DialogPlayer DialogPlayer;

    public GameObject StartScreen;
    public Button EnterMapMenu;

    public GameObject MapViewGameObject;

    public GameObject NightScreen;

    public GameObject LeaveFinishScreen;
    public Button LeaveFinishVBtn;

    public GameObject LeaveUnFinishScreen;
    public Button LeaveUnFinishVBtn;

    //public GameObject achievementScreen;
    //public Button achievementBtn;

    private GameState _gameState;
    public Button SkipNightScreenBtn;

    public string LoadLevelString = "GyroTestScene";

    private bool acc;

    /// <summary>
    /// Init Screens...
    /// </summary>
    void Awake()
    {

        if (instance == null)
            instance = this;

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
        SkipNightScreenBtn.onClick.AddListener(SkipNightScreen);

        // Add Listener to AchievementScreen
        //achievementBtn.onClick.AddListener(SkipNightScreen);
    }

    // First Entry Point After MiniMap -> Play TherapyStory
    void PersonChanged()
    {
        MapViewGameObject.SetActive(false);
        DialogPlayer.PlayTherapyStory(_gameState.CurrentPerson.TherapyStory, _gameState.CurrentPerson);
    }

    // Second Entry Point After Night Scene -> Play ReviewStory
    void NightEnded()
    {
        LeaveFinishScreen.SetActive(false);
        LeaveUnFinishScreen.SetActive(false);
        DialogPlayer.PlayReviewStory(_gameState.CurrentPerson.ReviewStory, _gameState.CurrentPerson);
    }

    //public void SkipAchievementScreen()
    //{
    //    achievementScreen.SetActive(false);
    //}


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

    // Für robert, spaeter weg
    public void isLevelAccomplished(bool isAccomplished)
    {
        // UnLoadlevel after ever Session
        SceneManager.UnloadSceneAsync(LoadLevelString);


        // Obsolet maybe for alternativ loadScreen
        if (isAccomplished)
        {
            // Finish leave Screen, only skipable when Level is Unload      
            LeaveFinishScreen.SetActive(true);
            LeaveFinishVBtn.onClick.AddListener(NightEnded);
        }
        else 
        {
            // Unfinsih LeaveScreen Only Skipable wenn Level is unloaded 
            LeaveUnFinishScreen.SetActive(true);
            LeaveUnFinishVBtn.onClick.AddListener(NightEnded);
        }
    }

    // Callback from DialogPlayer
    public void EnterNightScene()
    {
        NightScreen.SetActive(true);
        StartCoroutine(LoadAsynchonusly(LoadLevelString));
    }

    public void EnterMiniMapMenu()
    {
        //achievementScreen.SetActive(true);
        MapViewGameObject.SetActive(true);
    }

}