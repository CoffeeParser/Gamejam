using System.Collections;
using System.Collections.Generic;
using MobileSensors;
using UnityEngine;

public class InteractableObjectHghlighter : MonoBehaviour
{
    private GameObject currentlyHighlightedGameObject;
    private int _interactableLayer = 8;
    public Camera GyroCam;
    
	// Update is called once per frame
	void FixedUpdate () {
	    Vector2 screenCenter = GyroCam.pixelRect.center;
	    GameObject hitGameObject = MobileInput.instance.CastRayHitFromCam(screenCenter, GyroCam, _interactableLayer);
	    if (hitGameObject != null && hitGameObject != currentlyHighlightedGameObject)
	    {
	        if (currentlyHighlightedGameObject != null)
	        {
	            currentlyHighlightedGameObject.GetComponent<HighightableObject>().DisableHighlight(); // disable old highlighted object
	        }
            Debug.Log("hitting interactable obj!!!");
	        currentlyHighlightedGameObject = hitGameObject;
            currentlyHighlightedGameObject.GetComponent<HighightableObject>().EnableHighlight();
        }
	    else if (hitGameObject == null && currentlyHighlightedGameObject != null)
	    {
	        currentlyHighlightedGameObject.GetComponent<HighightableObject>().DisableHighlight();
	        currentlyHighlightedGameObject = null;
	    }
	}

    
}
