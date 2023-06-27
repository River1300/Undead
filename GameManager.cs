using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("# Game Control")]
    public static GameManager instance;
    public float gameTime;
    public float maxGameTime;
    public bool isLive;
    
    [Header("# Player Info")]
    public int playerId;
    public float maxHealth;
    public float health;
    public int level;
    public int kill;
    public int exp;
    public int[] nextExp;

    [Header("# Game Object")]
    public PoolManager pool;
    public Player player;
    public LevelUp uiLevelUp;
    public Result uiResult;
    public GameObject enemyCleaner;

    void Awake()
    {
        instance = this;
    }

    public void GameStart(int id)
    {
        playerId = id;
        player.gameObject.SetActive(true);

        health = maxHealth;
        uiLevelUp.Select(playerId % 2);
        isLive = true;
        Resume();

        AudioManager.instance.PlaySfx(AudioManager.Sfx.Select);
        AudioManager.instance.PlayBgm(isLive);
    }

    public void GameRetry()
    {
        SceneManager.LoadScene(0);
    }

    public void GameVictory()
    {
        StartCoroutine(GameVictoryRoutine());
    }

    IEnumerator GameVictoryRoutine()
    {
        isLive = false;
        enemyCleaner.SetActive(true);

        yield return new WaitForSeconds(0.5f);
        
        uiResult.gameObject.SetActive(true);
        uiResult.Win();
        Stop();

        AudioManager.instance.PlaySfx(AudioManager.Sfx.Win);
        AudioManager.instance.PlayBgm(isLive);
    }

    public void GameOver()
    {
        StartCoroutine(GameOverRoutine());
    }

    IEnumerator GameOverRoutine()
    {
        isLive = false;

        yield return new WaitForSeconds(0.5f);

        uiResult.gameObject.SetActive(true);
        uiResult.Lose();
        Stop();

        AudioManager.instance.PlaySfx(AudioManager.Sfx.Lose);
        AudioManager.instance.PlayBgm(isLive);
    }

    void Update()
    {
        if(!isLive) return;

        gameTime += Time.deltaTime;

        if(gameTime > maxGameTime)
        {
            gameTime = maxGameTime;
            GameVictory();
        }
    }

    public void GetExp()
    {
        if(!isLive) return;

        exp++;

        if(exp == nextExp[Mathf.Min(level, nextExp.Length - 1)])
        {
            exp = 0;
            level++;
            uiLevelUp.Show();
        }
    }

    public void Stop()
    {
        isLive = false;
        Time.timeScale = 0;
    }

    public void Resume()
    {
        isLive = true;
        Time.timeScale = 1;
    }
}
