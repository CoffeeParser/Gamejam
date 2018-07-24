using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour {

    public static LevelManager instance;

    public GameObject loadingScreen;
    public Slider slider;


    public UnityEvent WonTheGame;
    public UnityEvent LooseTheGame;
    public UnityEvent OpenPopUp;
    public UnityEvent RestartGame;

    private static int IndexOfLastLoadedScene = -1;
    private static string StringOfLastLoadedScene = "";


    //public UnityEvent levelIsOver;
    //public UnityEvent levelReload;

    [Header("SceneFadeAnimation")]
    public Texture2D fadeOutTexture;    // ScreenOverlay
    public float fadeSpeed = 0.8f;      // FadeOutTime 

    private int drawDeath = -1000;      // TextureLayerOrder -1000 it renders on top 
    private float alpha = 1.0f;         // Texture AlphaValue
    private int fadedir = -1;           // the direction to fade in = -1 out = 1

    private void Awake()
    {
        instance = this;
    }

    //FADEEFFEKT
    private void OnGUI()
    {
        // fade out/in the alpha value using a direction, a speed and time.deltatime 
        alpha += fadedir * fadeSpeed * Time.deltaTime;
        // force clamp the number bewteen 0, 1 because of GUIColor
        alpha = Mathf.Clamp01(alpha);

        // set Color of GUI
        GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, alpha);            // Set alpha value
        GUI.depth = drawDeath;                                                          // render Texture
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), fadeOutTexture);   // Let fir Texture to Screen

    }

    public float BeginFade(int direction)
    {
        //Debug.Log("Begin Fade");
        fadedir = direction;
        return (fadeSpeed); // return for applicationen LoadLevel
    }

    private void Start()
    {
        //Debug.Log("Start");
        BeginFade(-1);
    }

    private void Update()
    {
        WonTheGame.AddListener(IfGameHasWon);
        RestartGame.AddListener(IfLevelHasToReload);
        OpenPopUp.AddListener(IfPopUpHasToOpen);
    }

    public void FadeIn()
    {
        StartCoroutine(IEFadeIn());
    }
    public IEnumerator IEFadeIn()
    {
        //Debug.Log("Fade IN Change");
        float fadeTime = BeginFade(-1);
        yield return new WaitForSeconds(fadeTime);
    }

    public void FadeOut(int sceneIndex)
    {
        StartCoroutine(IEFadeOut(sceneIndex));
    }
    public IEnumerator IEFadeOut(int sceneIndex = -1)
    {
        //Debug.Log("Fade OUT Change");
        float fadeTime = BeginFade(1);
        yield return new WaitForSeconds(fadeTime);
        if (sceneIndex >= 0)
        {
            SceneManager.LoadScene(sceneIndex, LoadSceneMode.Additive);
        }
        //StartCoroutine(FadeIn());
    }


    public int GetIndexOfCurrentScene()
    {
        int index = SceneManager.GetActiveScene().buildIndex;

        Debug.Log("Buildindex of CurrentScene is: " + index);
        return index;
    }

    public int GetIndexOfLastLoadedScene()
    {
        Debug.Log("Buildindex of CurrentScene is: " + IndexOfLastLoadedScene);
        return IndexOfLastLoadedScene;
    }

    public void LoadByIndex(int sceneIndex)
    {
        FadeOut(sceneIndex);
        IfLevelIsOver();
        Invoke("FadeIn", 1);
        IndexOfLastLoadedScene = sceneIndex;
    }

    public void LoadByString(string sceneString)
    {
        IfLevelIsOver();
        SceneManager.LoadScene(sceneString, LoadSceneMode.Additive);
        StringOfLastLoadedScene = sceneString;
    }

    /// <summary>
    /// Load Scene with Loading Bar
    /// </summary>

    public void LoadLevel (int sceneIndex)
    {
        SceneManager.LoadSceneAsync(sceneIndex, LoadSceneMode.Additive);
        
    }
    public IEnumerator LoadAsynchonusly(int sceneIndex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);

        loadingScreen.SetActive(true);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / .9f);
            slider.value = progress;
            //Debug.Log(slider.value);
            yield return null;
        }
        loadingScreen.SetActive(false);
    }
    public IEnumerator LoadAsynchonusly(string sceneString)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneString);

        loadingScreen.SetActive(true);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / .9f);
            slider.value = progress;
            //Debug.Log(slider.value);
            yield return null;
        }
        loadingScreen.SetActive(false);
    }

    public void IfLevelIsOver()
    {
        if (IndexOfLastLoadedScene >= 0)
        {
            try
            {
                //Debug.Log("Current SceneNumber: " + IndexOfLastLoadedScene);
                SceneManager.UnloadSceneAsync(IndexOfLastLoadedScene);
            }
            catch (Exception e)
            {
                Debug.Log("Scene was not Found");
            }
        }
        else if (StringOfLastLoadedScene != "")
        {
            try
            {
                SceneManager.UnloadSceneAsync(StringOfLastLoadedScene);
            }
            catch (Exception e)
            {
                Debug.Log("Scene was not Found");
            }
        }
    }

    public void IfLevelHasToReload()
    {
        LoadAsynchonusly(GetIndexOfCurrentScene());
        //LoadByIndex(GetIndexOfCurrentScene());
    }

    public void IfGameHasWon()
    {
        Time.timeScale = 0f;
        WonTheGame.RemoveListener(IfGameHasWon);
        UIController.WinMenuisActive = true;
    }

    public void IfPopUpHasToOpen(string PopUpName)
    {
        Time.timeScale = 0f;
        UIController.PopUpMenuIsActive = true;

    }
    public void IfPopUpHasToOpen()
    {
        Time.timeScale = 0f;
        UIController.PopUpMenuIsActive = true;
    }



    /// CallBack bei Scene Loaded ? / 




}
