using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


public class UIController : MonoBehaviour {

    

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

    [Header("Video")]
    public Slider Bightness;
    public Toggle Antialiasing;
    public Toggle Toggel;

    [Header("GamePlay")]
    public Slider ShakeSensitivity;

    [Header("OptionsBack")]
    public Button OptionsBack;


    [Header("PauseMenu")]
    public GameObject PauseMenu;
    public Button PauseBtn;


    [Header("WinMenu")]
    public GameObject WinMenu;
    public Button RestartBtn;
    public Button WinQuit;



    private static bool MenuisActive = false;
    private bool OptionsisActive = false;
    private bool PauseMenuisActive = false;

    public static bool GameIsPaused = false;
    public static bool WinMenuisActive = false;



    // Use this for initialization
    void Start () {

        MenuisActive = true;

        MainMenu.SetActive(MenuisActive);
        OptionsMenu.SetActive(OptionsisActive);
        PauseMenu.SetActive(PauseMenuisActive);
        WinMenu.SetActive(WinMenuisActive);

        // Main 
        OpenMenu.onClick.AddListener(OpenTheMenu);
        StartButton.onClick.AddListener(StartTheGame);
        Options.onClick.AddListener(OpenTheOptions);
        HighScore.onClick.AddListener(OpenHighScore);
        MenuBackButton.onClick.AddListener(CloseMenu);
        Quit.onClick.AddListener(QuitGame);

        //PauseMenu
        PauseBtn.onClick.AddListener(OpenPauseMenu);

        //WinMenu
        RestartBtn.onClick.AddListener(StartTheGame);
        WinQuit.onClick.AddListener(QuitGame);

        // Options
        // Audio
        MasterVolume.value = 1;
        MusicVolume.value = 1;
        SFXVolume.value = 1;
        // Video
        Bightness.value = 50;

        Antialiasing.isOn = false;
        Antialiasing.onValueChanged.AddListener(delegate
        {
            AntialiasingToggleHasChanges(Antialiasing);
        });
        OptionsBack.onClick.AddListener(OpenTheMenu);

        Toggel.isOn = false;
        // GamePlay
        ShakeSensitivity.value = 1;


        //LevelManager.instance.LoadByIndex(1);
    }


    // Update is called once per frame
    void Update()
    {     
        // MenuActicity
        MainMenu.SetActive(MenuisActive);
        OptionsMenu.SetActive(OptionsisActive);
        PauseMenu.SetActive(PauseMenuisActive);
        WinMenu.SetActive(WinMenuisActive);

        // Audio
        SetMasterVolume();
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

        LevelManager.instance.LoadByIndex(2);
        MenuisActive = false;
        //GetComponent<LevelManager>().LoadByIndex(0);
        /*
        try
        {
            GetComponent<LevelManager>().LoadByIndex(0);
        }
        catch (Exception e)
        {
            Debug.Log("Cant Load Scene");
        }
        */
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

    /// <summary>
    /// OptionsMenu Functions
    /// </summary>

    // Audio
    public void SetMasterVolume()
    {
        AudioListener.volume = GetMasterVolumeValue();
        //Debug.Log(AudioListener.volume);
    }

    public float GetMasterVolumeValue()
    {
        return MasterVolume.value;
    }
    public float GetMusicVolumeValue()
    {
        return MasterVolume.value;
    }
    public float GetSFXVolumeValue()
    {
        return MasterVolume.value;
    }

    // Video
    public float GetBightnessValue()
    {
        return Bightness.value;
    }

    public bool AntialiasingValue()
    {
        return Antialiasing.isOn;
    }

    private void AntialiasingToggleHasChanges(Toggle antialiasing)
    {
        Debug.Log(Antialiasing.isOn);
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


}
