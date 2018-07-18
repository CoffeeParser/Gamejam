using UnityEngine;
using MobileSensors;

public class Compass : MonoBehaviour {

    public float magneticHeading;
    public float trueHeading;
    public Vector3 rawVector;

    private void Start()
    {
        MobileInput.inst.location.OnLocationUpdate.AddListener(UpdateCompass);
    }

    private void UpdateCompass(LocationInfo info)
    {
        magneticHeading = Input.compass.magneticHeading;
        trueHeading = Input.compass.trueHeading;
        rawVector = Input.compass.rawVector;
    }

    // Update is called once per frame
    void Update () {

	}
}
