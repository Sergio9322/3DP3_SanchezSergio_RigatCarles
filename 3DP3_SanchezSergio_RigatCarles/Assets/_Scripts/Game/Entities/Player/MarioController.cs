using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(MarioHealth))]
public class MarioController : MonoBehaviour, IRestartGameElement
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
    [SerializeField] ParticleSystem particleRun;
    [SerializeField] ParticleSystem particleJump;
    [SerializeField] bool enabledInput = true;

    [SerializeField] float walkSpeed;
    [SerializeField] float runSpeed;
    [SerializeField] float jumpSpeed;
    [SerializeField] int maxJumps;
    float currentSpeed;
    [SerializeField] Transform feet;
    [SerializeField] LayerMask groundMask;
    int jumpCounter=0;
    float m_VerticalSpeed = 0.0f;
    bool onGround=true;
    bool touchingCeiling = false;
    public float m_BridgeForce;

    [SerializeField] KeyCode fwKey;
    [SerializeField] KeyCode backKey;
    [SerializeField] KeyCode rightKey;
    [SerializeField] KeyCode leftKey;
    [SerializeField] KeyCode runKey;
    [SerializeField] KeyCode jumpKey;


    [Header("Colliders")]
    public Collider m_LeftHandCollider;
    public Collider m_RightHandCollider;
    public Collider m_KickCollider;

    [Header("Punch")]
    TPunchType m_CurrentPunch = TPunchType.RIGHT_HAND;
    float m_CurrentPunchTime;
    public float m_PunchComboTime = 0.25f;
    bool m_PunchActive = false;

    Vector3 m_StartPosition;
    Quaternion m_StartRotation;

    [Header("Elevator")]
    [SerializeField] float m_ElevatorMaxAngleAllowed = 10.0f;
    Collider m_CurrentElevator = null;

    Checkpoint m_CurrentCheckpoint = null;

    [Header("Combat")]
    [SerializeField] float m_KillGoombaMaxAngleAllowed = 30.0f;
    [SerializeField] float m_VerticalKillSpeed = 5.0f;

    [Header("Special Idle")]
    [SerializeField] float secsToSpecialIdle = 10;
    float waitingCounterIdle = 0f;

    [Header("Camera Comeback")]
    [SerializeField] float secsToCameraComeback = 5;
    float waitingCounterCamera = 0f;
    [SerializeField] UnityEvent m_CameraComebackEvent;

    MarioHealth m_MarioHealth;

    [Header("Wall jump")]
    [SerializeField] float slidingSpeed;
    bool canWallJump;
    [SerializeField] float noInputTime = 6f;


    void Awake()
    {
        GameController.GetGameController();
    }
    
    private void Start()
    {
        m_CurrentPunchTime = -m_PunchComboTime;
        m_LeftHandCollider.gameObject.SetActive(false);
        m_RightHandCollider.gameObject.SetActive(false);
        m_KickCollider.gameObject.SetActive(false);
        m_MarioHealth = GetComponent<MarioHealth>();

        m_StartPosition = transform.position;
        m_StartRotation = transform.rotation;
        GameController.GetGameController().AddRestartGameElement(this);
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

        checkFrontCollision();
        if (canWallJump)
        {
            if (Input.GetKeyDown(jumpKey))
            {
                WallJump();
            }
        }

        if(Input.GetMouseButtonDown(0) && CanPunch())
        {
            if (MustStartComboPunch())
            {
                SetComboPunch(TPunchType.RIGHT_HAND);
            }else
                NextComboPunch();
        }
        UpdateWaitingCounter();

        

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
        if (Input.GetKey(rightKey) )
            movement += right;
        if (Input.GetKey(leftKey))
            movement -= right;

        if (Input.GetKeyDown(jumpKey) && jumpCounter < maxJumps && currentSpeed >= runSpeed)
        {
            m_VerticalSpeed = jumpSpeed*0.8f;
            movement += fw*2;
            jumpCounter++;
            ResetWaitingCounter();
        }
        if (Input.GetKeyDown(jumpKey) && jumpCounter < maxJumps && currentSpeed < runSpeed)
        {
            if (jumpCounter == 0) particleJump.Play();
            m_VerticalSpeed = jumpSpeed;
            jumpCounter++;
            ResetWaitingCounter();
        }
        if (movement.magnitude > 0.0f)
        {
            currentSpeed = Input.GetKey(runKey) ? runSpeed : walkSpeed;
            movement = movement.normalized* currentSpeed* Time.deltaTime;
            transform.forward = movement;
            animator.SetFloat("speed", currentSpeed);
            if (currentSpeed == runSpeed && onGround) particleRun.Play();
            ResetWaitingCounter();
        }
        else { animator.SetFloat("speed", 0.0f); }
        
        animator.SetInteger("jumpCounter", jumpCounter);
        animator.SetFloat("verticalSpeed", m_VerticalSpeed);
        animator.SetBool("onGround", onGround);
        
        //Apply gravity to verticalSpeed
        m_VerticalSpeed += Physics.gravity.y * Time.deltaTime;
        movement.y += m_VerticalSpeed * Time.deltaTime;

        //Move: charController.move
        CollisionFlags flags = charController.Move(movement);


        //If onGround -> verticalSpeed=0

        
        onGround = Physics.Raycast(new Ray(feet.transform.position, Vector3.down), 0.2f);
        touchingCeiling = (flags & CollisionFlags.Above) != 0;

        if (onGround)
        {
            m_VerticalSpeed = 0.0f;
            jumpCounter = 0;
        }
        if (touchingCeiling && m_VerticalSpeed > 0.0f) m_VerticalSpeed = 0.0f;

    }

    private void checkFrontCollision()
    {
        bool facingDirection = Physics.Raycast(new Ray(new Vector3(transform.position.x, transform.position.y + 1.5f, transform.position.z), transform.forward), 0.5f);
        if (facingDirection && !onGround)
        {
            m_VerticalSpeed = slidingSpeed;
            canWallJump = true;
            StartCoroutine(DisableInput());
        } else canWallJump = false;
    }

    private IEnumerator DisableInput()
    {
        enabledInput = false;
        yield return new WaitForSeconds(noInputTime);
        canWallJump = false;
        enabledInput = true;
    }

    private void WallJump()
    {
        m_VerticalSpeed = jumpSpeed;
        transform.forward = -transform.forward;
        StartCoroutine(WallJumpMovement());
    }
    private IEnumerator WallJumpMovement()
    {
        for (int i = 0; i < 20; i++)
        {
            yield return new WaitForEndOfFrame();
            charController.Move(transform.forward * Time.deltaTime * 2f);
        }
        enabledInput = true;
        canWallJump = false;
    }

    void UpdateWaitingCounter()
    {
        waitingCounterIdle += Time.deltaTime;
        waitingCounterCamera += Time.deltaTime;

        if (waitingCounterIdle > secsToSpecialIdle) { animator.SetTrigger("specialIdle"); waitingCounterIdle = 0; }
        if (waitingCounterCamera > secsToCameraComeback) { m_CameraComebackEvent.Invoke(); waitingCounterCamera = 0; }
    }

    void ResetWaitingCounter()
    {
        waitingCounterIdle = 0;
        waitingCounterCamera = 0;
    }

    public void OnControllerColliderHit(ControllerColliderHit l_Hit)
    {
        if (l_Hit.collider.tag == "Bridge")
        {
            Rigidbody l_Bridge = l_Hit.collider.GetComponent<Rigidbody>();
            l_Bridge.AddForceAtPosition(-l_Hit.normal * m_BridgeForce, l_Hit.point);
        }
        else if (l_Hit.collider.tag == "Goomba" )
        {
            Goomba l_Goomba = l_Hit.collider.GetComponent<Goomba>();
            if (l_Goomba.IsAlive()) GoombaHit(l_Goomba, l_Hit);
        }
    }

    public void GoombaHit(Goomba l_Goomba, ControllerColliderHit l_Hit)
    {
        if (CanKillWithFeet(l_Hit.normal))
        {
            l_Goomba.KillJumping();
            JumpOverEnemy();
        }
        else GoombaHit(l_Goomba);
    }

    public void GoombaHit(Goomba l_Goomba)
    {
        if (l_Goomba.TryGetDamage())
        {
            m_MarioHealth.TakeDamage(l_Goomba.GetDamageAmount());
            Vector3 l_Direction = (l_Goomba.transform.position - transform.position).normalized;
            if (l_Goomba.TryGetComponent(out Impulsable l_GoombaImpulsable))
                l_GoombaImpulsable.GetImpulsed(l_Direction);
            if (this.TryGetComponent(out Impulsable l_MarioImpulsable))
                l_MarioImpulsable.GetImpulsed(-l_Direction);
        }
    }

    bool CanKillWithFeet(Vector3 Normal)
    {
        return m_VerticalSpeed < 0.0f && 
               Vector3.Dot(Normal, Vector3.up) > Mathf.Cos(m_KillGoombaMaxAngleAllowed * Mathf.Deg2Rad);
    }

    void JumpOverEnemy()
    {
        m_VerticalSpeed = m_VerticalKillSpeed;
    }

    void LateUpdate()
    {
        float l_AngleY = transform.rotation.eulerAngles.y;
        transform.rotation = Quaternion.Euler(0.0f, l_AngleY, 0.0f);
    }

    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Elevator")
        {
            if (m_CurrentElevator == null && CanAttachToElevator(other))
                AttachToElevator(other);
        }

        else if (other.tag == "Checkpoint")
            m_CurrentCheckpoint = other.gameObject.GetComponent<Checkpoint>();
        else if (other.gameObject.tag == "Pickable")
            other.GetComponent<IPickable>().Pick();
    }   

    private void OnTriggerStay(Collider other) {
        if (other.tag == "Elevator")
        {
            if (m_CurrentElevator == null)
            {
                if (CanAttachToElevator(other))
                    AttachToElevator(other); 
            }
            else
            {
                if (m_CurrentElevator == other && !CanAttachToElevator(other))
                    DetachElevator();
            }
        }
    }

    bool CanAttachToElevator(Collider other)
    {
        return Vector3.Dot(other.transform.forward, Vector3.up) > Mathf.Cos(m_ElevatorMaxAngleAllowed*Mathf.Deg2Rad);
    }

    void AttachToElevator(Collider other)
    {
        transform.SetParent(other.transform);
        m_CurrentElevator = other;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Elevator" && other == m_CurrentElevator)
            DetachElevator();
    }

    void DetachElevator()
    {
        transform.SetParent(null);
        m_CurrentElevator = null;
    }

    public void RestartGame()
    {
        charController.enabled = false;
        if (m_CurrentCheckpoint == null)
        {
            transform.position = m_StartPosition;
            transform.rotation = m_StartRotation;
        }
        else
        {
            transform.position = m_CurrentCheckpoint.m_RespawnPosition.position;
            transform.rotation = m_CurrentCheckpoint.m_RespawnPosition.rotation;
        }
        transform.SetParent(null);
        m_CurrentElevator = null;
        charController.enabled = true;
        ResetWaitingCounter();
    }

    public void SetAnimatorSpeedToZero() { animator.SetFloat("speed", 0.0f); }
}
