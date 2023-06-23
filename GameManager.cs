using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("# Game Control")]
    public static GameManager instance;
    public float gameTime;
    public float maxGameTime;
    
    [Header("# Player Info")]
    public int maxHealth;
    public int health;
    public int level;
    public int kill;
    public int exp;
    public int[] nextExp;

    [Header("# Game Object")]
    public PoolManager pool;
    public Player player;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        health = maxHealth;
    }

    void Update()
    {
        gameTime += Time.deltaTime;

        if(gameTime > maxGameTime)
        {
            gameTime = maxGameTime;
        }
    }

    public void GetExp()
    {
        exp++;

        if(exp == nextExp[level])
        {
            exp = 0;
            level++;
        }
    }
}
