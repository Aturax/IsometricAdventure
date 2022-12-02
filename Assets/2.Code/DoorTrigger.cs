using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    [SerializeField] private Transform _entrance = null;
    [SerializeField] private int _roomNumber = 0;

    public Vector3 GetEntrancePosition()
    {
        return _entrance.position;
    }

    public int GetRoomNumber()
    {
        return _roomNumber;
    }
}
