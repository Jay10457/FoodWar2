using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillCoolDown : MonoBehaviour
{
   
     public Image cdProgress;
     public TMP_Text timeText;
   
  


    
   
    private void Awake()
    {
        timeText.gameObject.SetActive(false);

        
    }
  
   


}
