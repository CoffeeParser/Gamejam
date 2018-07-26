using UnityEngine;
using UnityEngine.UI;

public class MapViewController : MonoBehaviour
{

    private GameState _gamestate;
    private Button[] _allButtons;

    public Button Level1Button;
    public Button Level2Button;
    public Button Level3Button;
    public Button Level4Button;

    public Sprite Level_1Sprite_InActive, Level_1Sprite_Active;
    public Sprite Level_2Sprite_InActive, Level_2Sprite_Active;
    public Sprite Level_3Sprite_InActive, Level_3Sprite_Active;
    public Sprite Level_4Sprite_InActive, Level_4Sprite_Active;
    private Sprite[] _activeLevelSprites;
    private Sprite[] _inactiveLevelSprites;

    private bool _levelButtonsInitialized = false;

    private void Start()
    {
        var go = GameObject.FindGameObjectWithTag("GlobalLifeTime");
        _gamestate = go.GetComponent<GameState>();

        EnterMiniMap();
    }

    void OnEnable()
    {
        if (_levelButtonsInitialized)
        {
            EnterMiniMap();
        }
    }

    private void InitializeLevelButtons()
    {
        if (!_levelButtonsInitialized)
        {
            _allButtons = new[]
                {Level1Button, Level2Button, Level3Button, Level4Button};
            _activeLevelSprites = new[] { Level_1Sprite_Active, Level_2Sprite_Active , Level_3Sprite_Active , Level_4Sprite_Active };
            _inactiveLevelSprites = new[] { Level_1Sprite_InActive, Level_2Sprite_InActive, Level_3Sprite_InActive, Level_4Sprite_InActive };

            if (_allButtons.Length != _gamestate.CurrentWorld.Persons.Count) // zur sicherheit
            {
                Debug.LogError("Minimap Button length is not equal to person length in CurrentWorld!!!");
            }

            for (int i = 0; i < _gamestate.CurrentWorld.Persons.Count; i++)
            {
                var currentIndex = i;
                _allButtons[i].onClick.AddListener(() => _gamestate.SelectAnLevel(_gamestate.CurrentWorld.Persons[currentIndex]));
            }
            _levelButtonsInitialized = true;
        }
    }

    private void RefreshLevelButtonStates()
    {
        for (int i = 0; i < _gamestate.CurrentWorld.Persons.Count; i++)
        {
            if (_gamestate.CurrentWorld.Persons[i].Finished) // first state: the level is already finished
            {
                _allButtons[i].interactable = false;
                _allButtons[i].gameObject.GetComponent<Image>().sprite = _inactiveLevelSprites[i];
            }
            else if (_gamestate.CurrentWorld.Persons[i].Unlocked) // second state: this is the next possible level!
            {
                _allButtons[i].interactable = true;
                _allButtons[i].gameObject.GetComponent<Image>().sprite = _activeLevelSprites[i];
            }
            else // third state: not finished and locked = can't visit yet
            {
                _allButtons[i].interactable = false;
                _allButtons[i].gameObject.GetComponent<Image>().sprite = _inactiveLevelSprites[i];
            }
        }
    }

    /// <summary>
    /// Call this whenever the minimap is entered to refresh level button states
    /// </summary>
    public void EnterMiniMap()
    {
        InitializeLevelButtons();
        RefreshLevelButtonStates();
    }
}