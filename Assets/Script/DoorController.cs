using UnityEngine;

public class DoorController : MonoBehaviour
{
    [Header("Door Settings")]
    [SerializeField] private Vector3 openPositionOffset; // Décalage local pour ouvrir la porte
    [SerializeField] private float openSpeed = 3.0f; // Vitesse d'ouverture

    private Vector3 closedPositionLocal; // Position initiale de la porte (locale)
    private Vector3 openPositionLocal; // Position cible de la porte (locale)
    private bool isOpening = false; // Indique si la porte est en train de s'ouvrir

    private void Start()
    {
        // Sauvegarde la position initiale comme position fermée (locale)
        closedPositionLocal = transform.localPosition;

        // Calcule la position cible (locale)
        openPositionLocal = closedPositionLocal + openPositionOffset;
    }

    private void Update()
    {
        if (isOpening)
        {
            // Déplace la porte vers la position ouverte en coordonnées locales
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, openPositionLocal, Time.deltaTime * openSpeed);

            // Vérifie si la porte est arrivée à destination
            if (Vector3.Distance(transform.localPosition, openPositionLocal) < 0.01f)
            {
                isOpening = false; // Arrête le déplacement
                transform.localPosition = openPositionLocal; // Assure une position finale exacte
                Debug.Log($"Porte ouverte à la position locale : {transform.localPosition}");
            }
        }
    }

    public void OpenDoor()
    {
        Debug.Log($"Ouverture de la porte vers la position locale : {openPositionLocal}");
        isOpening = true;
    }

    public void CloseDoor()
    {
        Debug.Log("Fermeture de la porte...");
        isOpening = false;
        transform.localPosition = closedPositionLocal; // Retour à la position initiale
    }
}
