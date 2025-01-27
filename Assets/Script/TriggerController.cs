using UnityEngine;

public class TriggerController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private DoorController door; // Référence à la porte à ouvrir

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Ouvre la porte lorsque le joueur entre dans le trigger
            door.OpenDoor();
        }
    }
}
