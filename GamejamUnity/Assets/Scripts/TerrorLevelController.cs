using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MobileSensors;
using DrEvil.DataStructure;

namespace DrEvil.Mechanics
{
    /// <summary>
    /// This Class handles whole mechanics of the terror room
    /// </summary>
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

        public void Init()
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
        }

        private void Start()
        {


            attemptTime = failAttemptWaitTime;
            gyroCam = FindObjectOfType<GyroCamera>().GetComponent<Camera>();

        }

        private void Update()
        {
            attemptTime -= Time.deltaTime;
            attemptTime = Mathf.Max(0, attemptTime);
        }

        /// <summary>
        /// Add a scratch event to the game if it is defined in the gameData
        /// </summary>
        /// <param name="scratchEvilAction"></param>
        public void AddScratchAction(EvilAction scratchEvilAction)
        {
            GameObject tempGo = new GameObject();
            ObjectTrigger objTrigger = tempGo.AddComponent<ObjectTrigger>();
            objTrigger.actionTrigger = scratchEvilAction;
            scratchActions.Add(objTrigger);
        }

        /// <summary>
        /// Add a voice event to the game if it is defined in the gameData
        /// </summary>
        /// <param name="voiceEvilAction"></param>
        public void AddVoiceAction(EvilAction voiceEvilAction)
        {
            GameObject tempGo = new GameObject();
            ObjectTrigger objTrigger = tempGo.AddComponent<ObjectTrigger>();
            objTrigger.actionTrigger = voiceEvilAction;
            objTrigger.holdingTimeThreshold = .1f;
            objTrigger.triggerCompleteTime = 2f;
            objTrigger.isSolved = false;
            voiceActionObjs.Add(objTrigger);
        }

        /// <summary>
        /// Handling user scratch
        /// </summary>
        /// <param name="touch"></param>
        void OnUserScratch(Touch[] touch)
        {
            PlayAudio(AssetManager.instance.scratch, 0.5f);
            hasUserScratched = true;
        }

        /// <summary>
        /// Handling on user scratch ended
        /// </summary>
        /// <param name="touch"></param>
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

        /// <summary>
        /// Handling user voice threshold exceeded
        /// </summary>
        /// <param name="voiceLevel"></param>
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

        /// <summary>
        /// Handling user swipe
        /// </summary>
        /// <param name="touch"></param>
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

        /// <summary>
        /// Handling user Push (Acceleration)
        /// </summary>
        /// <param name="shakeThreshold"></param>
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

        /// <summary>
        /// Handling onUser Touch
        /// </summary>
        /// <param name="tapPostiion"></param>
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

        /// <summary>
        /// Play an audio
        /// </summary>
        /// <param name="clip"></param>
        /// <param name="timeStamp"></param>
        void PlayAudio(AudioClip clip, float timeStamp = 0)
        {
            if (!levelAudioSource.isPlaying)
            {
                levelAudioSource.time = timeStamp;
                levelAudioSource.clip = clip;
                levelAudioSource.Play();
            }
        }

        /// <summary>
        /// Try to trigger an object and get its object trigger
        /// </summary>
        /// <param name="hitObj"></param>
        /// <returns></returns>
        ObjectTrigger TryGetObjectTrigger(GameObject hitObj)
        {
            if (hitObj != null)
            {
                return hitObj.GetComponent<ObjectTrigger>();
            }
            return null;
        }

        /// <summary>
        /// Try to trigger an object
        /// </summary>
        /// <param name="triggerObj"></param>
        /// <param name="triggerType"></param>
        /// <returns></returns>
        bool TryTriggerInteraction(ObjectTrigger triggerObj, TriggerTypes triggerType)
        {
            Debug.Log(triggerObj + "TriggerType: " + triggerType);
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

        /// <summary>
        /// Call if an interaction was bad
        /// </summary>
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

        /// <summary>
        /// Play a failed sound feedback from the patient
        /// </summary>
        /// <returns></returns>
        public IEnumerator PlayFailedSound()
        {
            VoiceAudioSource.PlayOneShot(FailedClips[failedClipIndex]);
            yield return new WaitForSeconds(FailedClips[failedClipIndex].length);
            if (maxFailAttempts < 0)
                ScreenViewHandler.instance.FinalizeLevel(false);
        }

        /// <summary>
        /// Callback for solved Interactions
        /// </summary>
        /// <param name="action"></param>
        void CompleteAction(EvilAction action)
        {
            StartCoroutine(CompleteActionWithDelay(action));
        }

        /// <summary>
        /// Complete a solved interaction whith delay
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        IEnumerator CompleteActionWithDelay(EvilAction action)
        {
            if (GameState.instance.SolveAction(action))
            {
                Debug.Log("ACTION SOLVED: " + action.ActionType + " <> " + GameState.instance.CurrentPerson.EvilAction.Count + " left!");
                if (GameState.instance.CurrentPerson.EvilAction.Count <= 0)
                {
                    yield return new WaitForSeconds(3f);
                    ScreenViewHandler.instance.FinalizeLevel(true);
                }
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
