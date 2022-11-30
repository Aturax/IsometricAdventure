using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.HID;

public class IceBall : MonoBehaviour
{

    [SerializeField] private float speed = 10.0f;
    [SerializeField] private float timer = 5.0f;
    [SerializeField] private float freezerTimer = 3.0f;
    
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0.0f)
        {
            Destroy(gameObject);
        }

        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    public float GetFreezerTimer()
    {
        return freezerTimer;
    }

    public void DestroyBall()
    {
        Destroy(gameObject);
    }
}
