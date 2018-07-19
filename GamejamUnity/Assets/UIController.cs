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





    private bool MenuisActive = false;
    private bool OptionsisActive = false;



    // Use this for initialization
    void Start () {
        MainMenu.SetActive(MenuisActive);
        OptionsMenu.SetActive(OptionsisActive);

        // Main 
        OpenMenu.onClick.AddListener(OpenTheMenu);
        StartButton.onClick.AddListener(StartTheGame);
        Options.onClick.AddListener(OpenTheOptions);
        HighScore.onClick.AddListener(OpenHighScore);
        Quit.onClick.AddListener(QuitGame);

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


        LevelManager.instance.LoadByIndex(1);
    }


    // Update is called once per frame
    void Update()
    {     
        MainMenu.SetActive(MenuisActive);
        OptionsMenu.SetActive(OptionsisActive);
    }


    /// <summary>
    /// MAIN MENU FUNCTIONS
    /// </summary>
    public void OpenTheMenu()
    {
        OptionsisActive = false;

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
        // LoadLevel
        MenuisActive = false;
        OptionsisActive = false;
        
        LevelManager.instance.LoadByIndex(0);

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
        Debug.Log("HighScore Menu is Not Implemetet");
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



}
