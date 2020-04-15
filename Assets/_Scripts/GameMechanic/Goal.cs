﻿using UnityEngine;
using System.Collections;
using System;
using UnityEngine.SceneManagement;

public class Goal : MonoBehaviour {
    void OnTriggerEnter2D (Collider2D other) {
        if (other.tag == "Player")
        {
            StageManager.InitializeNextStage();
        }
    }
}
