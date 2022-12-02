using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingGround : MonoBehaviour
{
    [SerializeField] private float _gravity = -9.81f;
    [SerializeField] private float _fallingDelay = 0.5f;
    [SerializeField] private float _fallingTimer = 0.0f;
    private bool touched = false;

    private void Start()
    {
        _fallingTimer = _fallingDelay;
    }

    void Update()
    {
        if (touched) _fallingTimer -= Time.deltaTime;
        
        if (touched && _fallingTimer <=0.0f)
        {
            transform.Translate(Vector3.up * _gravity * Time.deltaTime);
            return;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
            touched = true;        
    }
}
