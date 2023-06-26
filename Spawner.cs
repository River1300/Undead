using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public SpawnData[] spawnData;
    public Transform[] spawnPoints;

    int level;
    float timer;

    void Awake()
    {
        spawnPoints = GetComponentsInChildren<Transform>();
    }

    void Update()
    {
        if(!GameManager.instance.isLive) return;

        timer += Time.deltaTime;
        level = Mathf.Min(Mathf.FloorToInt
            (GameManager.instance.gameTime / 10f), spawnData.Length - 1);

        if(timer > spawnData[level].spawnTime)
        {
            timer = 0;
            Spawn();
        }
    }

    void Spawn()
    {
        GameObject enemy = GameManager.instance.pool.Get(0);

        level = level == 0 ? Random.Range(0, 1) : Random.Range(0, level + 1);
        
        enemy.GetComponent<Enemy>().Init(spawnData[level]);
        enemy.transform.position = spawnPoints[Random.Range(1, spawnPoints.Length)].position;
    }
}

[System.Serializable]
public class SpawnData
{
    public int spriteType;
    public int health;
    public float spawnTime;
    public float speed;
}