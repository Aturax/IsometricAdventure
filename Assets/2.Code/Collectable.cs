using System;
using UnityEngine;

public enum Item
{
    Hat,
    Staff,
    Book
}

public class Collectable : MonoBehaviour
{
    [SerializeField] private Item _itemType = Item.Hat;

    public event Action<Item> Pick = (Item item) => { };

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
            Pick(_itemType);        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            Pick(_itemType);        
    }
}
