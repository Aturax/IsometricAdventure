using UnityEngine;

public class IceBall : MonoBehaviour
{

    [SerializeField] private float _speed = 10.0f;
    [SerializeField] private float _timer = 5.0f;
    [SerializeField] private float _freezerTimer = 3.0f;
    
    void Update()
    {
        _timer -= Time.deltaTime;
        if (_timer <= 0.0f)
            Destroy(gameObject);        

        transform.Translate(Vector3.forward * _speed * Time.deltaTime);
    }

    public float GetFreezerTimer()
    {
        return _freezerTimer;
    }

    public void DestroyBall()
    {
        Destroy(gameObject);
    }
}
