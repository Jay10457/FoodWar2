using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class MaterialSlot : MonoBehaviour
{
    public static MaterialSlot instance;
    public List<GameObject> materials;
    [SerializeField] Item[] materialItems;
    [SerializeField] TMP_Text amountText = null;
    public int materialAmount;
    public Item currentCharacterMat;
    public Action addMaterialAmount;
    public Action reduceMaterialAmount;




    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        
    }
    private void Start()
    {

        currentCharacterMat = materialItems[SaveManager.instance.nowData.characterID];
        materialAmount = 5;
        addMaterialAmount = AddMaterialAmount;
        reduceMaterialAmount = ReduceMaterilaAmount;
        

    }
    private void AddMaterialAmount()
    {
        materialAmount += 1;
        amountText.text = string.Format("{0}/5 ", materialAmount);
        AmountTextPop();
        if (materialAmount > 0)
        {
            amountText.color = new Color(0.2941177f, 0.2313726f, 0.1882353f);
        }
        
    }
    public void AmountTextPop()
    {
        amountText.gameObject.transform.localScale = Vector3.one;
        if (amountText.gameObject.transform.localScale == Vector3.one)
        {
            
            LeanTween.scale(amountText.gameObject, Vector3.one * 1.5f, 0.5f).setEase(LeanTweenType.punch);
        }
        
        

    }

    private void ReduceMaterilaAmount()
    {
        materialAmount -= 1;
        amountText.text = string.Format("{0}/5 ", materialAmount);
        AmountTextPop();
        if (materialAmount <= 0)
        {
            materialAmount = 0;
            amountText.color = Color.red;
           
        }
    }

    public void SetMaterialImage(int _id)
    {
        materials[_id].SetActive(true);
    }
}
