using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class EnemyPattern : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 5.0f;
    [SerializeField] private Vector2 moveDirection = Vector2.zero;
    [SerializeField] private List<Transform> waypoints = null;
    private int indexWaypoint = 1;
    private CharacterController characterController = null;

    private bool freeze = false;
    private float freezerTimer = 0.0f;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    void Start()
    {
        GameObject go = new GameObject();
        go.transform.position = transform.position;
        waypoints = new List<Transform>();
        waypoints.Insert(0, go.transform);
    }

    void Update()
    {
        SetDirectionToWaypoint();

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

    private void SetDirectionToWaypoint()
    {
        float x = waypoints[indexWaypoint].position.x - transform.position.x;
        float z = waypoints[indexWaypoint].position.z - transform.position.z;

        if (Mathf.Abs(x) > 0.03f) moveDirection = new Vector2(x, 0.0f).normalized;
        else if (Mathf.Abs(z) > 0.03f) moveDirection = new Vector2(0.0f, z).normalized;
        
        if (Mathf.Abs(x) < 0.03f && Mathf.Abs(z) < 0.03f) SetNewWaypoint();        
    }

    private void SetNewWaypoint()
    {
        if (indexWaypoint < waypoints.Count-1) indexWaypoint++;
        else indexWaypoint = 0;

        SetDirectionToWaypoint();
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
