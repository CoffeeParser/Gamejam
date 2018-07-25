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
    void Start ()
    {
        _gameState = GameState.instance;
    }

    public void PlayReviewStory(List<ReviewStory> reviewStories, Person person)
    {
        Debug.Log("PlayReviewStory started!");
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
        }else if (solvedActions == 0) // nothing solved at all! just show TherapyStory
        {
            NextDialogButton.onClick.AddListener(ShowTherapieDialog);
            ShowTherapieDialog();
        }
        // Show PartiallSolved Story
        else if (evilActions != 0 && solvedActions != 0)
        {
            NextDialogButton.onClick.AddListener(ShowPartiallySolvedDialog);
            ShowPartiallySolvedDialog();
        }
    }

    public void PlayTherapyStory(List<TherapyStory> therapyStories, Person person)
    {
        // 0. activate therapyscreen
        TherapyScreenGameObject.SetActive(true);

        InitializePatientScreen(person);

        // 2. Reset dialogIndex to -1 (start from scratch)
        dialogIndex = -1;

        DialogField.SetActive(true);
        NextDialogButton.onClick.AddListener(ShowTherapieDialog);
        ShowTherapieDialog();
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
            dialogFieldtext.text = (_gameState.CurrentPerson.TherapyStory[dialogIndex].Message);
        }
    }
    public void ShowSolvedDialog()
    {
        dialogIndex++;
        if (dialogIndex >= _gameState.CurrentPerson.ReviewStory.Count)
        {
            EndDialog();
            ScreenViewHandler.instance.EnterMiniMapMenu();
            //später MiniMap ZUrück
        }
        else
        {
            UpdateActor(_gameState.CurrentPerson.ReviewStory[dialogIndex].Actor);
            dialogFieldtext.text = _gameState.CurrentPerson.ReviewStory[dialogIndex].DialogType.Equals("Story") ? (_gameState.CurrentPerson.ReviewStory[dialogIndex].Message) : (_gameState.CurrentPerson.ReviewStory[dialogIndex].SolvedMessage);
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

}
