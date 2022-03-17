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
    [SerializeField] float bodyWeight = 0.5f;
    [Range(0, 1)]
    [SerializeField] float headWeight = 0.5f;
    [Range(0, 1)]
    public float rightHandWeight;
    public float leftHandWeight;
    [SerializeField] Transform rHand;
    [SerializeField] Transform rHint;
    [SerializeField] Transform lHand;
    [SerializeField] Transform lHint;


    [SerializeField] Vector3 gunPos;
    [SerializeField] Quaternion gunRot;
    [SerializeField] Vector3 gunHint;

    [SerializeField] Vector3 bombPos;
    [SerializeField] Quaternion bombRot;
    [SerializeField] Vector3 bombHint;



    public int currentWeaponId;



    private void Awake()
    {



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

        lookAt = CrossHair.instance.transform.position;



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
            leftHandWeight = 0;

        }
        if (currentWeaponId == 1)
        {
            rHand.localPosition = bombPos;
            rHand.localRotation = bombRot;
            rHint.localPosition = bombHint;
            rightHandWeight = 0.9f;
            leftHandWeight = 0.8f;
        }
        if (currentWeaponId == 2)
        {
            rHand.localPosition = gunPos;
            rHand.localRotation = gunRot;
            rHint.localPosition = gunHint;
            rightHandWeight = 0.8f;
            leftHandWeight = 0;
        }
        if (currentWeaponId == -1)
        {
            rightHandWeight = 0;
            leftHandWeight = 0;
        }
        #region RightHand
        animator.SetIKPosition(AvatarIKGoal.RightHand, rHand.position);
        animator.SetIKPositionWeight(AvatarIKGoal.RightHand, rightHandWeight);

        animator.SetIKHintPosition(AvatarIKHint.RightElbow, rHint.position);
        animator.SetIKHintPositionWeight(AvatarIKHint.RightElbow, rightHandWeight);

        animator.SetIKRotation(AvatarIKGoal.RightHand, rHand.rotation);
        animator.SetIKRotationWeight(AvatarIKGoal.RightHand, rightHandWeight);
        #endregion
        #region LeftHand
        animator.SetIKPosition(AvatarIKGoal.LeftHand, lHand.position);
        animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, leftHandWeight);

        animator.SetIKRotation(AvatarIKGoal.LeftHand, lHand.rotation);
        animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, leftHandWeight);

        animator.SetIKHintPosition(AvatarIKHint.LeftElbow, lHint.position);
        animator.SetIKHintPositionWeight(AvatarIKHint.LeftElbow, leftHandWeight);
        #endregion
    }








}
