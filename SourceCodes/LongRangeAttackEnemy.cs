using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LongRangeAttackEnemy : EnemyAI
{
    [Header("弾のPrefab")] public GameObject bullet;

    [Header("攻撃判定の位置")] public Transform attackPoint;

    [Header("攻撃の間隔")]public float span = 5f;

    [Header("攻撃の効果音")]public AudioClip shoot;

    private float currentTime = 0f;

    void Update()
    {
        if (rb.velocity.x > 0.05f)
        {
            anim.SetBool("move", true);
        }
        else if (rb.velocity.x < -0.05f)
        {
            anim.SetBool("move", true);
        }
        else
        {
            anim.SetBool("move", false);
        }

        if (rb.velocity.y > 0.05f)
        {
            anim.SetBool("jump", true);
        }
        else
        {
            anim.SetBool("jump", false);
        }

        if (Vector2.Distance(transform.position, target.position) < 50f)
        {
            currentTime += Time.deltaTime;

            if (currentTime > span)
            {
                Attack();
                currentTime = 0f;
            }
        }
    }

    private void Attack()
    {
        anim.SetTrigger("IsShooting");
        audioSource.PlayOneShot(shoot);
        Instantiate(bullet, attackPoint.position, Quaternion.identity);
    }

    public override void OnDamage()
    {
        base.OnDamage();

        if (enemyHp > 0)
        {
            anim.SetTrigger("Hurt");
        }
    }
}
