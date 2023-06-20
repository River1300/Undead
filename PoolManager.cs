using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public GameObject[] prefabs;
    public List<GameObject>[] pools;

    void Awake()
    {
        pools = new List<GameObject>[prefabs.Length];
        for(int i = 0; i < prefabs.Length; i++)
        {
            pools[i] = new List<GameObject>();
        }
    }

    public GameObject Get(int index)
    {
        GameObject select = null;

        foreach(GameObject monster in pools[index])
        {
            if(!monster.activeSelf)
            {
                select = monster;
                select.SetActive(true);
                break;
            }
        }

        if(!select)
        {
            select = Instantiate(prefabs[index], transform);
            pools[index].Add(select);
            select.SetActive(true);
        }

        return select;
    }
}
