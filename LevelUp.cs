using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUp : MonoBehaviour
{
    Item[] items;

    RectTransform rect;

    void Awake()
    {
        items = GetComponentsInChildren<Item>(true);
        rect = GetComponent<RectTransform>();
    }

    public void Select(int index)
    {
        items[index].OnClick();
    }

    void Next()
    {
        int[] ran = new int[3];

        foreach(Item item in items)
        {
            item.gameObject.SetActive(false);
        }

        while(true)
        {
            ran[0] = Random.Range(0, items.Length);
            ran[1] = Random.Range(0, items.Length);
            ran[2] = Random.Range(0, items.Length);

            if(ran[0] != ran[1] && ran[0] != ran[2] && ran[1] != ran[2]) break;
        }

        for(int i = 0; i < ran.Length; i++)
        {
            Item ranItem = items[ran[i]];

            if(ranItem.level == ranItem.data.damages.Length)
            {
                items[Random.Range(4, items.Length)].gameObject.SetActive(true);
            }
            else
            {
                ranItem.gameObject.SetActive(true);
            }
        }
    }

    public void Show()
    {
        rect.localScale = Vector3.one;
        GameManager.instance.Stop();
    }

    public void Hide()
    {
        rect.localScale = Vector3.zero;
        GameManager.instance.Resume();
    }
}
