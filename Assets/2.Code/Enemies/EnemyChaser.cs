using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class EnemyChaser : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 5.0f;
    [SerializeField] private Vector2 moveDirection = Vector2.zero;

    private PlayerController player = null;
    private Vector3 playerCurrentPosition = Vector3.zero;
    private CharacterController characterController = null;

    private bool freeze = false;
    private float freezerTimer = 0.0f;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        player = FindObjectOfType<PlayerController>();        
    }

    private void Start()
    {
        moveDirection = Vector2.left;        
    }    

    void Update()
    {
        playerCurrentPosition = player.GetPosition();
        SetDirection();
        
        if (!freeze) Move();
        else freezerTimer -= Time.deltaTime;
        if (freezerTimer < 0.0f) freeze = false;
    }

    private void Move()
    {
        characterController.Move(new Vector3(moveDirection.x * movementSpeed, 0.0f, moveDirection.y * movementSpeed) * Time.deltaTime);
        
        Vector3 lootAt = new Vector3(moveDirection.x, 0.0f, moveDirection.y);
        transform.rotation = Quaternion.LookRotation(lootAt, Vector3.up);
    }

    private void SetDirection()
    {
        float x = playerCurrentPosition.x - transform.position.x;
        float y = playerCurrentPosition.z - transform.position.z;

        if (Mathf.Abs(y) > Mathf.Abs(x))
        {
            if (Mathf.Round(x) != 0.0f)
            {
                moveDirection = new Vector2(x, 0.0f).normalized;
            }
            else if (Mathf.Round(y) != 0.0f)
            {
                moveDirection = new Vector2(0.0f, y).normalized;
            }            
        }            
        else if (Mathf.Abs(y) < Mathf.Abs(x))
        {
            if (Mathf.Round(y) != 0.0f)
            {
                moveDirection = new Vector2(0.0f, y).normalized;
            }
            else if (Mathf.Round(x) != 0.0f)
            {
                moveDirection = new Vector2(x, 0.0f).normalized;
            }
        }            
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("IceBall"))
        {
            IceBall ice = hit.gameObject.GetComponent<IceBall>();
            freezerTimer = ice.GetFreezerTimer();
            freeze = true;
            ice.DestroyBall();
        }
    }
}
