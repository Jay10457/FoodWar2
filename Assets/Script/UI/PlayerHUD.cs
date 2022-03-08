using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHUD : MonoBehaviour
{
    [SerializeField] MaterialSlot materialSlot;
    private void Awake()
    {
        materialSlot = GetComponentInChildren<MaterialSlot>();
        materialSlot.SetMaterialImage(SaveManager.instance.nowData.characterID);
    }
}
