using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Item
{
    Hat,
    Staff,
    Book
}

public class Collectable : MonoBehaviour
{
    [SerializeField] private Item itemType = Item.Hat;

    public event Action<Item> Pick = (Item item) => { };

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Pick(itemType);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Pick(itemType);
        }
    }
}
