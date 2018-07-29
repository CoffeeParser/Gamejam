using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DrEvil.DataStructure;
using DrEvil.Visuals;

namespace DrEvil.Mechanics
{
    /// <summary>
    /// This class handles everything regarding scenes and views and screens.
    /// </summary>
    public class ScreenViewHandler : MonoBehaviour
    {
        public static ScreenViewHandler instance;

        private SoundController soundController;
        public DialogPlayer DialogPlayer;

        public GameObject StartScreen;
        public Button EnterMapMenu;

        public GameObject MapViewGameObject;
        public GameObject DayScreenGameObject;
        public UI_SpriteAnimator AnimateEnterTherapyAfterMap;

        public GameObject NightScreen;

        public GameObject LevelAccomplishedScreen;
        public Button LeaveFinishVBtn;

        public GameObject LevelFailedScreen;
        public Button LeaveUnFinishVBtn;

        private GameState _gameState;
        public Button SkipNightScreenBtn;

        private string LoadLevelString = "ZimmerTerror";

        private bool acc;

        /// <summary>
        /// Init Screens...
        /// </summary>
        void Awake()
        {
            soundController = GetComponent<SoundController>();

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
                OnSceneStateChanged();
            });

            //Add Listeners to Skip Evil Dialog
            SkipNightScreenBtn.onClick.AddListener(SkipNightScreen);

            // Add Listener to AchievementScreen
            //achievementBtn.onClick.AddListener(SkipNightScreen);

        }

        private void Start()
        {

            OnSceneStateChanged();
        }

        /// <summary>
        /// Event on Scene changed
        /// </summary>
        private void OnSceneStateChanged()
        {
            soundController.StopAllSources();
            if (StartScreen.activeSelf)
            {
                Debug.Log("StartScreen");
                soundController.PlayMusicLoop(soundController.StartScreenMusik);
                soundController.PlayVoiceLoop(soundController.DrEvilLaugh);
                soundController.audioListener.enabled = true;
            }
            else if (MapViewGameObject.activeSelf)
            {
                Debug.Log("Map");

                soundController.PlayMusicLoop(soundController.StartScreenMusik);
                soundController.audioListener.enabled = true;
            }
            else if (DayScreenGameObject.activeSelf)
            {
                Debug.Log("Day");
                soundController.PlayMusicLoop(soundController.DialogMusik);
                soundController.audioListener.enabled = true;
            }
            else if (DialogPlayer.TherapyScreenGameObject.activeSelf)
            {
                Debug.Log("Therapy");
                soundController.PlayMusicLoop(soundController.DialogMusik);
                soundController.audioListener.enabled = true;
            }
            else if (NightScreen.activeSelf)
            {
                Debug.Log("Night");
                soundController.StopAllSources();
                soundController.PlayMusicLoop(soundController.NightRoomMusik);
                soundController.audioListener.enabled = false;
                soundController.voice.Stop();
            }
            else if (LevelAccomplishedScreen.activeSelf)
            {
                Debug.Log("LevelAccomplishedScreen");
                soundController.PlayMusicLoop(soundController.NightRoomMusik);
                soundController.audioListener.enabled = true;
            }

        }

        /// <summary>
        /// Event on Level Started
        /// </summary>
        void LevelStarted()
        {

            MapViewGameObject.SetActive(false);
            DayScreenGameObject.SetActive(true);
            OnSceneStateChanged();
            AnimateEnterTherapyAfterMap.PlayOneBurstWithCallback(() =>
            {
                DayScreenGameObject.SetActive(false);
                DialogPlayer.PlayStory(_gameState.CurrentPerson, true);
                OnSceneStateChanged();

            });
        }

        /// <summary>
        /// Finalize night ended Level
        /// </summary>
        void NightEnded()
        {
            LevelAccomplishedScreen.SetActive(false);
            LevelFailedScreen.SetActive(false);
            DialogPlayer.PlayStory(_gameState.CurrentPerson);
            OnSceneStateChanged();
        }

        //public void SkipAchievementScreen()
        //{
        //    achievementScreen.SetActive(false);
        //}

        /// <summary>
        /// Method for skipping night loading screen
        /// </summary>
        public void SkipNightScreen()
        {
            NightScreen.SetActive(false);
            OnSceneStateChanged();
            TerrorLevelController.instance.Init();
        }

        /// <summary>
        /// Load a level asyncronously
        /// </summary>
        /// <param name="sceneString"></param>
        /// <returns></returns>
        public IEnumerator LoadAsynchonusly(string sceneString)
        {
            AsyncOperation operation = SceneManager.LoadSceneAsync(sceneString, LoadSceneMode.Additive);

            NightScreen.SetActive(true);

            while (!operation.isDone)
            {
                SkipNightScreenBtn.enabled = false;
                yield return null;
            }
            SkipNightScreenBtn.enabled = true;
            OnSceneStateChanged();
            //Debug.Log("LevelLoad is finish");
            //NightScreen.SetActive(false);
            //LevelAccomplishTest.instance.onLevelIsOver.AddListener(isLevelAccomplished);
        }

        /// <summary>
        /// Finalize a level depending on solved status
        /// </summary>
        /// <param name="isAccomplished"></param>
        public void FinalizeLevel(bool isAccomplished)
        {
            // UnLoadlevel after ever Session
            SceneManager.UnloadSceneAsync(LoadLevelString);

            // Obsolet maybe for alternativ loadScreen
            if (isAccomplished)
            {
                // Finish leave Screen, only skipable when Level is Unload      
                LevelAccomplishedScreen.SetActive(true);
                LeaveFinishVBtn.onClick.AddListener(NightEnded);
                OnSceneStateChanged();
                //GameState.instance.AccomplishActualLevel();
            }
            else
            {
                // Unfinsih LeaveScreen Only Skipable wenn Level is unloaded 
                LevelFailedScreen.SetActive(true);
                LeaveUnFinishVBtn.onClick.AddListener(NightEnded);
                OnSceneStateChanged();
            }
        }

        /// <summary>
        /// Enter the night screen before accessing terror scene
        /// </summary>
        public void EnterNightScene()
        {
            NightScreen.SetActive(true);
            StartCoroutine(LoadAsynchonusly(LoadLevelString));
            OnSceneStateChanged();
        }

        /// <summary>
        /// Show the Minimap Level overview
        /// </summary>
        public void EnterMiniMapMenu()
        {
            //achievementScreen.SetActive(true);
            MapViewGameObject.SetActive(true);
            OnSceneStateChanged();
            //MapViewGameObject.GetComponent<MapViewController>().EnterMiniMap();
        }

    }
}