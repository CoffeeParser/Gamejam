﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventTestScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.Space))
        {
            //LevelManager.instance.WonTheGame.Invoke();
            //LevelManager.instance.OpenPopUp.Invoke();
            //StartCoroutine( LevelManager.instance.LoadAsynchonusly(1));
            //UIController.TakeDamageEvent.Invoke(10);
            

        }
    }
}
