using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    public bool isLeft;

    public SpriteRenderer spriter;
    SpriteRenderer player;

    Vector3 rightPos = new Vector3(0.2f, -0.25f, 0);
    Vector3 rightPosRevers = new Vector3(0f, -0.25f, 0);
    Quaternion leftRot = Quaternion.Euler(0, 0, -25);
    Quaternion leftRotRevers = Quaternion.Euler(0, 0, -150);

    void Awake()
    {
        player = GetComponentsInParent<SpriteRenderer>()[1];
    }

    void LateUpdate()
    {
        bool isReverse = player.flipX;

        if (isLeft)
        {
            transform.localRotation = isReverse ? leftRotRevers : leftRot;
            spriter.flipY = isReverse;
            spriter.sortingOrder = isReverse ? 4 : 6;
        }
        else
        {
            transform.localPosition = isReverse ? rightPosRevers : rightPos;
            spriter.flipX = isReverse;
            spriter.sortingOrder = isReverse ? 6 : 4;
        }
    }
}
