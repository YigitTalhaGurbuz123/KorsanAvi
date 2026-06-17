using UnityEngine;

public class PirateShipAI : MonoBehaviour
{
    public Transform player;

    [Header("Movement")]
    public float moveSpeed = 5f;
    public float rotationSpeed = 2f;

    [Header("Attack Distance")]
    public float stopDistance = 15f;

    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (player == null) return;

        // Oyuncuya yön
        Vector3 direction = player.position - transform.position;
        direction.y = 0f;

        // Dönüþ
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            rotationSpeed * Time.fixedDeltaTime
        );

        // Mesafe
        float distance = direction.magnitude;

        // Hareket
        if (distance > stopDistance)
        {
            rb.velocity = transform.forward * moveSpeed;
        }
        else
        {
            rb.velocity = Vector3.zero;
        }
    }
}