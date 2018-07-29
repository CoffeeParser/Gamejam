using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DrEvil.DataStructure;

namespace DrEvil.Mechanics
{
    /// <summary>
    /// This class handles all mechanics regarding diaogues
    /// </summary>
    public class DialogPlayer : MonoBehaviour
    {
        public GameObject TherapyScreenGameObject;
        public GameObject DialogField;
        public Text dialogFieldtext;

        public GameObject EvilScreenGameObject;


        public GameObject PatientScreenGameObject;
        public Image PatientSprite;
        public Text PatientBioInfoText;

        public Button NextDialogButton;

        private GameState _gameState;
        private int dialogIndex;

        private SoundController soundController;

        // Use this for initialization
        void Start()
        {
            soundController = GetComponent<SoundController>();
            _gameState = GameState.instance;
        }

        /// <summary>
        /// Play a given story from a level
        /// </summary>
        /// <param name="person"></param>
        /// <param name="newLevel"></param>
        public void PlayStory(Person person, bool newLevel = false)
        {
            NextDialogButton.onClick.RemoveAllListeners();
            // 0. activate therapyscreen
            TherapyScreenGameObject.SetActive(true);

            // 1. Initialize Persons visuals
            InitializePatientScreen(person);

            // 2. Reset dialogIndex to -1 (start from scratch)
            dialogIndex = -1;

            // 3. Setup Next Dialog Button handler
            // How many Actions are to do
            int evilActions = _gameState.CurrentPerson.EvilAction.Count;
            DialogField.SetActive(true);
            // How many Actions are solved
            int solvedActions = _gameState.CurrentPerson.SolvedActions.Count;

            if (newLevel)
            {
                //Debug.Log("Start level from new");
                NextDialogButton.onClick.AddListener(ShowTherapieDialog);
                ShowTherapieDialog();
            }
            else
            {
                //Debug.Log(evilActions + " " + solvedActions);
                // All Actions Are Solved
                if (evilActions <= 0) // Show all SolvedMessages
                {
                    //Debug.Log("ALL SOLVED");
                    NextDialogButton.onClick.AddListener(ShowSolvedDialog);
                    ShowSolvedDialog();
                }
                //else (solvedActions == 0) // nothing solved at all! just show TherapyStory
                //{
                //    Debug.Log("NONE SOLVED");
                //    NextDialogButton.onClick.AddListener(ShowTherapieDialog);
                //    ShowPartiallySolvedDialog();
                //}
                //// Show PartiallSolved Story
                else
                {
                    //Debug.Log("PART SOLVED");
                    NextDialogButton.onClick.AddListener(ShowPartiallySolvedDialog);
                    ShowPartiallySolvedDialog();
                }
            }
        }

        /// <summary>
        /// Set Person Description and Update Sprite
        /// </summary>
        /// <param name="person"></param>
        public void InitializePatientScreen(Person person)
        {
            Sprite charSprite = Resources.Load<Sprite>(person.SpritePathInResources);
            PatientSprite.sprite = charSprite;
            PatientBioInfoText.text = $"{person.Name}, {person.Age}, {person.Alias}";
        }

        /// <summary>
        /// Update the screen depending on which actor is talking
        /// </summary>
        /// <param name="actor"></param>
        private void UpdateActor(string actor)
        {
            EvilScreenGameObject.SetActive(actor.Equals("Dr"));
            PatientScreenGameObject.SetActive(actor.Equals("Patient"));
        }

        /// <summary>
        /// Finalize a dialogue
        /// </summary>
        private void EndDialog()
        {
            StopAllCoroutines();
            dialogIndex = -1;
            NextDialogButton.onClick.RemoveAllListeners();
            DialogField.SetActive(false);
            PatientScreenGameObject.SetActive(false);
            EvilScreenGameObject.SetActive(false);
            TherapyScreenGameObject.SetActive(false);
        }

        /// <summary>
        /// Handling the init Dialogue for the patients
        /// </summary>
        public void ShowTherapieDialog()
        {
            dialogIndex++;
            // Show TherapieStory again
            if (dialogIndex >= _gameState.CurrentPerson.TherapyStory.Count) // max
            {
                //hier ende, lade szene raum
                //Debug.Log("End");
                EndDialog();
                ScreenViewHandler.instance.EnterNightScene();
            }
            else
            {
                string actor = _gameState.CurrentPerson.TherapyStory[dialogIndex].Actor;
                UpdateActor(actor);

                StopAllCoroutines();
                StartCoroutine(DialogueWriter(_gameState.CurrentPerson.TherapyStory[dialogIndex].Message, actor));
                //dialogFieldtext.text = (_gameState.CurrentPerson.TherapyStory[dialogIndex].Message);
            }
        }

        /// <summary>
        /// The Dialogue Writer for adding text over time to duialogue box
        /// </summary>
        /// <param name="displayText"></param>
        /// <param name="actor"></param>
        /// <returns></returns>
        public IEnumerator DialogueWriter(string displayText, string actor)
        {
            soundController.voice.Stop();
            int subStringLength = 0;
            while (subStringLength < displayText.Length)
            {
                if (!soundController.voice.isPlaying)
                {
                    if (actor == "Dr")
                    {
                        soundController.PlayVoice(soundController.DrEvilTalk);
                    }
                    else
                    {
                        soundController.PlayVoice(soundController.PatientTalk);
                    }
                }
                dialogFieldtext.text = displayText.Substring(0, subStringLength);
                subStringLength++;
                yield return new WaitForSeconds(0.02f);
            }
            soundController.voice.Stop();
        }

        /// <summary>
        /// Handling dialogue mechanic for solved levels
        /// </summary>
        public void ShowSolvedDialog()
        {
            string message = null;
            while (message == null)
            {
                dialogIndex++;
                if (dialogIndex >= _gameState.CurrentPerson.ReviewStory.Count)
                {
                    EndDialog();
                    GameState.instance.ResetLevel(GameState.instance.CurrentPerson);
                    GameState.instance.AccomplishActualLevel();
                    ScreenViewHandler.instance.EnterMiniMapMenu();
                    break;
                }
                else
                {
                    ReviewStory reviewStory = _gameState.CurrentPerson.ReviewStory[dialogIndex];



                    if (reviewStory.DialogType.Equals("All") || reviewStory.DialogType.Equals("Solved"))
                    {
                        message = reviewStory.Message != null ? reviewStory.Message : reviewStory.SolvedMessage;
                        if (!string.IsNullOrEmpty(message))
                        {
                            UpdateActor(reviewStory.Actor);
                            StopAllCoroutines();
                            string actor = reviewStory.Actor;
                            StartCoroutine(DialogueWriter(message, actor));
                        }
                    }
                }
            }
        }

        /// <summary>
        /// handling the dialogue for partially solved levels
        /// </summary>
        public void ShowPartiallySolvedDialog()
        {
            string message = null;
            while (message == null)
            {
                dialogIndex++;
                if (dialogIndex >= _gameState.CurrentPerson.ReviewStory.Count)
                {
                    EndDialog();
                    GameState.instance.ResetLevel(GameState.instance.CurrentPerson);
                    ScreenViewHandler.instance.EnterMiniMapMenu();
                }
                else
                {
                    ReviewStory reviewStory = _gameState.CurrentPerson.ReviewStory[dialogIndex];
                    UpdateActor(reviewStory.Actor);
                    if (reviewStory.DialogType.Equals("Action") || reviewStory.DialogType.Equals("All"))
                    {
                        Debug.Log("PARTIALY");
                        message = reviewStory.Message != null ? reviewStory.Message : GetMessageDependingOnSolvedActionStatus(reviewStory);
                        if (!string.IsNullOrEmpty(message))
                        {
                            string actor = reviewStory.Actor;
                            StopAllCoroutines();
                            StartCoroutine(DialogueWriter(message, actor));
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Get proper dialogue message depending on its solved status
        /// </summary>
        /// <param name="story"></param>
        /// <returns></returns>
        private string GetMessageDependingOnSolvedActionStatus(ReviewStory story)
        {
            if (story.DialogType != "Action")
            {
                if (_gameState.CurrentPerson.EvilAction.Count > 0)
                {
                    return story.FailedMessage;
                }
                return story.SolvedMessage;
            }
            else
            {
                foreach (EvilAction evilAction in GameState.instance.CurrentPerson.EvilAction)
                {
                    if (evilAction.Identifier == story.Identifier)
                    {
                        return story.FailedMessage;
                    }
                }
                return story.SolvedMessage;
            }
        }

    }

}