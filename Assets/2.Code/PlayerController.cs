using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerController : MonoBehaviour
{

    [SerializeField] private float movementSpeed = 5.0f;
    [SerializeField] private IceBall iceBallPrefab = null;
    [SerializeField] private Transform iceBallSpawnPoint = null;
    [Header("Physics")]
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float gravityModifier = -2.0f;
    [SerializeField] private float jumpHeight = 1f;

    [Header("Equipment")]
    [SerializeField] private GameObject hat = null;
    [SerializeField] private GameObject staff = null;
    [SerializeField] private GameObject book = null;

    [Header("Spells")]
    [SerializeField] private float hatModifier = 2.0f;
    [SerializeField] private float hasteModifier = 2.0f;    
    [Space]    
    [SerializeField] private float iceBallCooldown = 3.0f;
    private float iceBallTimer = 0.0f;
    [Space]
    [SerializeField] private bool isHasted = false;
    [SerializeField] private float hasteDuration = 5.0f;
    [SerializeField] private float hasteCooldown = 5.0f;
    private float hasteTimer = 0.0f;

    private Vector3 playerVelocity = Vector3.zero;
    private CharacterController characterController = null;
    private InputController inputController = null;
    private bool jumping = false;

    [SerializeField] private Animator animator = null;
    private int animationBlend = 0;

    public event Action<DoorTrigger> Teleport = (DoorTrigger doorTrigger) => { };


    private void Awake()
    {
        characterController = GetComponent<CharacterController>();        
    }

    private void Start()
    {
        inputController = new InputController();
        animationBlend = Animator.StringToHash("Blend");
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    private void Update()
    {
        CaptureInputMovement();
        CaptureInputSpell();

        if (hat.activeInHierarchy) hatModifier = 2.0f;
        else hatModifier = 1.0f;

        CheckSpells();
        Gravity(gravity / hatModifier);
        Jump(gravity / hatModifier);
        Move();
        Rotate();
    }

    private void CaptureInputMovement()
    {
        if (characterController.isGrounded || hat.activeInHierarchy)
        {
            Vector2 input = inputController.Move().normalized;

            // This is for continuing with the direction while jumping with the hat
            if (input == Vector2.zero)
            {
                animator.SetFloat(animationBlend, 0.0f);

                if (hat.activeInHierarchy && jumping) return;
            }
            
            playerVelocity = new Vector3(input.y, playerVelocity.y, -input.x);            
        }
    }

    private void CaptureInputSpell()
    {
        if (book.activeInHierarchy && inputController.HasteSpell() && !isHasted && hasteTimer <= 0.0f)
        {
            isHasted = true;
            hasteTimer = hasteDuration;
        }

        if (staff.activeInHierarchy && inputController.IceBall() && iceBallTimer <= 0.0f)
        {
            iceBallTimer = iceBallCooldown;
            IceBall iceBall = Instantiate(iceBallPrefab, iceBallSpawnPoint.position, transform.rotation);
        }
    }

    private void CheckSpells()
    {
        iceBallTimer -= Time.deltaTime;
        hasteTimer -= Time.deltaTime;

        if (isHasted)
        {
            hasteModifier = 2.0f;            

            if (hasteTimer <= 0.0f)
            {
                isHasted = false;
                hasteTimer = hasteCooldown;
            }
        }
        else
        {
            hasteModifier = 1.0f;            
        }

        if (inputController.Move().normalized == Vector2.zero)
        {
            animator.SetFloat(animationBlend, 0.0f);
        }
        else
        {
            animator.SetFloat(animationBlend, hasteModifier);
        }
    }

    private void Gravity(float gravityValue)
    {
        if (!characterController.isGrounded)
        {
            playerVelocity.y += gravityValue * Time.deltaTime;
        }
        else
        {
            jumping = false;
            playerVelocity.y = -2.0f;
        }
    }

    private void Jump(float gravityValue)
    {
        if (characterController.isGrounded && inputController.Jump() > 0.0f)
        {
            jumping = true;
            playerVelocity.y = Mathf.Sqrt(jumpHeight * gravityModifier * gravityValue);
        }
    }

    private void Move()
    {
        if (jumping)
        {
            characterController.Move(new Vector3(playerVelocity.x * movementSpeed, playerVelocity.y, playerVelocity.z * movementSpeed) * Time.deltaTime);
        }
        else
        {
            float speed = movementSpeed * hasteModifier;
            characterController.Move(new Vector3(playerVelocity.x * speed, playerVelocity.y, playerVelocity.z * speed) * Time.deltaTime);
        }
    }

    private void Rotate()
    {
        if (playerVelocity.x == 0.0f && playerVelocity.z == 0.0f) return;
        
        Vector3 lootAt = new Vector3(playerVelocity.x, 0.0f, playerVelocity.z);
        transform.rotation = Quaternion.LookRotation(lootAt, Vector3.up);
    }

    public void PickItem(Item itemPicked)
    {
        if (itemPicked == Item.Hat) hat.SetActive(true);
        else if (itemPicked == Item.Staff) staff.SetActive(true);
        else book.SetActive(true);
    }

    public void TeleportPlayer(Vector3 position)
    {
        transform.position = position;        
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("Teleport"))
        {
            Teleport(hit.gameObject.GetComponent<DoorTrigger>());
        }
    }
}
