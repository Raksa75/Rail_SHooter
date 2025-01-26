using UnityEngine;

public class GunProjectile : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody m_rb;

    [Header("Values")]
    [SerializeField] private float m_baseSpeed = 5.0f; // Vitesse de base de la balle
    [SerializeField] private Vector2 m_speedMultiplier = new Vector2(1, 10); // Ajustez les valeurs min/max
    [SerializeField] private Vector2 m_sizeMultiplier = new Vector2(1, 2); // Ajustez les valeurs min/max
    [SerializeField] private float m_chargeDuration = 1.0f;

    [Header("Lifetime")]
    [SerializeField] private float m_lifetime = 5.0f; // Temps avant destruction automatique de la balle

    private float m_currentChargeTime = 0.0f;

    public float CurrentChargeTime => m_currentChargeTime;

    private void Start()
    {
        // Détruire la balle après m_lifetime secondes
        Destroy(gameObject, m_lifetime);
    }

    public void SetCharge(float chargeTime)
    {
        m_currentChargeTime = Mathf.Clamp(chargeTime, 0, m_chargeDuration);
        float ratio = m_currentChargeTime / m_chargeDuration;

        // Ajuste la taille en fonction du ratio
        float newSize = Mathf.Lerp(m_sizeMultiplier.x, m_sizeMultiplier.y, ratio);
        transform.localScale = Vector3.one * newSize;

        Debug.Log($"Projectile chargé : Taille = {newSize}");
    }

    public void Shoot(Vector3 dir)
    {
        float ratio = m_currentChargeTime / m_chargeDuration;

        // Calcule la vitesse finale en combinant la vitesse de base et le multiplicateur
        float finalSpeed = m_baseSpeed + Mathf.Lerp(m_speedMultiplier.x, m_speedMultiplier.y, ratio);
        Debug.Log($"Projectile tiré avec vitesse : {finalSpeed}");

        // Applique la force
        m_rb.velocity = Vector3.zero; // Réinitialise la vitesse avant d'appliquer une force
        m_rb.AddForce(finalSpeed * dir, ForceMode.Impulse);

        // Réinitialise le temps de charge après le tir
        m_currentChargeTime = 0.0f;
    }
}
