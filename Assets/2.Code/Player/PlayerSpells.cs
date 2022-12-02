using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XInput;

public class PlayerSpells : MonoBehaviour
{
    // Spells cooldowns
    private const float IceBallCooldown = 3.0f;
    private const float HasteCooldown = 5.0f;

    [Header("Spells")]
    [SerializeField] private IceBall _iceBallPrefab = null;
    [SerializeField] private Transform _iceBallSpawnPoint = null;
    [Space]
    [SerializeField] private bool _isHasted = false;
    [SerializeField] private float _hasteDuration = 5.0f;
    private float _iceBallTimer = 0.0f;
    private float _hasteTimer = 0.0f;

    private InputController _inputController = null;

    private bool _staffEquipped = false;
    private bool _bookEquipped = false;

    public event Action<bool> HastePlayer = ( bool isHasted ) => { };

    void Start()
    {
        _inputController = new InputController();
    }

    void Update()
    {
        CaptureInputSpell();
        UpdateSpellsTimers();
    }

    private void CaptureInputSpell()
    {
        if (_bookEquipped && _inputController.HasteSpell() && !_isHasted && _hasteTimer <= 0.0f)
        {
            _isHasted = true;
            _hasteTimer = _hasteDuration;
            HastePlayer(_isHasted);
        }

        if (_staffEquipped && _inputController.IceBall() && _iceBallTimer <= 0.0f)
        {
            _iceBallTimer = IceBallCooldown;
            IceBall iceBall = Instantiate(_iceBallPrefab, _iceBallSpawnPoint.position, transform.rotation);
        }
    }

    private void UpdateSpellsTimers()
    {
        _iceBallTimer -= Time.deltaTime;
        _hasteTimer -= Time.deltaTime;        

        if (_isHasted && _hasteTimer <= 0.0f)
        {
            _isHasted = false;
            _hasteTimer = HasteCooldown;
            HastePlayer(_isHasted);
        }
    }

    public void EquipStaff()
    {
        _staffEquipped = true;
    }

    public void EquipBook()
    {
        _bookEquipped = true;
    }
}
