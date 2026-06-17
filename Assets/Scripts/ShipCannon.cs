using UnityEngine;

public class CannonFire : MonoBehaviour
{
    [Header("References")]
    public Transform firePoint;

    [Header("Fallback (envanter yoksa)")]
    public GameObject defaultCannonballPrefab;
    public float defaultFireForce = 30f;


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Fire();
        }
    }

    private void Fire()
    {
        // Envanterden seçili bombayı al
        BombData bomb = BombInventory.Instance?.SelectedBomb;

        GameObject prefabToUse;
        float forceToUse;

        if (bomb != null && bomb.cannonballPrefab != null)
        {
            prefabToUse = bomb.cannonballPrefab;
            forceToUse = bomb.fireForce;
        }
        else
        {
            // Fallback
            prefabToUse = defaultCannonballPrefab;
            forceToUse = defaultFireForce;
        }

        if (prefabToUse == null) return;

        GameObject ball = Instantiate(prefabToUse, firePoint.position, firePoint.rotation);

        // BombData'yı topa aktar
        CannonBall cannonBall = ball.GetComponent<CannonBall>();
        if (cannonBall != null && bomb != null)
        {
            cannonBall.explosionRadius = bomb.explosionRadius;
            cannonBall.explosionForce = bomb.explosionForce;
            cannonBall.explosionDamage = bomb.explosionDamage;
            cannonBall.explosionEffectPrefab = bomb.explosionEffectPrefab;
        }

        Rigidbody rb = ball.GetComponent<Rigidbody>();
        if (rb != null)
            rb.AddForce(firePoint.forward * forceToUse, ForceMode.Impulse);

        Debug.Log($"BOOOOM 💣 - {bomb?.bombName ?? "Default"}");
    }
}