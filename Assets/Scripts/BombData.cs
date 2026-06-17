using UnityEngine;

[CreateAssetMenu(fileName = "NewBomb", menuName = "KorsanAvi/BombData")]
public class BombData : ScriptableObject
{
    [Header("Bilgi")]
    public string bombName = "Gülle";
    public Sprite icon; // UI ikonu

    [Header("Prefablar")]
    public GameObject cannonballPrefab;
    public GameObject explosionEffectPrefab;

    [Header("Ýstatistikler")]
    public float explosionRadius = 5f;
    public float explosionForce = 700f;
    public float explosionDamage = 100f;
    public float fireForce = 30f;

    [Header("Görsel")]
    public Color uiColor = Color.white; // UI'da renk kodu
}