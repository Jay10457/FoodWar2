using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialSlot : MonoBehaviour
{
    public List<GameObject> materials;

    public void SetMaterialImage(int _id)
    {
        materials[_id].SetActive(true);
    }
}
