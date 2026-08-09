using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemyProfile", menuName = "Wave System/Enemy Profile")]
public class EnemyProfile : ScriptableObject
{
    [Header("Información del enemigo")]
    public string enemyName;

    [Header("Estadísticas")]
    public int health = 100;
    public float speed = 2f;
    public int damage = 10;

    [Header("Prefab")]
    public GameObject enemyPrefab;
}