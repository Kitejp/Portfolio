using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShortRangeAttackEnemy : EnemyAI
{
    [Header("攻撃範囲")] public float attackRadius;

    [Header("攻撃判定の位置")] public Transform attackPoint;

    [Header("プレイヤーのレイヤー")] public LayerMask playerLayer;

    public AudioClip hit;

    private void Update()
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

        if (Vector2.Distance(transform.position, target.position) < 10f)
        {
            anim.SetTrigger("IsAttack");
        }
    }

    /// <summary>
    /// 攻撃の処理(このメソッドはAnimationのEventで呼び出される)
    /// </summary>
    public void Attack()
    {
        Collider2D[] hitPlayers = Physics2D.OverlapCircleAll(attackPoint.position, attackRadius, playerLayer);

        foreach (Collider2D hitPlayer in hitPlayers)
        {
            hitPlayer.GetComponent<PlayerController>().PlayerHurt();
        }
    }

    /// <summary>
    /// Sceneビューで攻撃範囲を可視化できるようにするためのメソッド
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
    }

    /// <summary>
    /// ダメージを受けた時の処理
    /// </summary>
    public override void OnDamage()
    {
        base.OnDamage();

        if (enemyHp > 0)
        {
            anim.SetTrigger("Hit");
            audioSource.PlayOneShot(hit);
        }
    }
}
