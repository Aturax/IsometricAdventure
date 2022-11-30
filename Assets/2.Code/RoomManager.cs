using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> rooms = null;
    [SerializeField] private int roomIndex = 0;

    public void LoadRoom(int roomNumber)
    {
        rooms[roomIndex].SetActive(false);

        roomIndex = roomNumber;
        rooms[roomIndex].SetActive(true);
    }
}
