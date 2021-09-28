using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordVFX : MonoBehaviour
{
    public float speed = 10f;
    public float spawnTime = 0.1f;
    [Header("攻撃範囲")] public float attackRadius;
    [Header("敵のレイヤー")] public LayerMask enemyLayer;

    private float currentTime = 0f;
    private Rigidbody2D rb;
    private PlayerController player;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = FindObjectOfType<PlayerController>();

        if (player.transform.localScale.x < 0)
        {
            speed = -speed;
            transform.localScale = new Vector3(-5, 5, 5);
        }
    }

    // Update is called once per frame
    void Update()
    {
        Move();

        currentTime += Time.deltaTime;

        if (currentTime > spawnTime)
            Destroy(this.gameObject);
    }

    private void Move()
    {
        Vector2 movement = new Vector2(1, 0).normalized;
        rb.velocity = movement * speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            // OverlapCircleAllで触れた敵（のコライダー）を取得する
            Collider2D[] hitEnemys = Physics2D.OverlapCircleAll(this.transform.position, attackRadius, enemyLayer);

            // 取得した敵にダメージを与える
            foreach (Collider2D hitEnemy in hitEnemys)
            {
                hitEnemy.GetComponent<EnemyManager>().OnDamage();
                Destroy(this.gameObject);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(this.transform.position, attackRadius);
    }
}
