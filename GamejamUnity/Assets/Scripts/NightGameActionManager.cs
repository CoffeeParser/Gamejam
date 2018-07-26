using System.Linq;
using UnityEngine;

public class NightGameActionManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
		InitializeObjectTriggersOnRoomObjects();
	}

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
}
