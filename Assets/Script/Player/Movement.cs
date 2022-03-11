using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Cinemachine;

public class Movement : MonoBehaviour
{
    Animator ani;
    Rigidbody rb;
    //[SerializeField] Transform playerPos;
    [SerializeField] Transform camLookAt = null;
    [SerializeField] CinemachineVirtualCamera Vcam = null;
    [SerializeField] Vector3 playerMoveInput = Vector3.zero;
    [SerializeField] float yaw = 0;
    [SerializeField] float pitch = 0;
    [SerializeField] float velocity = 10;
    [SerializeField] Vector3 moveDir = Vector3.zero;
    [SerializeField] float sprintForce = 0;
    [SerializeField] float sprintTime;
    [SerializeField] float MOUSE_SENTIVY = 10f;
    [SerializeField] float syncRate = 10f;
    [SerializeField] float distanceToSync = 0.1f;
    [SerializeField] float cdTime = 10;
    [SerializeField] GameObject gun;
    [SerializeField] GameObject bumb;
    [SerializeField] SkillCoolDown skillCoolDown;
    public List<GameObject> characters;
    float angle = 0;
    bool sprint;
    public bool isSprinting = false;
    bool isSkillOk = true;
    Vector3 angles = Vector3.zero;

    Vector3 rpcPos = Vector3.zero;
    Quaternion rpcRot = Quaternion.identity;
    Vector3 lastPos = Vector3.zero;


    Quaternion nextRotation;




    private void Start()
    {


        ani = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody>();
        skillCoolDown = GameObject.FindObjectOfType<SkillCoolDown>();

        Cursor.lockState = CursorLockMode.Locked;



        Vcam = FindObjectOfType<CinemachineVirtualCamera>();
        camLookAt = this.gameObject.transform.Find("camLookAt");

        if (true)// if team == green
        {
            camLookAt.transform.localPosition = new Vector3(0, 3.5f, 0);
        }
        else // if team == red 
        {
            camLookAt.transform.localPosition = new Vector3(0, 3, 0);
        }






       


    }

    //PlayerInput;
    private void Update()
    {
        InputMagnitude();
        Sprint();
        #region testIK

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ani.SetLayerWeight(1, 1);
            ani.SetLayerWeight(2, 0);
            gun.SetActive(true);
            bumb.SetActive(false);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ani.SetLayerWeight(1, 0);
            ani.SetLayerWeight(2, 1);
            gun.SetActive(false);
            bumb.SetActive(true);

        }
        #endregion
    }

    void InputMagnitude()
    {
        playerMoveInput = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        yaw = -Input.GetAxis("Mouse Y");
        pitch = Input.GetAxis("Mouse X");
        sprint = Input.GetKeyDown(KeyCode.LeftShift) ? true : false;

    }
    private void Sprint()
    {
        if (sprint && isSkillOk && !isSprinting)
        {
            //Debug.LogError("sprint");
            StartCoroutine(SprintCoroutine());

        }

        
    }
    private IEnumerator SprintCoroutine()
    {
        float startTime = Time.time;
        float originVelocity = velocity;

        isSkillOk = false;
        while (Time.time < startTime + sprintTime)
        {

            isSprinting = true;
            velocity = sprintForce;


            yield return null;
            isSprinting = false;

            velocity = originVelocity;

        }
        StartCoroutine(CoolDown());
    }
    private IEnumerator CoolDown()
    {
        float startTime = Time.time;
        skillCoolDown.timeText.gameObject.SetActive(true);

        while (Time.time < startTime + cdTime)
        {


            skillCoolDown.timeText.text = ((startTime + cdTime) - Time.time).ToString("0");
            skillCoolDown.cdProgress.fillAmount = ((startTime + cdTime) - Time.time) / cdTime;


            yield return null;
        }
        skillCoolDown.timeText.gameObject.SetActive(false);
        isSkillOk = true;

    }
    private void LateUpdate()
    {
        Vcam.Follow = camLookAt;
        CameraRotation();
        AnimatePlayer();


    }

    private void Move()
    {
        moveDir = Vector3.ClampMagnitude(transform.TransformDirection(playerMoveInput), 1) * velocity;
        //moveDir = transform.TransformDirection(playerMoveInput) * velocity;
        rb.velocity = new Vector3(moveDir.x, rb.velocity.y, moveDir.z);




        

    }


    private void FixedUpdate()
    {

        Move();
        PlayerRotate();


    }

    private void CameraRotation()
    {
        camLookAt.transform.rotation *= Quaternion.AngleAxis(pitch * MOUSE_SENTIVY, Vector3.up);
        camLookAt.transform.rotation *= Quaternion.AngleAxis(yaw * MOUSE_SENTIVY, Vector3.right);
        angles = camLookAt.transform.localEulerAngles;
        angles.z = 0;
        angle = camLookAt.transform.localEulerAngles.x;


        //Clamp down and Up
        if (angle > 180 && angle < 340)
        {
            angles.x = 340;
        }
        else if (angle < 180 && angle > 45)
        {
            angles.x = 45;
        }


        camLookAt.transform.localEulerAngles = angles;
        //nextRotation = Quaternion.Lerp(camLookAt.transform.rotation, nextRotation, Time.deltaTime * 0.5f);


    }

    private void AnimatePlayer()
    {
        ani.SetFloat("MoveX", playerMoveInput.x);
        ani.SetFloat("MoveZ", playerMoveInput.z);
        if (isSprinting)
        {
            ani.SetBool("Sprint", true);
        }
        else
        {
            ani.SetBool("Sprint", false);
        }

    }

    private void PlayerRotate()
    {
        
        Quaternion camDir = Quaternion.Euler(0, camLookAt.transform.rotation.eulerAngles.y, 0);
        //playerPos.rotation = Quaternion.Lerp(playerPos.rotation, camDir, Time.fixedDeltaTime * 10f);

        //rb.MoveRotation(Quaternion.Lerp(rb.rotation, camDir, Time.fixedDeltaTime * 10f));

        rb.MoveRotation(camDir);
        //rb.MoveRotation(Quaternion.Slerp(rb.rotation, camDir, 4f * Time.deltaTime));
        camLookAt.transform.localEulerAngles = new Vector3(angles.x, 0, 0);

    }


}
