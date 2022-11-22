using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MarioController : MonoBehaviour
{
    [SerializeField] Camera cam;
    [SerializeField] CharacterController charController;
    [SerializeField] Animator animator;

    [SerializeField] float walkSpeed;
    [SerializeField] float runSpeed;
    [SerializeField] float jumpSpeed;
    [SerializeField] int maxJumps;
    [SerializeField] Transform feet;
    [SerializeField] LayerMask groundMask;
    public int jumpCounter=0;
    float verticalSpeed = 0.0f;
    bool onGround = false;
    bool touchingCeiling = false;

    [SerializeField] KeyCode fwKey;
    [SerializeField] KeyCode backKey;
    [SerializeField] KeyCode rightKey;
    [SerializeField] KeyCode leftKey;
    [SerializeField] KeyCode runKey;
    [SerializeField] KeyCode jumpKey;

    [Header("Audio")]
    [SerializeField] AudioSource audioSource;


    void Update()
    {

        //Check Input
        //Movement: CameraDir, Input, Speed, deltaTime
        Vector3 fw = cam.transform.forward;
        fw.y = 0.0f;
        fw = fw.normalized;
        Vector3 right = cam.transform.right;
        right.y = 0.0f;
        right = right.normalized;
        Vector3 movement = Vector3.zero;
        if (Input.GetKey(fwKey))
            movement += fw;
        else if (Input.GetKey(backKey))
            movement -= fw;
        if (Input.GetKey(rightKey))
            movement += right;
        if (Input.GetKey(leftKey))
            movement -= right;

        if (Input.GetKeyDown(jumpKey) && jumpCounter < maxJumps)
        {
            verticalSpeed = jumpSpeed;
            jumpCounter++;
            
        }

        if (movement.magnitude > 0.0f)
        {
            float currentSpeed = Input.GetKey(runKey) ? runSpeed : walkSpeed;
            movement = movement.normalized* currentSpeed* Time.deltaTime;
            transform.forward = movement;
        }
        animator.SetFloat("speed", movement.magnitude);
        animator.SetInteger("jumpCounter", jumpCounter);
        animator.SetFloat("verticalSpeed", verticalSpeed);
        //Debug.Log(movement.magnitude);
        animator.SetBool("onGround", onGround);
        
        //Apply gravity to verticalSpeed
        verticalSpeed += Physics.gravity.y * Time.deltaTime;
        movement.y += verticalSpeed * Time.deltaTime;

        //Move: charController.move
        CollisionFlags flags = charController.Move(movement);


        //If onGround -> verticlaSpeed=0
        onGround = (flags & CollisionFlags.Below) != 0;
        touchingCeiling = (flags & CollisionFlags.Above) != 0;

        /*
        RaycastHit l_RaycastHit;
        Ray l_Ray = new Ray(feet.position, feet.forward);
        if (Physics.Raycast(l_Ray, out l_RaycastHit, 0f, groundMask))
        {
            onGround = true;
            Debug.Log("Tocas suelo, no?");
        }
        */
        if (onGround)
        {
            //Debug.Log("Grounded");
            verticalSpeed = 0.0f;
            jumpCounter = 0;
        }
        if (touchingCeiling && verticalSpeed > 0.0f) verticalSpeed = 0.0f;

    }




}
