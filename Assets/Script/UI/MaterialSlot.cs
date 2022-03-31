using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialSlot : MonoBehaviour
{
    public static MaterialSlot instance;
    public List<GameObject> materials;
    [SerializeField] Item[] materialItems;
    public Item currentCharacterMat;
    


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
        

    }


    public void SetMaterialImage(int _id)
    {
        materials[_id].SetActive(true);
    }
}
