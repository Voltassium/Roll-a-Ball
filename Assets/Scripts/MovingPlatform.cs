using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public float speed = 2f;
    public float moveDistance = 5f;
    
    private Vector3 startPosition;
    private float currentOffset;
    private int direction = 1;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        currentOffset += direction * speed * Time.deltaTime;
        
        if (Mathf.Abs(currentOffset) >= moveDistance)
        {
            currentOffset = moveDistance * Mathf.Sign(currentOffset);
            direction *= -1;
        }

        transform.position = startPosition + transform.right * currentOffset;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(transform);
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(null);
        }
    }
}