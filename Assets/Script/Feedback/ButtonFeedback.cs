using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonFeedback : EventTrigger
{
    [SerializeField] public float scaleSize;
    [SerializeField] public AudioSource source;
    [SerializeField] public AudioClip clip;

    private void Awake()
    {
        source = this.GetComponent<AudioSource>();
    }
    public override void OnPointerEnter(PointerEventData eventData)
    {
        
        source.PlayOneShot(clip, 0.3f);
        LeanTween.scale(this.gameObject, Vector3.one * scaleSize, 0.1f);





    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        LeanTween.scale(this.gameObject, Vector3.one, 0.1f);
    }

}

