using UnityEngine;
using UnityEngine.Splines;

public class SplineFollower : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private SplineContainer splineContainer; // Contient la spline à suivre
    [SerializeField] private Transform character; // Personnage à déplacer

    [Header("Settings")]
    [SerializeField] private float speed = 5.0f; // Vitesse du déplacement
    [SerializeField] private bool loop = true; // Boucler sur la spline

    private Spline spline; // La spline à suivre
    private float progress = 0.0f; // Position actuelle sur la spline (0 à 1)

    private void Start()
    {
        if (splineContainer == null)
        {
            Debug.LogError("SplineContainer n'est pas assigné !");
            return;
        }

        spline = splineContainer.Spline;
    }

    private void Update()
    {
        if (spline == null || character == null) return;

        // Calculer la progression en fonction de la vitesse
        progress += speed * Time.deltaTime / spline.GetLength();

        // Boucle ou arrêt à la fin
        if (progress > 1.0f)
        {
            if (loop)
                progress -= 1.0f;
            else
                progress = 1.0f;
        }

        // Déplacer le personnage à la position sur la spline
        SplineUtility.Evaluate(spline, progress, out var position, out var tangent, out var up);

        character.position = position; // Met à jour la position
        character.rotation = Quaternion.LookRotation(tangent, up); // Oriente le personnage
    }
}
