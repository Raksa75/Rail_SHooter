using UnityEngine;
using UnityEngine.Splines;

public class EnemySplineFollower : MonoBehaviour
{
    private Spline spline; // La spline à suivre
    private float progress; // Progression actuelle sur la spline (0 à 1)
    private float speed; // Vitesse de déplacement
    private bool loop; // Boucler sur la spline

    public void Initialize(Spline spline, float startProgress, float speed, bool loop)
    {
        this.spline = spline;
        this.progress = startProgress;
        this.speed = speed;
        this.loop = loop;
    }

    private void Update()
    {
        if (spline == null) return;

        // Calculer la progression
        progress += speed * Time.deltaTime / spline.GetLength();

        // Gestion du bouclage ou de l'arrêt à la fin
        if (progress > 1.0f)
        {
            if (loop)
                progress -= 1.0f;
            else
                progress = 1.0f;
        }

        // Évaluer la spline à la progression actuelle
        SplineUtility.Evaluate(spline, progress, out var position, out var tangent, out var up);

        // Mettre à jour la position et la rotation
        transform.position = position;
        transform.rotation = Quaternion.LookRotation(tangent, up);
    }
}
