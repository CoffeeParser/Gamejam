using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    /// <summary>
    /// Let dont destroy all GameObjectChildren under this
    /// </summary>

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
