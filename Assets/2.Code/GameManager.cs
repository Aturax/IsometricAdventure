using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    [SerializeField] private PlayerController player = null;
    [SerializeField] private RoomManager roomManager = null;
    [SerializeField] private Collectable[] collectables;

    private void OnEnable()
    {
        player.Teleport += Teleport;

        foreach(Collectable collectable in collectables)
        {
            collectable.Pick += PickItem;
        }
    }

    private void OnDisable()
    {
        player.Teleport -= Teleport;

        foreach (Collectable collectable in collectables)
        {
            collectable.Pick -= PickItem;
        }
    }

    private void Teleport(DoorTrigger doorTrigger)
    {
        roomManager.LoadRoom(doorTrigger.GetRoomNumber());

        player.TeleportPlayer(doorTrigger.GetEntrancePosition());
    }

    private void PickItem(Item itemType)
    {
        collectables[(int)itemType].gameObject.SetActive(false);
        player.PickItem(itemType);
        // TODO send order to UI to show the item collected
    }
}
