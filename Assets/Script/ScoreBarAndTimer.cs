using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreBarAndTimer : MonoBehaviour
{
    public static ScoreBarAndTimer instance;
    public TMP_Text timerText;
    public Slider scoreSlider;
    public TMP_Text r_Score;
    public TMP_Text g_Score;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }


    
}
