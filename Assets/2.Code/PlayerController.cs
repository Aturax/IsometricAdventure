using System;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(PlayerSpells))]
public class PlayerController : MonoBehaviour
{
    [Header("Equipment")]
    [SerializeField] private GameObject _hat = null;
    [SerializeField] private GameObject _staff = null;
    [SerializeField] private GameObject _book = null;

    private PlayerMovement _playerMovement = null;
    private PlayerSpells _playerSpells = null;

    public event Action<DoorTrigger> Teleport = (DoorTrigger doorTrigger) => { };

    private void Awake()
    {
        _playerMovement = GetComponent<PlayerMovement>();
        _playerSpells = GetComponent<PlayerSpells>();
    }

    private void OnEnable()
    {
        _playerSpells.HastePlayer += HastePlayer;
    }

    private void OnDisable()
    {
        _playerSpells.HastePlayer -= HastePlayer;
    }

    private void HastePlayer(bool isHasted)
    {
        _playerMovement.HastePlayer(isHasted);
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }    

    public void PickItem(Item itemPicked)
    {
        if (itemPicked == Item.Hat)
        {
            _hat.SetActive(true);
            _playerMovement.EquipHat();
        }
        else if (itemPicked == Item.Staff)
        {
            _staff.SetActive(true);
            _playerSpells.EquipStaff();
        }
        else
        {
            _book.SetActive(true);
            _playerSpells.EquipBook();
        }
    }

    public void TeleportPlayer(Vector3 position)
    {
        transform.position = position;        
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("Teleport"))
            Teleport(hit.gameObject.GetComponent<DoorTrigger>());        
    }
}
