using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace MobileSensors
{
    public class MobileInput : MonoBehaviour
    {

        public static MobileInput inst;

        public TouchInput touch;
        public Pinch pinch;
        public Mic mic;
        public Accelerate accel;
        public Gyro gyro;
        public Steps steps;
        public Compass compass;
        public MobileCamera camMain;
        public MobileCamera camFront;
        public Location location;

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

        // Use this for initialization
        void Awake()
        {
            if (!inst)
                inst = this;

            touch = GetComponentInChildren<TouchInput>();
            pinch = GetComponentInChildren<Pinch>();
            mic = GetComponentInChildren<Mic>();
            accel = GetComponentInChildren<Accelerate>();
            gyro = GetComponentInChildren<Gyro>();
            steps = GetComponentInChildren<Steps>();
            compass = GetComponentInChildren<Compass>();
            camMain = GetComponentsInChildren<MobileCamera>()[0];
            camFront = GetComponentsInChildren<MobileCamera>()[1];
            location = GetComponentInChildren<Location>();

        }

        public Transform CastRayHitFromCam(Vector2 pos, int layerMask)
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(pos);

            if (Physics.Raycast(ray, out hit, layerMask))
                return hit.transform;
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

public class EventTouch : UnityEvent<Touch>
{
}

public class EventLocation : UnityEvent<LocationInfo>
{
}

[System.Serializable]
public struct Threshold
{
    public string name;
    public float amount;
}