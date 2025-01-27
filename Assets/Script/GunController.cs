using UnityEngine;

public class GunController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject m_projectilePrefab; // Prefab du projectile
    [SerializeField] private Transform m_shootingSpot;
    [SerializeField] private Camera m_mainCamera;
    [SerializeField] private Transform m_gunPivot; // Le point pivot pour orienter le canon

    [Header("Settings")]
    [SerializeField] private float m_rotationSpeed = 5.0f; // Vitesse de rotation du canon
    [SerializeField] private float m_maxRotationAngle = 45.0f; // Angle maximal de rotation
    [SerializeField] private LayerMask m_rayMask; // Masque de couches pour le raycast

    private GunProjectile m_currentProjectile;
    private bool m_isCharging = false;

    private void Update()
    {
        HandleMouseAim();

        if (Input.GetMouseButtonDown(0))
        {
            OnMouseButtonDown();
        }

        if (Input.GetMouseButtonUp(0))
        {
            OnMouseButtonUp();
        }

        if (m_isCharging && m_currentProjectile)
        {
            float newChargeTime = m_currentProjectile.CurrentChargeTime + Time.deltaTime;
            m_currentProjectile.SetCharge(newChargeTime);
        }
    }

    private void HandleMouseAim()
    {
        Ray ray = m_mainCamera.ScreenPointToRay(Input.mousePosition);

        // Effectuer un raycast en tenant compte du LayerMask
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, m_rayMask))
        {
            Vector3 targetPosition = hit.point;
            Vector3 direction = (targetPosition - m_gunPivot.position).normalized;

            // Calculer la rotation cible
            Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);

            // Appliquer une rotation corrective si nécessaire
            Quaternion correctiveRotation = Quaternion.Euler(0, 180, 0);
            targetRotation *= correctiveRotation;

            // Interpolation fluide vers la rotation cible
            m_gunPivot.rotation = Quaternion.Slerp(
                m_gunPivot.rotation,
                targetRotation,
                Time.deltaTime * m_rotationSpeed
            );

            // Limiter l'angle de rotation
            float angle = Quaternion.Angle(Quaternion.identity, m_gunPivot.localRotation);
            if (angle > m_maxRotationAngle)
            {
                m_gunPivot.localRotation = Quaternion.RotateTowards(
                    Quaternion.identity,
                    m_gunPivot.localRotation,
                    m_maxRotationAngle
                );
            }
        }
    }

    private void OnMouseButtonUp()
    {
        m_isCharging = false;

        if (m_currentProjectile != null)
        {
            // Détacher le projectile avant le tir
            m_currentProjectile.transform.SetParent(null);

            Vector3 targetPoint = GetMouseWorldPosition();
            Vector3 shootDirection = (targetPoint - m_shootingSpot.position).normalized;

            Debug.Log($"Tir vers la direction : {shootDirection}");
            ShootProjectile(m_currentProjectile, shootDirection);

            m_currentProjectile = null; // Réinitialise après le tir
        }
    }

    private void OnMouseButtonDown()
    {
        m_isCharging = true;
        m_currentProjectile = InstantiateProjectile();

        // Attacher le projectile au shooting spot pour qu'il suive le canon
        if (m_currentProjectile != null)
        {
            m_currentProjectile.transform.SetParent(m_shootingSpot);
            m_currentProjectile.transform.localPosition = Vector3.zero;
            m_currentProjectile.transform.localRotation = Quaternion.identity;
        }
    }

    private GunProjectile InstantiateProjectile()
    {
        if (m_projectilePrefab == null)
        {
            Debug.LogError("Prefab de projectile non assigné !");
            return null;
        }

        GameObject projectileInstance = Instantiate(m_projectilePrefab, m_shootingSpot.position, m_shootingSpot.rotation);
        GunProjectile projectileComponent = projectileInstance.GetComponent<GunProjectile>();

        if (projectileComponent == null)
        {
            Debug.LogError("Le prefab de projectile doit contenir un composant 'GunProjectile' !");
            Destroy(projectileInstance);
            return null;
        }

        return projectileComponent;
    }

    private void ShootProjectile(GunProjectile projectile, Vector3 dir)
    {
        projectile.Shoot(dir);
    }

    private Vector3 GetMouseWorldPosition()
    {
        Ray ray = m_mainCamera.ScreenPointToRay(Input.mousePosition);

        // Effectuer un raycast en tenant compte du LayerMask
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, m_rayMask))
        {
            return hit.point;
        }

        return ray.GetPoint(100); // Retourne un point éloigné si rien n'est touché
    }
}
