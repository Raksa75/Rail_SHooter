using UnityEngine;
using UnityEngine.Splines;
using System.Collections;

public class EnemySpawnerOnSpline : MonoBehaviour
{
    [Header("Spawner Settings")]
    [SerializeField] private SplineContainer splineContainer; // La spline où les ennemis apparaîtront
    [SerializeField] private GameObject enemyPrefab; // Le prefab de l'ennemi
    [SerializeField] private float spawnInterval = 1.0f; // Intervalle de spawn (secondes)
    [SerializeField] private float spawnDuration = 5.0f; // Durée totale du spawn (secondes)
    [SerializeField] private float initialPause = 3.0f; // Pause initiale avant de commencer le spawn

    [Header("Enemy Movement Settings")]
    [SerializeField] private float enemySpeed = 2.0f; // Vitesse de déplacement des ennemis
    [SerializeField] private SplineAnimate.LoopMode loopMode = SplineAnimate.LoopMode.Once; // Mode de boucle

    private Spline spline; // Référence à la spline

    private void Start()
    {
        if (splineContainer == null || enemyPrefab == null)
        {
            Debug.LogError("SplineContainer ou EnemyPrefab non assigné !");
            return;
        }

        spline = splineContainer.Spline;

        // Démarrer avec une pause initiale
        StartCoroutine(StartWithPause());
    }

    private IEnumerator StartWithPause()
    {
        Debug.Log($"Pause initiale de {initialPause} secondes...");
        yield return new WaitForSeconds(initialPause);

        // Démarrer le spawn après la pause
        StartCoroutine(SpawnEnemies());
    }

    private IEnumerator SpawnEnemies()
    {
        float elapsedTime = 0.0f;

        Debug.Log("Début du spawn des ennemis !");
        while (elapsedTime < spawnDuration)
        {
            SpawnEnemy();
            elapsedTime += spawnInterval;
            yield return new WaitForSeconds(spawnInterval);
        }

        Debug.Log("Fin du spawn des ennemis.");
    }

    private void SpawnEnemy()
    {
        // Initialisation du point de départ
        float startProgress = Random.Range(0f, 1f);

        // Évaluer les positions et tangentes locales
        Vector3 localPosition = spline.EvaluatePosition(startProgress);
        Vector3 localTangent = spline.EvaluateTangent(startProgress);

        // Convertir en coordonnées mondiales
        Vector3 worldPosition = splineContainer.transform.TransformPoint(localPosition);
        Vector3 worldTangent = splineContainer.transform.TransformDirection(localTangent);

        // Instancier l'ennemi
        GameObject enemyInstance = Instantiate(
            enemyPrefab,
            worldPosition,
            Quaternion.LookRotation(worldTangent, Vector3.up)
        );

        // Ajouter le composant SplineAnimate pour animer l'ennemi
        var splineAnimator = enemyInstance.AddComponent<SplineAnimate>();
        splineAnimator.Container = splineContainer;
        splineAnimator.Loop = loopMode;
        splineAnimator.AnimationMethod = SplineAnimate.Method.Speed; // Mode de vitesse
        splineAnimator.MaxSpeed = enemySpeed; // Appliquer la vitesse configurée

        // Activer l'animation
        splineAnimator.Play();

        Debug.Log($"Ennemi spawné à : {worldPosition} avec une vitesse de {enemySpeed}");
    }
}
