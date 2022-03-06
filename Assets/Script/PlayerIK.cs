using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIK : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] Vector3 lookAt;
    [SerializeField] Transform gunPos;
    [SerializeField] Transform bumbPos;
    [SerializeField] Transform rGunHintPos;
    [SerializeField] Transform rBumbHintPos;
    [SerializeField] Transform lHintPos;

    [Range(0, 1)]
    [SerializeField] float weight = 1;
    [Range(0, 1)]
    [SerializeField] float bodyWeight;
    [Range(0, 1)]
    [SerializeField] float headWeight;
    [Range(0, 1)]
    [SerializeField] float rightHandWeight;
    Transform lookAtTransform;



    private void Awake()
    {
        lookAtTransform = GameObject.Find("CrossHair").transform;
        animator = GetComponent<Animator>();
        
    }

    private void Update()
    {
        lookAt = lookAtTransform.position;
        
    }

    private void OnAnimatorIK(int layerIndex)
    {
        //Look IK
        animator.SetLookAtPosition(lookAt);
        animator.SetLookAtWeight(weight, bodyWeight, headWeight);

        if (layerIndex == 1 && animator.GetLayerWeight(1) == 1)
        {
            GunIK();
        }
        if (layerIndex == 2 && animator.GetLayerWeight(2) == 1)
        {
            BumbIK();
        }



    }

    public void GunIK()
    {

        
        animator.SetIKPosition(AvatarIKGoal.RightHand, gunPos.position);
        animator.SetIKRotation(AvatarIKGoal.RightHand, gunPos.rotation);
        animator.SetIKPositionWeight(AvatarIKGoal.RightHand, rightHandWeight);
        animator.SetIKRotationWeight(AvatarIKGoal.RightHand, rightHandWeight);


        animator.SetIKHintPosition(AvatarIKHint.RightElbow, rGunHintPos.position);
        animator.SetIKHintPositionWeight(AvatarIKHint.RightElbow, rightHandWeight);
    }

    public void BumbIK()
    {
        
        animator.SetIKPosition(AvatarIKGoal.RightHand, bumbPos.position);
        animator.SetIKRotation(AvatarIKGoal.RightHand, bumbPos.rotation);
        animator.SetIKPositionWeight(AvatarIKGoal.RightHand, rightHandWeight);
        animator.SetIKRotationWeight(AvatarIKGoal.RightHand, rightHandWeight);

        animator.SetIKHintPosition(AvatarIKHint.RightElbow, rBumbHintPos.position);
        animator.SetIKHintPositionWeight(AvatarIKHint.RightElbow, rightHandWeight);
    }






}
