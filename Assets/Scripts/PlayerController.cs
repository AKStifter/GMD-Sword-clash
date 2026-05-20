using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public InputActionAsset InputActions;
    public Transform enemy;
    public float rotationSpeed = 10f;
    public GameObject[] allweapons;


    private InputAction m_moveAction;
    private InputAction m_jumpAction;
    private InputAction m_attackAction;
    private InputAction m_defendAction;
    private InputAction interactAction;
    private GameObject activeWeapon = null;
    private GameObject activeShield = null;
    private GameObject nearbyWeapon;

    private bool isChargingAttack;
    private float attackHoldTime;


    private Vector2 m_moveAMT;
    private Animator m_animator;
    private Rigidbody m_rigidbody;

    public float WalkSpeed = 5;
    public float JumpSpeed = 5;


    private void OnEnable()
    {
        InputActions.FindActionMap("Player").Enable();
    }

    private void OnDisable()
    {
        InputActions.FindActionMap("Player").Disable();
    }

    private void Awake()
    {
        

    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        m_animator = GetComponent<Animator>();
        m_rigidbody = GetComponent<Rigidbody>();      

        m_moveAction = InputSystem.actions.FindAction("Move");
        m_jumpAction = InputSystem.actions.FindAction("Jump");
        m_attackAction = InputSystem.actions.FindAction("Attack");
        // m_defendAction = InputSystem.actions.FindAction("Defend");  
        interactAction = InputSystem.actions.FindAction("Interact");
    }

    // Update is called once per frame
    void Update()
    {

         Vector3 direction = enemy.position - transform.position;
        direction.y = 0f; // ignore vertical difference

        if (direction.sqrMagnitude < 0.001f)
            return;

        m_moveAMT = m_moveAction.ReadValue<Vector2>();

        if (m_jumpAction.WasPressedThisFrame())
        {
            Jump();
        }
        if (interactAction.WasPressedThisFrame() && nearbyWeapon != null)
        {
            PickupWeapon();
        }
        if (m_attackAction.WasPressedThisFrame())
        {
            StartChargeAttack();
        }
        if (isChargingAttack)
        {
            attackHoldTime += Time.deltaTime;
        }
        if (m_attackAction.WasReleasedThisFrame())
        {
            ReleaseAttack();
        }
        
        // if (m_defendAction.WasPressedThisFrame())
        // {
        //     m_animator.SetTrigger("Defend");
        // }

    }

    private void FixedUpdate()
    {
        FaceEnemy();
        Walking();
    }

    private void FaceEnemy()
{
    if (enemy == null) return;

    Vector3 direction = enemy.position - m_rigidbody.position;
    direction.y = 0f;

    if (direction.sqrMagnitude < 0.001f) return;

    Quaternion targetRotation = Quaternion.LookRotation(direction);

    Quaternion newRotation = Quaternion.Slerp(
        m_rigidbody.rotation,
        targetRotation,
        rotationSpeed * Time.fixedDeltaTime
    );

    m_rigidbody.MoveRotation(newRotation);
}

   private void Walking()
{
    if (enemy == null) return;

    // Get camera-independent movement direction based on INPUT only
    Vector3 inputDir = new Vector3(m_moveAMT.x, 0f, m_moveAMT.y);
    inputDir = inputDir.normalized;

    // Convert input into world space relative to enemy-facing rotation
    Quaternion flatRotation = Quaternion.Euler(0f, m_rigidbody.rotation.eulerAngles.y, 0f);
    Vector3 move = flatRotation * inputDir;

    m_rigidbody.MovePosition(
        m_rigidbody.position + move * WalkSpeed * Time.fixedDeltaTime
    );

    m_animator.SetBool("isRunning", inputDir.sqrMagnitude > 0.01f);
}
    public void Jump()
    {
        m_animator.SetTrigger("Jump");
        m_rigidbody.AddForce(Vector3.up * JumpSpeed, ForceMode.Impulse);
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Sword- bastard" || other.gameObject.tag == "PaladinShield"
            || other.gameObject.tag == "GreatSword")
        {
            nearbyWeapon = other.gameObject;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == nearbyWeapon)
        {
            nearbyWeapon = null;
        }
    }
    private void PickupWeapon()
    {
        if (nearbyWeapon.CompareTag("Sword- bastard"))
        {
            Destroy(nearbyWeapon);
            if (activeWeapon != null)
            {
                activeWeapon.SetActive(false);

                if(activeWeapon.gameObject.tag == "GreatSword")
                {
                    m_animator.SetBool("TwoHanded", false);
                }
            }
            
            allweapons[0].SetActive(true);
            activeWeapon = allweapons[0];
            m_animator.SetBool("OneHandSword", true);

            nearbyWeapon = null;
            return;
        }

        if (nearbyWeapon.CompareTag("PaladinShield"))
        {
            Destroy(nearbyWeapon);

            if (activeWeapon != null)
            {
                activeWeapon.SetActive(false);
                
                if(activeWeapon.gameObject.tag == "GreatSword")
                {
                    m_animator.SetBool("TwoHanded", false);
                    activeWeapon.SetActive(false);
                }
            }
            allweapons[1].SetActive(true);
            activeShield = allweapons[1];
            m_animator.SetBool("HasShield", true);


            nearbyWeapon = null;
            return;
        }
        if (nearbyWeapon.CompareTag("GreatSword"))
        {
            Destroy(nearbyWeapon);
            if (activeWeapon != null)
            {
                activeWeapon.SetActive(false);
            }
            if (activeShield != null)
            {
                activeShield.SetActive(false);
                m_animator.SetBool("HasShield", false);
                activeShield = null;
            }
            allweapons[2].SetActive(true);
            activeWeapon = allweapons[2];
            m_animator.SetBool("OneHandSword", false);
            m_animator.SetBool("TwoHanded", true);

            nearbyWeapon = null;
            return;
        }        
    }

    private void StartChargeAttack()
    {
        isChargingAttack = true;
        attackHoldTime = 0f;

    }

    private void ReleaseAttack()
    {
        isChargingAttack = false;

        if (attackHoldTime < 0.1f)
        {
            m_animator.SetTrigger("Attack");
        }
        else
        {
            m_animator.SetTrigger("HeavyAttack");
        }
    }
}
