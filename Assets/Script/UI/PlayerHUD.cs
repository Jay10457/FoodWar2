using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHUD : MonoBehaviour
{
    [SerializeField] MaterialSlot materialSlot;
   
    public Animator oilGunFX;
    public static PlayerHUD instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        materialSlot = GetComponentInChildren<MaterialSlot>();
        materialSlot.SetMaterialImage(SaveManager.instance.nowData.characterID);
    }
}
