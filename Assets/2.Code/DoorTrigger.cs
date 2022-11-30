using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    [SerializeField] private Transform entrance = null;
    [SerializeField] private int roomNumber = 0;

    public Vector3 GetEntrancePosition()
    {
        return entrance.position;
    }

    public int GetRoomNumber()
    {
        return roomNumber;
    }
}
