using UnityEngine;

public class CameraHeadBob : MonoBehaviour
{
    [Header("Head Bob Settings")]
    [SerializeField] private float bobSpeed = 5.0f; // Vitesse du mouvement
    [SerializeField] private float bobAmount = 0.1f; // Amplitude du mouvement
    [SerializeField] private bool isEnabled = true; // Active ou désactive l'effet

    private float defaultYPos; // Position de base en Y de la caméra
    private float timer = 0.0f;

    private void Start()
    {
        // Sauvegarde la position de départ en Y
        defaultYPos = transform.localPosition.y;
    }

    private void Update()
    {
        if (isEnabled)
        {
            ApplyHeadBob();
        }
    }

    private void ApplyHeadBob()
    {
        // Augmente le timer en fonction du temps et de la vitesse
        timer += Time.deltaTime * bobSpeed;

        // Calcule la nouvelle position en Y en utilisant une fonction sinus
        float newY = defaultYPos + Mathf.Sin(timer) * bobAmount;

        // Applique le nouveau positionnement en Y
        transform.localPosition = new Vector3(
            transform.localPosition.x,
            newY,
            transform.localPosition.z
        );
    }

    public void EnableHeadBob(bool enable)
    {
        isEnabled = enable;
        if (!isEnabled)
        {
            // Réinitialise la position si l'effet est désactivé
            transform.localPosition = new Vector3(
                transform.localPosition.x,
                defaultYPos,
                transform.localPosition.z
            );
        }
    }
}
