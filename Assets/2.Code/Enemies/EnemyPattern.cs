using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class EnemyPattern : MonoBehaviour
{
    [SerializeField] private float _movementSpeed = 5.0f;
    [SerializeField] private Vector2 _moveDirection = Vector2.zero;
    [SerializeField] private List<Transform> _waypoints = null;
    private int _indexWaypoint = 1;
    private CharacterController _characterController = null;

    private bool _freeze = false;
    private float _freezerTimer = 0.0f;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
    }

    void Start()
    {
        GameObject go = new GameObject();
        go.transform.position = transform.position;
        _waypoints = new List<Transform>();
        _waypoints.Insert(0, go.transform);
    }

    void Update()
    {
        SetDirectionToWaypoint();

        if (!_freeze) Move();
        else _freezerTimer -= Time.deltaTime;
        if (_freezerTimer < 0.0f) _freeze = false;
    }

    private void Move()
    {
        _characterController.Move(new Vector3(_moveDirection.x * _movementSpeed, 0.0f, _moveDirection.y * _movementSpeed) * Time.deltaTime);

        Vector3 lootAt = new Vector3(_moveDirection.x, 0.0f, _moveDirection.y);
        transform.rotation = Quaternion.LookRotation(lootAt, Vector3.up);
    }

    private void SetDirectionToWaypoint()
    {
        float x = _waypoints[_indexWaypoint].position.x - transform.position.x;
        float z = _waypoints[_indexWaypoint].position.z - transform.position.z;

        if (Mathf.Abs(x) > 0.03f) _moveDirection = new Vector2(x, 0.0f).normalized;
        else if (Mathf.Abs(z) > 0.03f) _moveDirection = new Vector2(0.0f, z).normalized;
        
        if (Mathf.Abs(x) < 0.03f && Mathf.Abs(z) < 0.03f) SetNewWaypoint();        
    }

    private void SetNewWaypoint()
    {
        if (_indexWaypoint < _waypoints.Count-1) _indexWaypoint++;
        else _indexWaypoint = 0;

        SetDirectionToWaypoint();
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("IceBall"))
        {
            IceBall ice = hit.gameObject.GetComponent<IceBall>();
            _freezerTimer = ice.GetFreezerTimer();
            _freeze = true;
            ice.DestroyBall();
        }
    }
}
