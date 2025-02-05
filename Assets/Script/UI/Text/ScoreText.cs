﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreText : MonoBehaviour
{
    TextMeshProUGUI timerText;

    void Start()
    {
        timerText = GetComponent<TextMeshProUGUI>();
        timerText.text = "0";
    }
    
    public void Text_UI_Update()
    { 
        timerText.text = string.Format("{0:#,##0}", Managers.Game.total_score); 
    }
}
