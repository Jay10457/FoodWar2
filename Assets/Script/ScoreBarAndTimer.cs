using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreBarAndTimer : MonoBehaviour
{
    public static ScoreBarAndTimer instance;
    public Text timerText;
    public Slider scoreSlider;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }


    
}
