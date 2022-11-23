using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MarioController : MonoBehaviour
{
    public enum TPunchType
    {
        RIGHT_HAND,
        LEFT_HAND,
        KICK
    }

    
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
    public float m_BridgeForce;

    [SerializeField] KeyCode fwKey;
    [SerializeField] KeyCode backKey;
    [SerializeField] KeyCode rightKey;
    [SerializeField] KeyCode leftKey;
    [SerializeField] KeyCode runKey;
    [SerializeField] KeyCode jumpKey;

    [Header("Audio")]
    [SerializeField] AudioSource audioSource;

    [Header("Colliders")]
    public Collider m_LeftHandCollider;
    public Collider m_RightHandCollider;
    public Collider m_KickCollider;

    [Header("Punch")]
    TPunchType m_CurrentPunch = TPunchType.RIGHT_HAND;
    float m_CurrentPunchTime;
    public float m_PunchComboTime = 0.25f;
    bool m_PunchActive = false;

    private void Start()
    {
        m_CurrentPunchTime = -m_PunchComboTime;
        m_LeftHandCollider.gameObject.SetActive(false);
        m_RightHandCollider.gameObject.SetActive(false);
        m_KickCollider.gameObject.SetActive(false);
    }

    public void HitPunch(TPunchType PunchType,bool Active)
    {
        if (PunchType == TPunchType.LEFT_HAND)
            m_LeftHandCollider.gameObject.SetActive(Active);
        else if(PunchType == TPunchType.RIGHT_HAND)
            m_RightHandCollider.gameObject.SetActive(Active);
        else if(PunchType == TPunchType.KICK)
            m_KickCollider.gameObject.SetActive(Active);
    }
    bool CanPunch()
    {
        return !m_PunchActive;
    }
    public void SetPunchActive(bool PunchActive)
    {
        m_PunchActive = PunchActive;
    }

    bool MustStartComboPunch()
    {
        return (Time.time - m_CurrentPunchTime) > m_PunchComboTime;
    }
    void NextComboPunch()
    {
        if (m_CurrentPunch == TPunchType.RIGHT_HAND)
            SetComboPunch(TPunchType.LEFT_HAND);
        else if (m_CurrentPunch == TPunchType.LEFT_HAND)
            SetComboPunch(TPunchType.KICK);
        else if (m_CurrentPunch == TPunchType.KICK)
            SetComboPunch(TPunchType.RIGHT_HAND);
    }

    void SetComboPunch(TPunchType PunchType)
    {
        m_CurrentPunch = PunchType;
        if (PunchType == TPunchType.RIGHT_HAND)
            animator.SetTrigger("Punch1");
        else if (PunchType == TPunchType.LEFT_HAND)
            animator.SetTrigger("Punch2");
        else if (PunchType == TPunchType.KICK)
            animator.SetTrigger("Punch3");
        m_CurrentPunchTime = Time.time;
        m_PunchActive = true;
    }


    void Update()
    {
        if(Input.GetMouseButtonDown(0) && CanPunch())
        {
            if (MustStartComboPunch())
            {
                SetComboPunch(TPunchType.RIGHT_HAND);
            }
            else
                NextComboPunch();
        }


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


    public void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.collider.tag == "Bridge")
        {
            Rigidbody l_Bridge = hit.collider.GetComponent<Rigidbody>();
            l_Bridge.AddForceAtPosition(-hit.normal * m_BridgeForce, hit.point);
        }
    }





}
