using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Pinch : MonoBehaviour
{

    public float pinchSpeed = 1f;
    public float pinchRotSpeed = 1f;

    //public float magnRotThreshold;
    public float maxDeltaRot;

    public float deltaMagn;
    public float deltaRot;

    private Touch touchFirst;
    private Touch touchSecond;

    private Vector2 touchZeroPrevPos;
    private Vector2 touchOnePrevPos;

    void Update()
    {
        if (Input.touchCount == 2)
        {
            //Get actual touches
            touchFirst = Input.GetTouch(0);
            touchSecond = Input.GetTouch(1);

            // Get previous position of touches
            touchZeroPrevPos = touchFirst.position - touchFirst.deltaPosition;
            touchOnePrevPos = touchSecond.position - touchSecond.deltaPosition;

            //Calculate and update delta magnitude
            deltaMagn = CalcDeltaMagnitude() * pinchSpeed;
            //Calculate and update delta Rotation
            deltaRot = CalcDeltaRotation() * pinchRotSpeed;
        }
        else
        {
            deltaMagn = 0;
            deltaRot = 0;
        }
    }

    private float CalcDeltaMagnitude()
    {
        float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
        float touchDeltaMag = (touchFirst.position - touchSecond.position).magnitude;

        // difference between previous touch and touch 
        return prevTouchDeltaMag - touchDeltaMag;
    }

    private float CalcDeltaRotation()
    {
        //Get pinch Dir and corresponding angle of previous Frame
        Vector2 pinchDirPrev = touchOnePrevPos - touchZeroPrevPos;
        float oldAngle = Mathf.Atan2(pinchDirPrev.x, pinchDirPrev.y);

        //Get pinch Dir and corresponding angle of actual Frame
        Vector2 pinchDir = touchSecond.position - touchFirst.position;
        float newAngle = Mathf.Atan2(pinchDir.x, pinchDir.y);

        //Return delta angle of previous and actual frame
        return Mathf.Clamp(Mathf.DeltaAngle(oldAngle, newAngle), -maxDeltaRot, maxDeltaRot);
    }
}
