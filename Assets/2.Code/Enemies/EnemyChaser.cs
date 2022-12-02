using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class EnemyChaser : MonoBehaviour
{
    [SerializeField] private float _movementSpeed = 5.0f;
    [SerializeField] private Vector2 _moveDirection = Vector2.zero;

    private PlayerController _player = null;
    private Vector3 _playerCurrentPosition = Vector3.zero;
    private CharacterController _characterController = null;

    private bool _freeze = false;
    private float _freezerTimer = 0.0f;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _player = FindObjectOfType<PlayerController>();        
    }

    private void Start()
    {
        _moveDirection = Vector2.left;        
    }    

    void Update()
    {
        _playerCurrentPosition = _player.GetPosition();
        SetDirection();
        
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

    private void SetDirection()
    {
        float x = _playerCurrentPosition.x - transform.position.x;
        float y = _playerCurrentPosition.z - transform.position.z;

        if (Mathf.Abs(y) > Mathf.Abs(x))
        {
            if (Mathf.Round(x) != 0.0f)
                _moveDirection = new Vector2(x, 0.0f).normalized;
            else if (Mathf.Round(y) != 0.0f)
                _moveDirection = new Vector2(0.0f, y).normalized;            
        }            
        else if (Mathf.Abs(y) < Mathf.Abs(x))
        {
            if (Mathf.Round(y) != 0.0f)
                _moveDirection = new Vector2(0.0f, y).normalized;
            else if (Mathf.Round(x) != 0.0f)
                _moveDirection = new Vector2(x, 0.0f).normalized;            
        }            
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
