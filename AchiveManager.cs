using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchiveManager : MonoBehaviour
{
    enum Achive { PupleMan, OrangeWoman }
    Achive[] achives;
    public GameObject[] lockCharacter;
    public GameObject[] unlockCharacter;
    public GameObject uiNotice;

    WaitForSecondsRealtime wait;

    void Awake()
    {
        achives = (Achive[])Enum.GetValues(typeof(Achive));
        wait = new WaitForSecondsRealtime(5);

        if(!PlayerPrefs.HasKey("MyData")) Init();
    }

    void Start()
    {
        UnlockCharacter();
    }

    void LateUpdate()
    {
        foreach(Achive achive in achives)
        {
            CheckAchive(achive);
        }
    }

    void Init()
    {
        PlayerPrefs.SetInt("MyData", 1);

        foreach(Achive achive in achives)
        {
            PlayerPrefs.SetInt(achive.ToString(), 0);
        }
    }

    void UnlockCharacter()
    {
        for(int i = 0; i < lockCharacter.Length; i++)
        {
            string achiveName = achives[i].ToString();
            bool isUnlock = PlayerPrefs.GetInt(achiveName) == 1;

            lockCharacter[i].SetActive(!isUnlock);
            unlockCharacter[i].SetActive(isUnlock);
        }
    }

    void CheckAchive(Achive achive)
    {
        bool isAchive = false;

        switch(achive)
        {
            case Achive.PupleMan:
                isAchive = GameManager.instance.kill >= 10;
                break;
            case Achive.OrangeWoman:
                isAchive = GameManager.instance.gameTime == GameManager.instance.maxGameTime;
                break;
        }

        if(isAchive && PlayerPrefs.GetInt(achive.ToString()) == 0)
        {
            PlayerPrefs.SetInt(achive.ToString(), 1);
            for(int i = 0; i < uiNotice.transform.childCount; i++)
            {
                bool isActive = i == (int)achive;
                uiNotice.transform.GetChild(i).gameObject.SetActive(isActive);
            }
            StartCoroutine(NoticeRoutine());
        }
    }

    IEnumerator NoticeRoutine()
    {
        uiNotice.SetActive(true);

        yield return wait;

        uiNotice.SetActive(false);
    }
}
