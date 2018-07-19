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

    [Header("PopUpMenu 1")]
    public GameObject PopUpMenu;
    public GameObject PopUpText;
    public Button SkipPopUp;

    [Header("HealthBar")]
    public Image currentHealthBar;
    public Text ratioText;
    public float health;
    public bool isDamaging;
    public float damage = 10;
    private float hitPoint = 150;
    private float maxHitPoint = 150;



    public static bool MenuisActive = false;
    public static bool OptionsisActive = false;
    public static bool PauseMenuisActive = false;
    public static bool PopUpMenuIsActive = false;
    public static bool GameIsPaused = false;
    public static bool WinMenuisActive = false;

    public static HealthBarUpdate TakeDamageEvent;
    public static HealthBarUpdate TakeHealEvent;

    private void Awake()
    {
        TakeDamageEvent = new HealthBarUpdate();
        TakeHealEvent = new HealthBarUpdate();
    }

    // Use this for initialization
    void Start () {

        currentHealthBar.rectTransform.localScale = new Vector3(1, 1, 1);
        ratioText.text = "100%";

        TakeDamageEvent.AddListener(TakeDamage);
        TakeHealEvent.AddListener(TakeHeal);

        MenuisActive = true;

        MainMenu.SetActive(MenuisActive);
        OptionsMenu.SetActive(OptionsisActive);
        PauseMenu.SetActive(PauseMenuisActive);
        WinMenu.SetActive(WinMenuisActive);
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
        PopUpMenu.SetActive(PopUpMenuIsActive);

        // Audio
        SetMasterVolume();
    }



    ///HealthBar
    ///

    public void UpdateHealthBar()
    {
        float ratio = hitPoint / maxHitPoint;
        currentHealthBar.rectTransform.localScale = new Vector3(ratio, 1, 1);
        ratioText.text = (ratio * 100).ToString() + '%';
        
    }

    private void TakeDamage(float damage)
    {
        hitPoint -= damage;
        if(hitPoint < 0)
        {
            hitPoint = 0;
            Debug.Log("Dead!");
        }
        UpdateHealthBar();
    }

    private void TakeHeal(float heal)
    {
        hitPoint += heal;
        if (hitPoint > maxHitPoint)
        {
            hitPoint = maxHitPoint;
            Debug.Log("Heal!");
        }
        UpdateHealthBar();
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


        //With FadeScreen
        //LevelManager.instance.LoadByIndex(2);

        // With LoadingScreen
        StartCoroutine(LevelManager.instance.LoadAsynchonusly(2));

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



    /// <summary>
    /// Hier Eventuell noch eien Json oder csv. importer Implementerien und die PopUps automatisch erzeugen!!!
    /// </summary>
    public struct PopUp
    {
        public string name;
        public string headLine;
        public string text1;
        public string text2;
    }

}

public class HealthBarUpdate : UnityEvent<float>
{

}
