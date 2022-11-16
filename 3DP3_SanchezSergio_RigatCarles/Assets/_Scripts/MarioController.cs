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
    float verticalSpeed = 0.0f;
    bool onGround = false;
    bool touchingCeiling = false;

    [SerializeField] KeyCode fwKey;
    [SerializeField] KeyCode backKey;
    [SerializeField] KeyCode rightKey;
    [SerializeField] KeyCode leftKey;
    [SerializeField] KeyCode runKey;
    [SerializeField] KeyCode jumpKey;


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

        if (Input.GetKey(jumpKey) && onGround)
        {
            verticalSpeed = jumpSpeed;
        }

        if (movement.magnitude > 0.0f)
        {
            float currentSpeed = Input.GetKey(runKey) ? runSpeed : walkSpeed;
            movement = movement.normalized* currentSpeed* Time.deltaTime;
            transform.forward = movement;
        }
        animator.SetFloat("speed",movement.magnitude);
        
        
        //Apply gravity to verticalSpeed
        verticalSpeed += Physics.gravity.y * Time.deltaTime;
        movement.y += verticalSpeed * Time.deltaTime;

        //Move: charController.move
        CollisionFlags flags = charController.Move(movement);


        //If onGround -> verticlaSpeed=0
        onGround = (flags & CollisionFlags.Below) != 0;
        touchingCeiling = (flags & CollisionFlags.Above) != 0;

        if (onGround) verticalSpeed = 0.0f;
        if (touchingCeiling && verticalSpeed > 0.0f) verticalSpeed = 0.0f;

        
    }
}
