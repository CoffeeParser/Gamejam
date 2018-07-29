using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DrEvil.DataStructure;
namespace DrEvil.Testing
{
    /// <summary>
    /// Testing script to solve some interactions
    /// </summary>
    public class SimulateSolvedActions : MonoBehaviour
    {

        public bool solvedActions_1;
        public bool solvedActions_2;
        public bool solvedActions_3;
        public bool solvedActions_4;

        private GameState _gameState;

        private void Awake()
        {
            _gameState = GameObject.FindGameObjectWithTag("GlobalLifeTime").GetComponent<GameState>();
        }

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (solvedActions_1)
            {
                // Sound
                // Set First Actions True
                EvilAction huehue = _gameState.CurrentPerson.EvilAction[0];
                _gameState.CurrentPerson.SolvedActions.Add(huehue);
                _gameState.CurrentPerson.EvilAction.Remove(huehue);
                solvedActions_1 = false;
            }
            if (solvedActions_2)
            {
                // Grap
                // Set First Actions True
                EvilAction huehue = _gameState.CurrentPerson.EvilAction[1];
                _gameState.CurrentPerson.SolvedActions.Add(huehue);
                _gameState.CurrentPerson.EvilAction.Remove(huehue);
                solvedActions_2 = false;
            }
            if (solvedActions_3)
            {
                // Trigger
                // Set First Actions True
                EvilAction huehue = _gameState.CurrentPerson.EvilAction[2];
                _gameState.CurrentPerson.SolvedActions.Add(huehue);
                _gameState.CurrentPerson.EvilAction.Remove(huehue);
                solvedActions_3 = false;
            }
        }
    }
}