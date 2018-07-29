using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DrEvil.DataStructure
{

    /// <summary>
    /// A static AssetManager for handling Data such as Audio.
    /// </summary>
    public class AssetManager : MonoBehaviour
    {

        public static AssetManager instance;

        public AudioClip scratch;
        public AudioClip swipe;
        public AudioClip push;
        public AudioClip touch;
        public List<AudioClip> patient_scared_sounds;
        public List<AudioClip> patient_angry_sounds;

        // Use this for initialization
        void Awake()
        {
            instance = this;
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
