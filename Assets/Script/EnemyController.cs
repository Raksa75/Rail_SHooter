using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("Enemy Settings")]
    [SerializeField] private GameObject deathEffect; // Effet visuel ou particules à jouer lors de la destruction

    private void OnTriggerEnter(Collider other)
    {
        // Vérifie si l'objet entrant est un projectile
        if (other.CompareTag("Projectile"))
        {
            // Joue l'effet de mort si défini
            if (deathEffect != null)
            {
                Instantiate(deathEffect, transform.position, Quaternion.identity);
            }

            // Détruit l'ennemi
            Destroy(gameObject);

            // Détruit le projectile
            Destroy(other.gameObject);
        }
    }
}
