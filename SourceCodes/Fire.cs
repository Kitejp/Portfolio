using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    public float fireSpeed = 10f;
    public float spawnTime = 0.1f;
    private float currentTime = 0f;

    private Rigidbody2D rb;

    private GameObject player;
    private GameObject boss;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.Find("Player");
        boss = GameObject.Find("Boss");

        if (boss != null)
        {
            if (player.transform.position.x > boss.transform.position.x)
            {
                fireSpeed = -fireSpeed;
                transform.localScale = new Vector3(5, 5, 5);
            }
        }
    }

    private void Update()
    {
        FireMove();

        currentTime += Time.deltaTime;

        if (currentTime > spawnTime)
            Destroy(this.gameObject);
    }

    private void FireMove()
    {
        Vector2 fireMovement = new Vector2(-1,0).normalized;

        rb.velocity = fireMovement * fireSpeed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
            Destroy(this.gameObject);
    }
}
