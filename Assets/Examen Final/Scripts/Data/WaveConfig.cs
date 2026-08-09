using UnityEngine;

public enum WaveType
{
    Normal,
    Elite,
    Boss
}

[CreateAssetMenu(fileName = "NewWaveConfig", menuName = "Wave System/Wave Config")]
public class WaveConfig : ScriptableObject
{
    [System.Serializable]
    public class EnemySpawnData
    {
        public EnemyProfile enemyProfile;
        public int amount = 5;
        public float spawnDelay = 1f;
    }

    [Header("Información")]
    public string waveName;
    public WaveType waveType;

    [Header("Enemigos")]
    public EnemySpawnData[] enemies;

    [Header("Tiempo")]
    public float timeBeforeWave = 3f;
    public float timeAfterWave = 5f;

    [Header("Configuración Elite")]
    public float eliteHealthMultiplier = 1.5f;

    [Header("Configuración Boss")]
    public float bossHealthMultiplier = 3f;
    public float bossSpeedMultiplier = 1.25f;
}