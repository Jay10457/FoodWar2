using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIK : MonoBehaviour
{
    static PlayerIK instance;
    [SerializeField] Animator animator;
    [SerializeField] Vector3 lookAt = Vector3.zero;


    [Range(0, 1)]
    [SerializeField] float weight = 1;
    [Range(0, 1)]
    [SerializeField] float bodyWeight;
    [Range(0, 1)]
    [SerializeField] float headWeight;
    [Range(0, 1)]
    [SerializeField] public float rightHandWeight;
    [SerializeField] Transform rHand;
    [SerializeField] Transform rHint;
    [SerializeField] Transform lookAtTransform;

    [SerializeField] Vector3 gunPos;
    [SerializeField] Quaternion gunRot;
    [SerializeField] Vector3 gunHint;

    [SerializeField] Vector3 bombPos;
    [SerializeField] Quaternion bombRot;
    [SerializeField] Vector3 bombHint;

    public int currentWeaponId;



    private void Awake()
    {
       
        lookAtTransform = GameObject.Find("CrossHairLookAt").transform;

        animator = GetComponent<Animator>();
        /*
        gunPos = new Vector3(1.65999997f, 1.99000001f, 1);
        gunRot = Quaternion.Euler(359.612396f, 36.8660126f, 353.045959f);
        gunHint = new Vector3(1.86099994f, 1.5f, 0.236000001f);
        bombPos = new Vector3(1.66999996f, 2.66000009f, -0.0900000036f);
        bombRot = Quaternion.Euler(355.146515f, 177.536423f, 320.388885f);
        bombHint = new Vector3(1.64499998f, 1.35599995f, 0.236000001f);
        */
        currentWeaponId = -1;

    }

    private void Update()
    {

        lookAt = lookAtTransform.position;



    }

    private void OnAnimatorIK(int layerIndex)
    {
        //Look IK

        lookAtIK();

        WeaponIK();








    }

    private void lookAtIK()
    {
        animator.SetLookAtPosition(lookAt);
        animator.SetLookAtWeight(weight, bodyWeight, headWeight);
    }

    private void WeaponIK()
    {
        if (currentWeaponId == 0)
        {
            rHand.localPosition = gunPos;
            rHand.localRotation = gunRot;
            rHint.localPosition = gunHint;
            rightHandWeight = 0.8f;
            
        }
        if (currentWeaponId == 1)
        {
            rHand.localPosition = bombPos;
            rHand.localRotation = bombRot;
            rHint.localPosition = bombHint;
            rightHandWeight = 1f;
        }
        if (currentWeaponId == -1)
        {
            rightHandWeight = 0;
        }
        animator.SetIKPosition(AvatarIKGoal.RightHand, rHand.position);
        animator.SetIKPositionWeight(AvatarIKGoal.RightHand, rightHandWeight);

        animator.SetIKHintPosition(AvatarIKHint.RightElbow, rHint.position);
        animator.SetIKHintPositionWeight(AvatarIKHint.RightElbow, rightHandWeight);

        animator.SetIKRotation(AvatarIKGoal.RightHand, rHand.rotation);
        animator.SetIKRotationWeight(AvatarIKGoal.RightHand, rightHandWeight);

    }

  






}
