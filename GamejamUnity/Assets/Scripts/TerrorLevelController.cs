using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MobileSensors;

public class TerrorLevelController : MonoBehaviour
{
    public static TerrorLevelController instance;

    public int maxFailAttempts = 5;
    public float failAttemptWaitTime = 1.5f;
    private float attemptTime;

    private TouchInput touchInput;
    private Mic micInput;
    private Accelerate accel;
    private Camera gyroCam;
    private List<ObjectTrigger> voiceActionObjs;
    private List<ObjectTrigger> scratchActions;

    private bool hasUserScratched;
    public bool hasUserSwiped;

    public AudioSource levelAudioSource;
    public AudioSource VoiceAudioSource;
    public AudioClip VoiceAudioClip;
    public List<AudioClip> FailedClips;
    public int failedClipIndex = -1;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        voiceActionObjs = new List<ObjectTrigger>();
        scratchActions = new List<ObjectTrigger>();
    }

    private void Start()
    {
        touchInput = MobileInput.instance.touch;
        micInput = MobileInput.instance.mic;
        accel = MobileInput.instance.accel;

        touchInput.OnTapPositionUp.AddListener(OnUserTouch);
        accel.OnShake.AddListener(OnUserPush);
        micInput.OnThresholdExceeded.AddListener(OnUserVoiceThresholdExceeded);
        touchInput.OnMove.AddListener(OnUserSwipe);
        touchInput.OnMultiMove.AddListener(OnUserScratch);
        touchInput.OnMultiMoveUp.AddListener(OnUserScratchEnded);

        attemptTime = failAttemptWaitTime;
        gyroCam = FindObjectOfType<GyroCamera>().GetComponent<Camera>();

    }

    private void Update()
    {
        attemptTime -= Time.deltaTime;
        attemptTime = Mathf.Max(0, attemptTime);
    }

    public void AddScratchAction(EvilAction scratchEvilAction)
    {
        GameObject tempGo = new GameObject();
        ObjectTrigger objTrigger = tempGo.AddComponent<ObjectTrigger>();
        objTrigger.actionTrigger = scratchEvilAction;
        scratchActions.Add(objTrigger);
    }

    public void AddVoiceAction(EvilAction voiceEvilAction)
    {
        GameObject tempGo = new GameObject();
        ObjectTrigger objTrigger = tempGo.AddComponent<ObjectTrigger>();
        objTrigger.actionTrigger = voiceEvilAction;
        objTrigger.holdingTimeThreshold = .1f;
        objTrigger.isSolved = false;
        voiceActionObjs.Add(objTrigger);
    }

    void OnUserScratch(Touch[] touch)
    {
        PlayAudio(AssetManager.instance.scratch, 0.5f);
        hasUserScratched = true;
    }

    void OnUserScratchEnded(Touch[] touch)
    {
        if (hasUserScratched)
        {
            foreach (ObjectTrigger scratchObj in scratchActions)
            {
                TryTriggerInteraction(scratchObj, TriggerTypes.scratch);
                scratchActions.Remove(scratchObj);
                hasUserScratched = false;
                break;
            }
        }

        levelAudioSource.Stop();
    }

    void OnUserVoiceThresholdExceeded(ThresholdLevel voiceLevel)
    {
        foreach (ObjectTrigger voiceObjTrigger in voiceActionObjs)
        {
            if (voiceObjTrigger.actionTrigger.details == voiceLevel.ToString())
            {
                voiceObjTrigger.holdingTime += Time.deltaTime;
                if (voiceObjTrigger.holdingTime >= voiceObjTrigger.holdingTimeThreshold && !voiceObjTrigger.isSolved)
                {
                    Debug.Log("Voice input action solved, nice");
                    voiceObjTrigger.isSolved = true;
                    StartCoroutine(voiceObjTrigger.TriggerAction(CompleteAction));
                    voiceObjTrigger.holdingTime = 0.0f;
                    VoiceAudioSource.PlayOneShot(VoiceAudioClip);
                }
            }
            else
            {
                if (!voiceObjTrigger.isSolved)
                {
                    voiceObjTrigger.holdingTime = 0;
                    TriggerFailAttempt();
                }
                
            }
        }
    }

    void OnUserSwipe(Touch touch)
    {
        GameObject hitObj = MobileInput.instance.CastRayHitFromCam(touch.position, gyroCam);
        ObjectTrigger objTrigger = TryGetObjectTrigger(hitObj);
        if (objTrigger != null)
        {
            hasUserSwiped = true;
            PlayAudio(AssetManager.instance.swipe);
            Rigidbody hitRig = hitObj.GetComponent<Rigidbody>();

            if (hitRig != null)
            {
                Vector3 force = Vector3.ClampMagnitude((gyroCam.transform.up * touch.deltaPosition.y + gyroCam.transform.right * touch.deltaPosition.x), 3);
                hitRig.AddForce(force, ForceMode.Impulse);
            }
            TryTriggerInteraction(objTrigger, TriggerTypes.swipe);
        }
    }

    void OnUserPush(ThresholdLevel shakeThreshold)
    {
        Vector2 shakeLoc = gyroCam.pixelRect.center;
        GameObject hitObj = MobileInput.instance.CastRayHitFromCam(shakeLoc, gyroCam);
        ObjectTrigger objTrigger = TryGetObjectTrigger(hitObj);
        if (objTrigger != null)
        {
            PlayAudio(AssetManager.instance.push);

            TryTriggerInteraction(objTrigger, TriggerTypes.push);
        }
    }

    void OnUserTouch(Vector2 tapPostiion)
    {
        GameObject hitObj = MobileInput.instance.CastRayHitFromCam(tapPostiion, gyroCam);
        levelAudioSource.Stop();
        ObjectTrigger objTrigger = TryGetObjectTrigger(hitObj);
        if (objTrigger != null && !hasUserSwiped)
        {
            PlayAudio(AssetManager.instance.touch);

            TryTriggerInteraction(objTrigger, TriggerTypes.touch);
        }
        hasUserSwiped = false;
    }

    void PlayAudio(AudioClip clip, float timeStamp = 0)
    {
        if (!levelAudioSource.isPlaying)
        {
            levelAudioSource.time = timeStamp;
            levelAudioSource.clip = clip;
            levelAudioSource.Play();
        }
    }

    ObjectTrigger TryGetObjectTrigger(GameObject hitObj)
    {
        if (hitObj != null)
        {
            return hitObj.GetComponent<ObjectTrigger>();
        }
        return null;
    }

    bool TryTriggerInteraction(ObjectTrigger triggerObj, TriggerTypes triggerType)
    {
        Debug.Log(triggerObj + "TriggerType: "+triggerType);
        if (triggerObj != null)
        {
            if (triggerObj.actionTrigger != null && triggerObj.actionTrigger.ActionType == triggerType.ToString())
            {
                StartCoroutine(triggerObj.TriggerAction(CompleteAction));
            }
            else if (attemptTime <= 0)
            {
                TriggerFailAttempt();
            }
            return true;
        }
        return false;
    }

    void TriggerFailAttempt()
    {
        attemptTime = failAttemptWaitTime;
        maxFailAttempts--;
        failedClipIndex++;
        if (failedClipIndex > FailedClips.Count)
        {
            failedClipIndex = 0;
        }

        StartCoroutine(PlayFailedSound());
    }

    public IEnumerator PlayFailedSound()
    {
        VoiceAudioSource.PlayOneShot(FailedClips[failedClipIndex]);
        yield return new WaitForSeconds(FailedClips[failedClipIndex].length);
        if (maxFailAttempts < 0)
            ScreenViewHandler.instance.FinalizeLevel(false);
    }

    void CompleteAction(EvilAction action)
    {
        if (GameState.instance.SolveAction(action))
        {
            Debug.Log("ACTION SOLVED: " + action.ActionType + " <> " + GameState.instance.CurrentPerson.EvilAction.Count + " left!");
            if (GameState.instance.CurrentPerson.EvilAction.Count <= 0)
            {
                ScreenViewHandler.instance.FinalizeLevel(true);
            }
        }
    }
}

interface InteractionTrigger
{
    IEnumerator TriggerAction(ActionComplete action);
    void StopAction();
    void ActionCompleted();
}

enum TriggerTypes
{
    voice,
    swipe,
    push,
    touch,
    scratch
}

public enum ThresholdLevel
{
    low,
    medium,
    high
}
