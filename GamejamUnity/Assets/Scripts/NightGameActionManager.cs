using System.Linq;
using UnityEngine;
using DrEvil.Mechanics;

namespace DrEvil.DataStructure
{
    /// <summary>
    /// Loading the gamedata from json and generating tasks for a level to solve from the player
    /// </summary>
    public class NightGameActionManager : MonoBehaviour
    {

        // Use this for initialization
        void Start()
        {
            InitializeObjectTriggersOnRoomObjects();
            InitializeGlobalObjectTriggersOnController();
        }

        /// <summary>
        /// Initialize tasks from the json data
        /// </summary>
        void InitializeObjectTriggersOnRoomObjects()
        {
            var evilActions = GameState.instance.CurrentPerson.EvilAction;
            GameObject[] interactableGameObjects = GameObject.FindGameObjectsWithTag("EvilActionObject");
            if (interactableGameObjects != null)
            {
                foreach (GameObject interactableGameObject in interactableGameObjects)
                {
                    // every interactableGameObject has an ObjectTrigger script attached to it
                    ObjectTrigger objectTrigger = interactableGameObject.GetComponent<ObjectTrigger>();
                    if (objectTrigger != null)
                    {
                        EvilAction matchingIdentifierEvilAction =
                            evilActions.FirstOrDefault(b => b.Identifier.Equals(objectTrigger.EvilActionIdentifier));
                        if (matchingIdentifierEvilAction != null)
                        {
                            objectTrigger.actionTrigger = matchingIdentifierEvilAction;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Initialize tasks from the json data which are not physically in the scene
        /// </summary>
        void InitializeGlobalObjectTriggersOnController()
        {
            var evilActions = GameState.instance.CurrentPerson.EvilAction;
            foreach (EvilAction evilAction in evilActions)
            {
                // this evilAtions are global!
                if (evilAction.ActionType.Equals("voice"))
                {
                    TerrorLevelController.instance.AddVoiceAction(evilAction);
                }
                if (evilAction.ActionType.Equals("scratch"))
                {
                    TerrorLevelController.instance.AddScratchAction(evilAction);
                }
            }
        }
    }
}