using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


public class UIController : MonoBehaviour {


    /// <summary>
    /// Obsolete Script
    /// </summary>

    /// <summary>
    /// Hold all Menu UI without ScreenGroup
    /// </summary>

    [Header("MAIN MENU")]
    public GameObject MainMenu;
    public Button OpenMenu;
    public Button StartButton;
    public Button Options;
    public Button HighScore;
    public Button MenuBackButton;
    public Button Quit;

    [Header("Options Menu")]
    public GameObject OptionsMenu;
    [Header("Audio")]
    public Slider MasterVolume;
    public Slider MusicVolume;
    public Slider SFXVolume;


    [Header("GamePlay")]
    public Slider ShakeSensitivity;

    [Header("OptionsBack")]
    public Button OptionsBack;



    [Header("WinMenu")]
    public GameObject WinMenu;
    public Button RestartBtn;
    public Button WinQuit;

    [Header("PopUpMenu 1")]
    public GameObject PopUpMenu;
    public GameObject PopUpText;
    public Button SkipPopUp;




    [Header("LooseMenu")]
    public GameObject LooseMenu;
    public Button LooseRestart;
    public Button LooseQuit;


    public static bool MenuisActive = false;
    public static bool OptionsisActive = false;
    public static bool PauseMenuisActive = false;
    public static bool PopUpMenuIsActive = false;
    public static bool GameIsPaused = false;
    public static bool WinMenuisActive = false;
    public static bool LooseMenuisActive = false;

    public static UnityEvent DialogHasEnded;




   
    // Use this for initialization
    void Start () {

        //MenuisActive = true;

        MainMenu.SetActive(MenuisActive);
        OptionsMenu.SetActive(OptionsisActive);
        WinMenu.SetActive(WinMenuisActive);
        LooseMenu.SetActive(LooseMenuisActive);
        PopUpMenu.SetActive(PopUpMenuIsActive);

        // Main 
        OpenMenu.onClick.AddListener(OpenTheMenu);
        StartButton.onClick.AddListener(StartTheGame);
        Options.onClick.AddListener(OpenTheOptions);
        HighScore.onClick.AddListener(OpenHighScore);
        MenuBackButton.onClick.AddListener(CloseMenu);
        Quit.onClick.AddListener(QuitGame);

        //PopUpMenu
        SkipPopUp.onClick.AddListener(SkipPopUpMenu);

        //WinMenu
        RestartBtn.onClick.AddListener(StartTheGame);
        WinQuit.onClick.AddListener(QuitGame);

        //LooseMenu
        LooseQuit.onClick.AddListener(QuitGame);
        LooseRestart.onClick.AddListener(StartTheGame);


        // Options
        // Audio
        MasterVolume.value = 1;
        MusicVolume.value = 1;
        SFXVolume.value = 1;

        //LevelManager.instance.LoadByIndex(1);
    }


    // Update is called once per frame
    void Update()
    {     
        // MenuActicity
        MainMenu.SetActive(MenuisActive);
        OptionsMenu.SetActive(OptionsisActive);
        WinMenu.SetActive(WinMenuisActive);
        PopUpMenu.SetActive(PopUpMenuIsActive);
        LooseMenu.SetActive(LooseMenuisActive);


        // Audio
        SetMasterVolume();
    }




    public void IfGameHasLoose()
    {
        LooseMenuisActive = true;
        Time.timeScale = 0f;
    }



    /// <summary>
    /// MAIN MENU FUNCTIONS
    /// </summary>
    public void OpenTheMenu()
    {
        
        OptionsisActive = false;
        PauseMenuisActive = false;

        if (!MenuisActive)
        {
            MenuisActive = true;
            //Debug.Log("openMenu" + MenuisActive);
        }
        else
        {
            MenuisActive = false;
            //Debug.Log("closeMenu" + MenuisActive);
        }
    }

    private void StartTheGame()
    {

        Time.timeScale = 1f;
        // LoadLevel
        MenuisActive = true;
        OptionsisActive = false;
        PauseMenuisActive = false;
        WinMenuisActive = false;
        LooseMenuisActive = false;

        //With FadeScreen
        //LevelManager.instance.LoadByIndex(2);

        // With LoadingScreen
        StartCoroutine(LevelManager.instance.LoadAsynchonusly(1));
        //StartCoroutine(ScreenViewHandler.instance.LoadAsynchonusly("StartSceen"));

        MenuisActive = false;
    }
    

    private void OpenTheOptions()
    {
        MenuisActive = false;
        PauseMenuisActive = false;
        WinMenuisActive = false;
        if (!OptionsisActive)
        {
            OptionsisActive = true;
            Debug.Log("openMenu" + OptionsisActive);
        }
        else
        {
            OptionsisActive = false;
            Debug.Log("closeMenu" + OptionsisActive);
        }
    }

    private void OpenHighScore()
    {
        MenuisActive = false;
        OptionsisActive = false;
        PauseMenuisActive = false;
        WinMenuisActive = false;
        Debug.Log("HighScore Menu is Not Implemetet");
    }

    private void CloseMenu()
    {
        MenuisActive = false;
        OptionsisActive = false;
    }

    private void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    // Audio
    public void SetMasterVolume()
    {
        AudioListener.volume = GetMasterVolumeValue();
        if (GetMasterVolumeValue() < GetMusicVolumeValue() || GetMasterVolumeValue() < GetMusicVolumeValue())
        {
            MusicVolume.value = GetMasterVolumeValue();
            SFXVolume.value = GetMasterVolumeValue();
        } 
        //Debug.Log(AudioListener.volume);
    }

    public void SetMusicVolume()
    {
        AudioListener.volume = GetMusicVolumeValue();
        //Debug.Log(AudioListener.volume);
    }

    public void SetSFXVolume()
    {
        AudioListener.volume = GetSFXVolumeValue();
        //Debug.Log(AudioListener.volume);
    }

    

    public float GetMasterVolumeValue()
    {
        return MasterVolume.value;
    }
    public float GetMusicVolumeValue()
    {
        return MusicVolume.value;
    }
    public float GetSFXVolumeValue()
    {
        return SFXVolume.value;
    }


    // GamePlay 
    public float GetShakeSensitivity()
    {
        return ShakeSensitivity.value;
    }

    //PauseMenu
    public void OpenPauseMenu()
    {
        //Debug.Log("Paused");
        if (!GameIsPaused)
        {
            PauseMenuisActive = true;
            Time.timeScale = 0f;
            GameIsPaused = true;
        }
        else
        {
            PauseMenuisActive = false;
            GameIsPaused = false;
            Time.timeScale = 1f;
        }
    }

    //WinMenu

    public void OpenWinMenu()
    {
        MenuisActive = false;
        OptionsisActive = false;
        PauseMenuisActive = false;

        if (!WinMenuisActive)
        {
            WinMenuisActive = true;
        }
        else
        {
            WinMenuisActive = false;
        }
    }


    // PopUpMenu
    public void OpenPopUpMenu()
    {
        MenuisActive = false;
        OptionsisActive = false;
        PauseMenuisActive = false;
        WinMenuisActive = false;

        if (!PopUpMenuIsActive)
        {
            PopUpMenuIsActive = true;
        }
        else
        {
            PopUpMenuIsActive = false;
        }
    }

    public void SkipPopUpMenu()
    {
        Time.timeScale = 1f;
        PopUpMenuIsActive = false;

    }
    public struct PopUp
    {
        public string name;
        public string headLine;
        public string text1;
        public string text2;
    }

}
