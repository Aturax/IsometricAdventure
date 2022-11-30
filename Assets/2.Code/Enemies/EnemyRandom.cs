using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class EnemyRandom : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 3.0f;
    [SerializeField] private bool hasTimer = false;
    [SerializeField] private float changeDirectionTimer = 2.0f;
    private float timer = 0.0f;

    private Vector2 moveDirection = Vector2.zero;
    private CharacterController characterController = null;
    private bool freeze = false;
    private float freezerTimer =0.0f;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    private void Start()
    {
        moveDirection = Vector2.left;
    }

    private void Update()
    {
        timer -= Time.deltaTime;
        if (hasTimer && timer <= 0.0f) ChangeDirection();
        
        if (!freeze) Move();
        else freezerTimer -= Time.deltaTime;
        if (freezerTimer < 0.0f) freeze = false;

        if (timer < 0.0f) timer = changeDirectionTimer;
    }

    private void Move()
    {
        characterController.Move(new Vector3(moveDirection.x * movementSpeed, 0.0f, moveDirection.y * movementSpeed) * Time.deltaTime);

        Vector3 lootAt = new Vector3(moveDirection.x, 0.0f, moveDirection.y);
        transform.rotation = Quaternion.LookRotation(lootAt, Vector3.up);
    }

    private void ChangeDirection()
    {
        moveDirection.x = Random.Range(-1.0f, 1.0f);
        moveDirection.y = Random.Range(-1.0f, 1.0f);

        moveDirection.Normalize();
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        ChangeDirection();

        if (hit.gameObject.CompareTag("IceBall"))
        {
            IceBall ice = hit.gameObject.GetComponent<IceBall>();
            freezerTimer = ice.GetFreezerTimer();
            freeze = true;
            ice.DestroyBall();
        }
    }
}
