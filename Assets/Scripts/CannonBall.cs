using UnityEngine;



public class CannonBall : MonoBehaviour
{
    [Header("Explosion Settings")]
    public float explosionRadius = 5f;
    public float explosionForce = 700f;
    public float explosionDamage = 100f;
    public GameObject explosionEffectPrefab;

    [SerializeField] private AudioClip soundClip; // bunu tut

    //  audioSource ve Start() kaldýrýldý

    void OnCollisionEnter(Collision collision)
    {
        AudioSource.PlayClipAtPoint(soundClip, transform.position); // 
        Explode();
    }

    void Explode()
    {
        if (explosionEffectPrefab != null)
        {
            GameObject effect = Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);
            Destroy(effect, 1f);
        }

        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider nearby in colliders)
        {
            Rigidbody rb = nearby.GetComponent<Rigidbody>();
            if (rb != null)
                rb.AddExplosionForce(explosionForce, transform.position, explosionRadius);
        }

        Debug.Log("?? PATLADI!");
        Destroy(gameObject);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}