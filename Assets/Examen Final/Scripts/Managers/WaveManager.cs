using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WaveManager : MonoBehaviour
{
    [Header("Configuración de oleadas")]
    [SerializeField] private WaveConfig[] waves;

    [Header("Spawn")]
    [SerializeField] private Transform[] spawnPoints;

    [Header("Objetivo")]
    [SerializeField] private Transform enemyTarget;

    [Header("Estado")]
    [SerializeField] private GameState currentState = GameState.Waiting;

    [Header("Información")]
    [SerializeField] private int currentWaveIndex = 0;
    [SerializeField] private int activeEnemies = 0;

    private bool gameFinished = false;

    public GameState CurrentState => currentState;
    public int CurrentWave => Mathf.Min(currentWaveIndex + 1, waves.Length);
    public int TotalWaves => waves.Length;
    public int ActiveEnemies => activeEnemies;

    private void Start()
    {
        if (!ValidateConfiguration())
        {
            currentState = GameState.Lose;
            return;
        }

        StartCoroutine(WaveSystem());
    }

    private IEnumerator WaveSystem()
    {
        while (currentWaveIndex < waves.Length && !gameFinished)
        {
            currentState = GameState.Spawning;

            WaveConfig currentWave = waves[currentWaveIndex];

            Debug.Log($"Comenzando {currentWave.waveName}");

            yield return new WaitForSeconds(
                currentWave.timeBeforeWave
            );

            if (gameFinished)
                yield break;

            yield return StartCoroutine(
                SpawnWave(currentWave)
            );

            if (gameFinished)
                yield break;

            currentState = GameState.Playing;

            yield return new WaitUntil(
                () => activeEnemies <= 0 || gameFinished
            );

            if (gameFinished)
                yield break;

            Debug.Log(
                $"{currentWave.waveName} completada."
            );

            currentWaveIndex++;

            if (currentWaveIndex < waves.Length)
            {
                currentState = GameState.Waiting;

                yield return new WaitForSeconds(
                    currentWave.timeAfterWave
                );
            }
        }

        if (!gameFinished)
        {
            WinGame();
        }
    }

    private IEnumerator SpawnWave(WaveConfig wave)
    {
        foreach (WaveConfig.EnemySpawnData spawnData in wave.enemies)
        {
            if (spawnData.enemyProfile == null)
            {
                Debug.LogError(
                    "Existe un EnemyProfile vacío en la oleada."
                );

                continue;
            }

            for (int i = 0; i < spawnData.amount; i++)
            {
                if (gameFinished)
                    yield break;

                SpawnEnemy(
                    spawnData.enemyProfile,
                    wave
                );

                yield return new WaitForSeconds(
                    spawnData.spawnDelay
                );
            }
        }
    }

    private void SpawnEnemy(EnemyProfile profile, WaveConfig wave)
    {
        if (profile == null)
        {
            Debug.LogError(
                "El EnemyProfile es nulo."
            );

            return;
        }

        if (profile.enemyPrefab == null)
        {
            Debug.LogError(
                $"El perfil {profile.enemyName} " +
                "no tiene un prefab asignado."
            );

            return;
        }

        if (spawnPoints == null ||
            spawnPoints.Length == 0)
        {
            Debug.LogError(
                "No existen Spawn Points."
            );

            return;
        }

        Transform spawnPoint =
            spawnPoints[
                Random.Range(
                    0,
                    spawnPoints.Length
                )
            ];

        GameObject enemyObject =
            Instantiate(
                profile.enemyPrefab,
                spawnPoint.position,
                spawnPoint.rotation
            );

        Enemy enemy =
            enemyObject.GetComponent<Enemy>();

        if (enemy == null)
        {
            Debug.LogError(
                "El prefab no contiene Enemy."
            );

            Destroy(enemyObject);
            return;
        }

        float healthMultiplier = 1f;
        float speedMultiplier = 1f;

        switch (wave.waveType)
        {
            case WaveType.Normal:
                healthMultiplier = 1f;
                speedMultiplier = 1f;
                break;

            case WaveType.Elite:
                healthMultiplier = wave.eliteHealthMultiplier;
                speedMultiplier = 1f;
                break;

            case WaveType.Boss:
                healthMultiplier = wave.bossHealthMultiplier;
                speedMultiplier = wave.bossSpeedMultiplier;
                break;
        }

        enemy.Initialize(
            profile,
            enemyTarget,
            healthMultiplier,
            speedMultiplier
        );

        enemy.OnEnemyDied += HandleEnemyDied;
        enemy.OnEnemyReachedTarget +=
            HandleEnemyReachedTarget;

        activeEnemies++;

        Debug.Log(
            $"Enemigo generado. " +
            $"Enemigos activos: {activeEnemies}"
        );
    }

    private void HandleEnemyDied(Enemy enemy)
    {
        if (gameFinished)
            return;

        activeEnemies--;

        activeEnemies =
            Mathf.Max(
                0,
                activeEnemies
            );

        enemy.OnEnemyDied -=
            HandleEnemyDied;

        enemy.OnEnemyReachedTarget -=
            HandleEnemyReachedTarget;

        Debug.Log(
            $"Enemigos restantes: {activeEnemies}"
        );
    }

    private void HandleEnemyReachedTarget(
        Enemy enemy)
    {
        if (gameFinished)
            return;

        LoseGame();

        if (enemy != null)
        {
            enemy.OnEnemyDied -=
                HandleEnemyDied;

            enemy.OnEnemyReachedTarget -=
                HandleEnemyReachedTarget;
        }
    }

    private void WinGame()
    {
        if (gameFinished)
            return;

        gameFinished = true;
        currentState = GameState.Win;

        Debug.Log(
            "¡VICTORIA! Todas las oleadas fueron completadas."
        );
    }

    private void LoseGame()
    {
        if (gameFinished)
            return;

        gameFinished = true;
        currentState = GameState.Lose;

        Debug.Log(
            "¡DERROTA! Un enemigo llegó al objetivo."
        );

        StopAllCoroutines();
    }

    private bool ValidateConfiguration()
    {
        if (waves == null ||
            waves.Length == 0)
        {
            Debug.LogError(
                "WaveManager: No hay oleadas configuradas."
            );

            return false;
        }

        if (spawnPoints == null ||
            spawnPoints.Length == 0)
        {
            Debug.LogError(
                "WaveManager: No hay Spawn Points."
            );

            return false;
        }

        if (enemyTarget == null)
        {
            Debug.LogError(
                "WaveManager: No existe Enemy Target."
            );

            return false;
        }

        return true;
    }

    public void RestartGame()
    {
        Scene currentScene =
            SceneManager.GetActiveScene();

        SceneManager.LoadScene(
            currentScene.buildIndex
        );
    }
}