using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingGround : MonoBehaviour
{
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float fallingDelay = 0.5f;
    [SerializeField] private float fallingTimer = 0.0f;
    private bool touched = false;

    private void Start()
    {
        fallingTimer = fallingDelay;
    }

    void Update()
    {
        if (touched) fallingTimer -= Time.deltaTime;
        
        if (touched && fallingTimer <=0.0f)
        {
            transform.Translate(Vector3.up * gravity * Time.deltaTime);
            return;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            touched = true;
        }
    }
}
