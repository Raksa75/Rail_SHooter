using UnityEngine;

public class GunController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GunProjectile m_projectilePrefab;
    [SerializeField] private Transform m_shootingSpot;

    private GunProjectile m_currentProjectile;
    private bool m_isCharging = false;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Clic gauche détecté !");
            OnMouseButtonDown();
        }

        if (Input.GetMouseButtonUp(0))
        {
            Debug.Log("Clic gauche relâché !");
            OnMouseButtonUp();
        }

        if (m_isCharging && m_currentProjectile)
        {
            float newChargeTime = m_currentProjectile.CurrentChargeTime + Time.deltaTime;
            m_currentProjectile.SetCharge(newChargeTime);
        }
    }

    private void OnMouseButtonUp()
    {
        m_isCharging = false;
        if (m_currentProjectile)
        {
            Debug.Log("Tir du projectile !");
            ShootProjectile(m_currentProjectile, m_shootingSpot.forward);
            m_currentProjectile = null; // Réinitialise après le tir
        }
    }

    private void OnMouseButtonDown()
    {
        m_isCharging = true;
        m_currentProjectile = InstantiateProjectile();
    }

    private GunProjectile InstantiateProjectile()
    {
        Debug.Log("Instanciation du projectile...");
        GunProjectile projectile = Instantiate(m_projectilePrefab, m_shootingSpot.position, m_shootingSpot.rotation);
        return projectile;
    }

    private void ShootProjectile(GunProjectile projectile, Vector3 dir)
    {
        Debug.Log($"Tir dans la direction : {dir}");
        projectile.Shoot(dir);
    }
}
