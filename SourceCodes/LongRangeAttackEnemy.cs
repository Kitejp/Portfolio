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
        // x方向の速度によって、歩行のアニメーションを再生するかどうか処理する
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

        // y方向の速度によって、ジャンプのアニメーションを再生するかどうか処理する
        if (rb.velocity.y > 0.05f)
        {
            anim.SetBool("jump", true);
        }
        else
        {
            anim.SetBool("jump", false);
        }

        // プレイヤーと敵との間隔が50以下だと攻撃する
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

    /// <summary>
    /// 攻撃の処理
    /// </summary>
    private void Attack()
    {
        anim.SetTrigger("IsShooting");
        audioSource.PlayOneShot(shoot);
        Instantiate(bullet, attackPoint.position, Quaternion.identity);
    }

    /// <summary>
    /// ダメージを受けた時の処理
    /// </summary>
    public override void OnDamage()
    {
        base.OnDamage();

        if (enemyHp > 0)
        {
            anim.SetTrigger("Hurt");
        }
    }
}
