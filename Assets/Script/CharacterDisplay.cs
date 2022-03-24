using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterDisplay : MonoBehaviour
{
    
   [SerializeField] GameObject[] characters;
    [SerializeField] ParticleSystem changeEffect;
    int currentIndex;

    private void Awake()
    {
        currentIndex = Random.Range(0, characters.Length);
        characters[currentIndex].SetActive(true);
    }

    private int RadomNotRepeat()
    {
        int temp = Random.Range(0, characters.Length);
        if (temp != currentIndex)
        {
            currentIndex = temp;
            return temp;
        }
        else
        {
            return RadomNotRepeat();
        }
    }

    public void RandomCharacter()
    {
        Instantiate(changeEffect, transform.position, Quaternion.Euler(-90, 0, 0));
        characters[currentIndex].SetActive(false);
        int temp = RadomNotRepeat();
        characters[temp].SetActive(true);
    }
}
