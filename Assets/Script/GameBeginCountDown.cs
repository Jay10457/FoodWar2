using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameBeginCountDown : MonoBehaviour
{
    public static GameBeginCountDown instance;
    public TMP_Text countDownText;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        this.gameObject.SetActive(false);
    }
}
