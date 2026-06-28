using UnityEngine;
using System.Collections;

public class TargetSpawner : MonoBehaviour
{
    public GameObject targetPrefab;

    public Transform[] spawnPoints;

    public float intervalo = 2f;

    public float minLifeTime = 2f;
    public float maxLifeTime = 5f;

    void Start()
    {
        StartCoroutine(SpawnTargets());
    }

    IEnumerator SpawnTargets()
    {
        while (true)
        {
            Spawn();

            yield return new WaitForSeconds(intervalo);
        }
    }

    void Spawn()
    {
        int randomSpawn = Random.Range(0, spawnPoints.Length);

        GameObject Enemigo = Instantiate(targetPrefab,spawnPoints[randomSpawn].position,Quaternion.identity);

        float randomLife = Random.Range(minLifeTime, maxLifeTime);

        Enemigo.GetComponent<Objetivo>().lifeTime = randomLife;
    }
}
