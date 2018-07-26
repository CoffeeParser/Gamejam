using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

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

    // Use this for initialization
    void Start()
    {
        _gameState = GameState.instance;
    }

    public void PlayStory(Person person)
    {
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
        // All Actions Are Solved
        if (evilActions == 0) // Show all SolvedMessages
        {
            NextDialogButton.onClick.AddListener(ShowSolvedDialog);
            ShowSolvedDialog();
        }
        else if (solvedActions == 0) // nothing solved at all! just show TherapyStory
        {
            NextDialogButton.onClick.AddListener(ShowTherapieDialog);
            ShowPartiallySolvedDialog();
        }
        // Show PartiallSolved Story
        else if (evilActions != 0 && solvedActions != 0)
        {
            NextDialogButton.onClick.AddListener(ShowPartiallySolvedDialog);
            ShowPartiallySolvedDialog();
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

    // Update Actor Screen
    private void UpdateActor(string actor)
    {
        EvilScreenGameObject.SetActive(actor.Equals("Dr"));
        PatientScreenGameObject.SetActive(actor.Equals("Patient"));
    }

    private void EndDialog()
    {
        dialogIndex = -1;
        NextDialogButton.onClick.RemoveAllListeners();
        DialogField.SetActive(false);
        PatientScreenGameObject.SetActive(false);
        EvilScreenGameObject.SetActive(false);
        TherapyScreenGameObject.SetActive(false);
    }

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
            UpdateActor(_gameState.CurrentPerson.TherapyStory[dialogIndex].Actor);

            StopAllCoroutines();
            StartCoroutine(DialogueWriter(_gameState.CurrentPerson.TherapyStory[dialogIndex].Message));
            //dialogFieldtext.text = (_gameState.CurrentPerson.TherapyStory[dialogIndex].Message);
        }
    }

    public IEnumerator DialogueWriter(string displayText)
    {

        int subStringLength = 0;
        while (subStringLength < displayText.Length)
        {
            dialogFieldtext.text = displayText.Substring(0, subStringLength);
            subStringLength++;
            yield return new WaitForSeconds(0.02f);
        }
    }

    public void ShowSolvedDialog()
    {
        string message = null;
        while (message == null)
        {
            dialogIndex++;
            if (dialogIndex >= _gameState.CurrentPerson.ReviewStory.Count)
            {
                EndDialog();
                ScreenViewHandler.instance.EnterMiniMapMenu();
            }
            else
            {
                ReviewStory reviewStory = _gameState.CurrentPerson.ReviewStory[dialogIndex];
                message = reviewStory.DialogType.Equals("Story") ? reviewStory.Message : reviewStory.SolvedMessage;
                UpdateActor(reviewStory.Actor);
                StartCoroutine(DialogueWriter(message));
            }
        }

        
    }

    public void ShowPartiallySolvedDialog()
    {
        dialogIndex++;
        if (dialogIndex >= _gameState.CurrentPerson.ReviewStory.Count)
        {
            EndDialog();
            ScreenViewHandler.instance.EnterNightScene();
        }
        else
        {
            Debug.Log("showing partial solved dialog");
            ReviewStory currentReviewStory = _gameState.CurrentPerson.ReviewStory[dialogIndex];
            UpdateActor(currentReviewStory.Actor);
            if (currentReviewStory.DialogType.Equals("Story"))
            {
                dialogFieldtext.text = currentReviewStory.Message;
            }
            else if (currentReviewStory.DialogType.Equals("ActionStatus")) // DialogType = ActionStatus
            {
                if (_gameState.CurrentPerson.SolvedActions.FirstOrDefault(b =>
                {
                    return b.ActionType.Equals(currentReviewStory.ActionType) && b.Identifier.Equals(currentReviewStory.Identifier);
                }) != null)
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

}
