using UnityEngine;

public class BulletMove : MonoBehaviour
{
    private Vector2 direction;
    public float Speed;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        direction = Vector2.up;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(direction * Speed * Time.deltaTime);
    }
}
