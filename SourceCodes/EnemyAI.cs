using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyAI : EnemyManager
{
    [Header("ターゲット（プレイヤー）")] public Transform target;
    [Header("検知する範囲")]public float activateDistance = 50f;
    [Header("パスを更新する秒数")]public float pathUpdateSeconds = 0.5f;

    [Header("次の通過点の距離")] public float nextWaypointDistance = 3f;
    public float jumpNodeHeightRequirement = 0.8f;
    [Header("ジャンプするときにかかる力")]public float jumpForce = 0.3f;
    public float jumpCheckOffset = 0.1f;

    [Header("プレイヤーに追従するか")] public bool followEnabled = true;
    [Header("ジャンプするかどうか")]public bool jumpEnabled = true;
    [Header("方向転換するかどうか")]public bool directionLookEnabled = true;

    protected Path path;
    protected int currentWaypoint = 0;
    protected bool isGrounded = false;
    protected Seeker seeker;

    //右を向いているか
    private bool isFacingRight;

    public override void Start()
    {
        base.Start();
        seeker = GetComponent<Seeker>();

        InvokeRepeating("UpdatePath", 0f, pathUpdateSeconds);
    }

    protected void FixedUpdate()
    {
        if (spriteRenderer.isVisible || nonVisibleAct)
        {
            if (TargetInDistance() && followEnabled)
            {
                PathFollow();
            }
            else
            {
                if (collisionCheck.isOn)
                {
                    isFacingRight = !isFacingRight;
                }

                int xVector = -1;

                if (isFacingRight)
                {
                    xVector = 1;
                    transform.localScale = new Vector3(-5, 5, 5);
                }
                else
                {
                    transform.localScale = new Vector3(5, 5, 5);
                }
                rb.velocity = new Vector2(xVector * speed, -rb.gravityScale);
            }
        }
        else
        {
            rb.Sleep();
        }
    }

    protected void UpdatePath()
    {
        if(followEnabled && TargetInDistance() && seeker.IsDone())
            seeker.StartPath(rb.position, target.position, OnPathComplete);
    }

    public virtual void PathFollow()
    {
        if(path == null)
            return;

        if(currentWaypoint >= path.vectorPath.Count)
            return;

        Vector3 startOffset = transform.position - new Vector3(0f, GetComponent<Collider2D>().bounds.extents.y + jumpCheckOffset);
        isGrounded = Physics2D.Raycast(startOffset, -Vector3.up, 0.05f);

        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 force = direction * speed * Time.deltaTime;

        if(jumpEnabled && isGrounded)
        {
            if (direction.y > jumpNodeHeightRequirement)
            {
                rb.AddForce(Vector2.up * speed * jumpForce);
            }
        }

        rb.AddForce(force);

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }

        if (directionLookEnabled)
        {
            if(rb.velocity.x > 0.05f)
            {
                transform.localScale = new Vector3(-1f * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);

            }
            else if(rb.velocity.x < -0.05f)
            {
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }

            // 敵が方向転換するとHPバーの向きも変わってしまうため、変わらないようにする
            if (transform.localScale.x < 0)
            {
                hpSlider.transform.localScale = new Vector3(-1 * Mathf.Abs(hpSlider.transform.localScale.x), hpSlider.transform.localScale.y, hpSlider.transform.localScale.z);
            }
            else
            {
                hpSlider.transform.localScale = new Vector3(Mathf.Abs(hpSlider.transform.localScale.x), hpSlider.transform.localScale.y, hpSlider.transform.localScale.z);
            }
        }
    }

    protected bool TargetInDistance()
    {
        return Vector2.Distance(transform.position, target.transform.position) < activateDistance;
    }

    protected void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }
}
