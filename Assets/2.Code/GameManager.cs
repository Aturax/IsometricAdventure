using UnityEngine;

public class GameManager : MonoBehaviour
{

    [SerializeField] private PlayerController _player = null;
    [SerializeField] private RoomManager _roomManager = null;
    [SerializeField] private Collectable[] _collectables;

    private void OnEnable()
    {
        _player.Teleport += Teleport;

        foreach(Collectable collectable in _collectables)
        {
            collectable.Pick += PickItem;
        }
    }

    private void OnDisable()
    {
        _player.Teleport -= Teleport;

        foreach (Collectable collectable in _collectables)
        {
            collectable.Pick -= PickItem;
        }
    }

    private void Teleport(DoorTrigger doorTrigger)
    {
        _roomManager.LoadRoom(doorTrigger.GetRoomNumber());
        _player.TeleportPlayer(doorTrigger.GetEntrancePosition());
    }

    private void PickItem(Item itemType)
    {
        _collectables[(int)itemType].gameObject.SetActive(false);
        _player.PickItem(itemType);
        // TODO send order to UI to show the item collected
    }
}
