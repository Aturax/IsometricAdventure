using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> _rooms = null;
    [SerializeField] private int _roomIndex = 0;

    public void LoadRoom(int roomNumber)
    {
        _rooms[_roomIndex].SetActive(false);

        _roomIndex = roomNumber;
        _rooms[_roomIndex].SetActive(true);
    }
}
