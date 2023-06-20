using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float maxHealth;
    public float health;
    public float speed;
    public Rigidbody2D target;
    public RuntimeAnimatorController[] animCon;

    bool isLive;

    Rigidbody2D rigid;
    SpriteRenderer spriter;
    Animator anim;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriter = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    void OnEnable()
    {
        isLive = true;
        health = maxHealth;

        target = GameManager.instance.player.GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if(!isLive) return;

        Vector2 dirVec = target.position - rigid.position;
        Vector2 nextVec = dirVec.normalized * speed * Time.fixedDeltaTime;

        rigid.MovePosition(rigid.position + nextVec);
        rigid.velocity = Vector2.zero;
    }

    void LateUpdate()
    {
        if(!isLive) return;

        spriter.flipX = target.position.x < rigid.position.x;
    }

    public void Init(SpawnData data)
    {
        anim.runtimeAnimatorController = animCon[data.spriteType];
        maxHealth = data.health;
        health = data.health;
        speed = data.speed;
    }
}
