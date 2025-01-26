using UnityEngine;
using UnityEngine.Splines;
using System.Collections;

public class SplineFollowerWithStops : MonoBehaviour
{
    [Header("Spline Settings")]
    [SerializeField] private SplineContainer splineContainer; // Spline à suivre
    [SerializeField] private float speed = 5.0f; // Vitesse de déplacement
    [SerializeField] private bool loop = false; // Si le déplacement doit boucler

    [Header("Stop Settings")]
    [SerializeField] private StopPoint[] stopPoints; // Liste des points d'arrêt

    [Header("Rotation Correction")]
    [SerializeField] private Vector3 rotationOffset = Vector3.zero; // Correction de la rotation

    private Spline spline; // Référence à la spline
    private float progress = 0.0f; // Position actuelle sur la spline (de 0 à 1)
    private bool isStopped = false; // Indique si l'objet est en train de s'arrêter

    private void Start()
    {
        if (splineContainer == null)
        {
            Debug.LogError("SplineContainer non assigné !");
            return;
        }

        spline = splineContainer.Spline;
    }

    private void Update()
    {
        if (spline == null || isStopped) return;

        // Avance le long de la spline
        progress += speed * Time.deltaTime / spline.GetLength();

        // Gérer la boucle ou stopper à la fin
        if (progress > 1.0f)
        {
            if (loop)
            {
                progress -= 1.0f;
            }
            else
            {
                progress = 1.0f;
                enabled = false; // Arrête le script une fois la spline parcourue
                return;
            }
        }

        // Évaluer la position et la rotation sur la spline
        Vector3 localPosition = spline.EvaluatePosition(progress);
        Vector3 tangent = spline.EvaluateTangent(progress);
        Vector3 up = Vector3.up; // Direction par défaut

        // Convertir la position locale en position globale
        Vector3 globalPosition = splineContainer.transform.TransformPoint(localPosition);

        // Appliquer la position globale
        transform.position = globalPosition;

        // Convertir la tangente locale en direction globale
        Vector3 globalTangent = splineContainer.transform.TransformDirection(tangent);
        Quaternion splineRotation = Quaternion.LookRotation(globalTangent, up);

        // Appliquer la rotation avec correction
        Quaternion correctiveRotation = Quaternion.Euler(rotationOffset);
        transform.rotation = splineRotation * correctiveRotation;

        // Vérifier les arrêts
        CheckStopPoints();
    }

    private void CheckStopPoints()
    {
        foreach (var stop in stopPoints)
        {
            if (Mathf.Abs(progress - stop.progress) < 0.01f) // Si proche du point d'arrêt
            {
                StartCoroutine(StopAtPoint(stop.duration));
                break;
            }
        }
    }

    private IEnumerator StopAtPoint(float duration)
    {
        isStopped = true;
        yield return new WaitForSeconds(duration);
        isStopped = false;
    }
}

[System.Serializable]
public class StopPoint
{
    [Tooltip("Position sur la spline (entre 0 et 1)")]
    public float progress;

    [Tooltip("Durée d'arrêt en secondes")]
    public float duration;
}
