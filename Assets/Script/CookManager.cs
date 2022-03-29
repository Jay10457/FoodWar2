using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CookManager : MonoBehaviour
{
    CookUI cookUI;
    
    private void Awake()
    {
        cookUI = CookUI.instance;
        
    }
    private void Start()
    {
        
    }

}
