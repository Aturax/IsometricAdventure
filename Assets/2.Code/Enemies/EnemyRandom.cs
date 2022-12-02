using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class EnemyRandom : MonoBehaviour
{
    [SerializeField] private float _movementSpeed = 3.0f;
    [SerializeField] private bool _hasTimer = false;
    [SerializeField] private float _changeDirectionTimer = 2.0f;
    private float _timer = 0.0f;

    private Vector2 _moveDirection = Vector2.zero;
    private CharacterController _characterController = null;
    private bool _freeze = false;
    private float _freezerTimer =0.0f;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
    }

    private void Start()
    {
        _moveDirection = Vector2.left;
    }

    private void Update()
    {
        _timer -= Time.deltaTime;
        if (_hasTimer && _timer <= 0.0f) ChangeDirection();
        
        if (!_freeze) Move();
        else _freezerTimer -= Time.deltaTime;
        if (_freezerTimer < 0.0f) _freeze = false;

        if (_timer < 0.0f) _timer = _changeDirectionTimer;
    }

    private void Move()
    {
        _characterController.Move(new Vector3(_moveDirection.x * _movementSpeed, 0.0f, _moveDirection.y * _movementSpeed) * Time.deltaTime);

        Vector3 lootAt = new Vector3(_moveDirection.x, 0.0f, _moveDirection.y);
        transform.rotation = Quaternion.LookRotation(lootAt, Vector3.up);
    }

    private void ChangeDirection()
    {
        _moveDirection.x = Random.Range(-1.0f, 1.0f);
        _moveDirection.y = Random.Range(-1.0f, 1.0f);

        _moveDirection.Normalize();
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        ChangeDirection();

        if (hit.gameObject.CompareTag("IceBall"))
        {
            IceBall ice = hit.gameObject.GetComponent<IceBall>();
            _freezerTimer = ice.GetFreezerTimer();
            _freeze = true;
            ice.DestroyBall();
        }
    }
}
