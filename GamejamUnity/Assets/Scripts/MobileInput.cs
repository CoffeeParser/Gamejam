using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace MobileSensors
{
    public class MobileInput : MonoBehaviour
    {
        public static MobileInput instance;

        public TouchInput touch;
        //public Pinch pinch;
        public Mic mic;
        public Accelerate accel;
        public Gyro gyro;
        //public Compass compass;
        //public MobileCamera camMain;
        //public MobileCamera camFront;
        //public Location location;

        public UnityEvent OnUnityRemoteStarted;

        private IEnumerator Start()
        {
#if UNITY_EDITOR
            //Wait until Unity connects to the Unity Remote, while not connected, yield return null
            while (!UnityEditor.EditorApplication.isRemoteConnected)
            {
                yield return new WaitForSeconds(1);
            }
#endif
            yield return null;
            OnUnityRemoteStarted.Invoke();
        }

        void Awake()
        {
            if (!instance)
                instance = this;

            touch = GetComponentInChildren<TouchInput>();
            mic = GetComponentInChildren<Mic>();
            accel = GetComponentInChildren<Accelerate>();
            gyro = GetComponentInChildren<Gyro>();

        }

        IEnumerator Debugger()
        {
            while (true)
            {
                Debug.Log("Mic: " + mic.micLevelinDb);
            }
        }

        public GameObject CastRayHitFromCam(Vector2 pos, Camera cam, int layerMask = 8)
        {
            RaycastHit hit;
            Ray ray = cam.ScreenPointToRay(pos);

            if (Physics.Raycast(ray, out hit, 100, 1 << layerMask))
                return hit.transform.gameObject;

            return null;
        }
    }
}

public class EventFloat : UnityEvent<float>
{
}

public class EventInt : UnityEvent<int>
{
}

public class EventString : UnityEvent<string>
{
}

public class EventThresholdLevel : UnityEvent<ThresholdLevel>
{
}

public class EventTouch : UnityEvent<Touch>
{
}

public class EventMultiTouch : UnityEvent<Touch[]>
{
}

public class EventScreenPosition : UnityEvent<Vector2>
{
}

public class EventLocation : UnityEvent<LocationInfo>
{
}

[System.Serializable]
public struct Threshold
{
    public ThresholdLevel level;
    public float amount;
}