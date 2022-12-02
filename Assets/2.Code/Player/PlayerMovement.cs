using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    private const float JumpGravityModifier = -2.0f;

    private CharacterController _characterController = null;
    private InputController _inputController = null;

    [Header("Physics")]
    [SerializeField] private float _movementSpeed = 5.0f;
    [SerializeField] private float _gravity = -9.81f;    
    [SerializeField] private float _jumpHeight = 1f;
    private Vector3 _playerVelocity = Vector3.zero;
    private bool _jumping = false;

    [Header("Spells Modifiers")]
    [SerializeField] private float _hatGravityModifier = 1.0f; // 2.0f when hat equipped
    [SerializeField] private float _speedModifier = 1.0f; // 2.0f when hasted    

    [Header("Animation")]
    [SerializeField] private Animator _animator = null;
    private int _animationBlend = 0;

    private bool _hatEquipped = false;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
    }

    void Start()
    {
        _inputController = new InputController();
        _animationBlend = Animator.StringToHash("Blend");
    }
    
    void Update()
    {
        UpdatePlayerDirection();
        CalculateGravity(_gravity / _hatGravityModifier);
        Jump(_gravity / _hatGravityModifier);
        MovePlayer();
        RotateTowardsPlayerDirection();
    }

    private void UpdatePlayerDirection()
    {
        if (_characterController.isGrounded || _hatEquipped)
        {
            Vector2 input = _inputController.Move().normalized;

            // This is for continuing with the direction while jumping with the hat
            if (input == Vector2.zero)
            {
                _animator.SetFloat(_animationBlend, 0.0f);
                if (_hatEquipped && _jumping) return;
            }
            else
            {
                _animator.SetFloat(_animationBlend, _speedModifier);
            }

            _playerVelocity = new Vector3(input.y, _playerVelocity.y, -input.x);
        }
    }

    private void CalculateGravity(float gravityValue)
    {
        if (!_characterController.isGrounded)
        {
            _playerVelocity.y += gravityValue * Time.deltaTime;
        }
        else
        {
            _jumping = false;
            _playerVelocity.y = -2.0f;
        }
    }

    private void Jump(float gravityValue)
    {
        if (_characterController.isGrounded && _inputController.Jump() > 0.0f)
        {
            _jumping = true;
            _playerVelocity.y = Mathf.Sqrt(_jumpHeight * JumpGravityModifier * gravityValue);
        }
    }

    private void MovePlayer()
    {
        float speed = _movementSpeed;
        
        if (!_jumping)
            speed *= _speedModifier;        

        _characterController.Move(new Vector3(_playerVelocity.x * speed, _playerVelocity.y, _playerVelocity.z * speed) * Time.deltaTime);
    }

    private void RotateTowardsPlayerDirection()
    {
        if (_playerVelocity.x == 0.0f && _playerVelocity.z == 0.0f) return;

        Vector3 lootAt = new Vector3(_playerVelocity.x, 0.0f, _playerVelocity.z);
        transform.rotation = Quaternion.LookRotation(lootAt, Vector3.up);
    }

    public void HastePlayer(bool isHasted)
    {   
        if (isHasted)
        {
            _speedModifier = 2.0f;
            return;
        }
        _speedModifier = 1.0f;
    }

    public void EquipHat()
    {
        _hatEquipped = true;
        _hatGravityModifier = 2.0f;
    }
}
