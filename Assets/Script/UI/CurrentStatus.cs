using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CurrentStatus : MonoBehaviour
{
    public static CurrentStatus instance;
    public Text statusText;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        gameObject.SetActive(false);

    }
}
