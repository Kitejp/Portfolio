using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region //インスペクターで設定する
    [Header("移動速度")]public float speed; 
    [Header("ジャンプする時にかかる力の大きさ")] public float jumpForce;
    [Header("接地判定の大きさ")] public float checkRadius;
    [Header("ジャンプ時間")]public float jumpTime;
    [Header("攻撃範囲")]public float attackRadius;
    [Header("壁ジャンプする時にかかる力の大きさ")]public float wallJumpForce = 20f;
    [Header("壁ジャンプの時間")]public float wallSlideTime = 0.2f;
    [Header("壁を滑る速度の最小値")]public float wallSlideSpeedMin = 0.5f;
    [Header("壁を判定する距離")]public float wallDistance = 0.4f;

    [Header("頭をぶつけた判定")] public HeadCheck headCheck;

    [Header("ダッシュの速さ表現")] public AnimationCurve dashCurve;

    [Header("攻撃判定の位置")]public Transform attackPoint;
    [Header("プレイヤーの足の座標")] public Transform feetPos;

    [Header("敵のレイヤー")]public LayerMask enemyLayer;
    [Header("地面のレイヤー")] public LayerMask groundLayer;

    [Header("音源")]public AudioSource audioSource;
    [Header("効果音")]public AudioClip run, jump, sword, hit;
    #endregion

    #region //プライベート変数
    private Animator anim;
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private BoxCollider2D col;
    private RaycastHit2D wallCheckHit;

    // 動作の判定
    private bool isRun;
    private bool isGrounded;
    private bool isHead; 
    private bool isJump;
    private bool isRolling;
    private bool isDie;
    private bool isHurt;
    private bool isAttack;
    private bool isContinue;
    private bool isBlocking;
    private bool isWallSliding;
    private bool touchingGround;
    private bool isPushLeftButton;
    private bool isPushRightButton;
    private bool isPushJumpButton;
    private bool isPushAttackButton;
    private bool isPushGuardButton;
    private bool isPushRollingButton;

    private float jumpTimeCounter;
    private float dashTime;
    private float beforeKey;
    private float blinkTime;
    private float hurtTime;
    private float continueTime;
    private float wallSlideTimeCounter;
    #endregion

    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        col = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        //頭の判定を得る
        isHead = headCheck.IsHead();
        // 地面のコライダーを取得する
        touchingGround = Physics2D.OverlapCircle(feetPos.position, checkRadius, groundLayer);

        if (touchingGround)
        {
            isGrounded = true;
            wallSlideTimeCounter = Time.time + wallSlideTime;
        }
        else if (wallSlideTimeCounter < Time.time)
        {
            isGrounded = false;
        }

        if (!isDie && !GameManager.instance.isGameOver)
        {
            Jump();
            Rolling();
            IsAttack();
            IsBlock();
        }

        if (GameManager.instance.playerHp <= 0)
        {
            PlayerDie();
        }

        //プレイヤーがダメージを受けたこと、もしくは、復活したことを分かりやすくするために点滅させるようにしています
        if (isHurt || isContinue)
        {
            if (blinkTime > 0.2f)
            {
                sr.enabled = true;
                blinkTime = 0f;
            }
            else if (blinkTime > 0.1f)
                sr.enabled = false;
            else
                sr.enabled = true;

            if (hurtTime > 1f)
            {
                isHurt = false;
                blinkTime = 0f;
                hurtTime = 0f;
                sr.enabled = true;
            }
            else
            {
                blinkTime += Time.deltaTime;
                hurtTime += Time.deltaTime;
            }

            if (continueTime > 1f)
            {
                isContinue = false;
                blinkTime = 0f;
                continueTime = 0f;
                sr.enabled = true;
            }
            else
            {
                blinkTime += Time.deltaTime;
                continueTime += Time.deltaTime;
            }
        }


    }

    private void FixedUpdate()
    {
        if (!isDie && !GameManager.instance.isGameOver)
        {
            //x座標軸の速度を求める
            float xSpeed = GetXSpeed();

            //移動速度を設定する
            rb.velocity = new Vector2(xSpeed, rb.velocity.y);

            WallSlide();
            SetBoolAnimation();
        }
        else
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
    }

    /// <summary>
    /// X成分で必要な計算をし、速度を返す。
    /// </summary>
    /// <returns> X軸の速さ </returns>
    private float GetXSpeed()
    {
        float horizontalKey = Input.GetAxis("Horizontal");
        float xSpeed;

        if (horizontalKey > 0 || isPushRightButton)
        {
            transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
            dashTime += Time.deltaTime;
            xSpeed = speed;
            isRun = true;
        }
        else if (horizontalKey < 0 || isPushLeftButton)
        {
            transform.localScale = new Vector3(-1.5f, 1.5f, 1.5f);
            dashTime += Time.deltaTime;
            xSpeed = -speed;
            isRun = true;
        }
        else
        {
            xSpeed = 0;
            dashTime = 0;
            isRun = false;
        }

        if (horizontalKey > 0 && beforeKey < 0)
        {
            dashTime = 0;
        }
        else if (horizontalKey < 0 && beforeKey > 0)
        {
            dashTime = 0;
        }

        beforeKey = horizontalKey;

        if (isBlocking)
        {
            xSpeed = 0;
        }

        //アニメーションカーブを移動速度に適応する
        xSpeed *= dashCurve.Evaluate(dashTime);

        return xSpeed;
    }

    /// <summary>
    /// ジャンプ
    /// </summary>
    private void Jump()
    {
        if (!isDie && !GameManager.instance.isGameOver)
        {
            if (isGrounded && (Input.GetKeyDown(KeyCode.Space) || isPushJumpButton))
            {
                isJump = true;
                jumpTimeCounter = jumpTime;
                rb.velocity = Vector2.up * jumpForce;
            }

            if (Input.GetKey(KeyCode.Space) || isPushJumpButton)
            {
                if (jumpTimeCounter > 0 && isJump)
                {
                    rb.velocity = Vector2.up * jumpForce;
                    jumpTimeCounter -= Time.deltaTime;
                }
                else
                {
                    isJump = false;
                }
            }

            if (Input.GetKeyUp(KeyCode.Space))
            {
                isJump = false;
            }

            if (isHead)
            {
                isJump = false;
            }
        }
    }

    /// <summary>
    /// 壁にぶつかったときの処理
    /// </summary>
    private void WallSlide()
    {
        float horizontalKey = Input.GetAxis("Horizontal");

        if (transform.localScale.x == 1.5f)
        {
            wallCheckHit = Physics2D.Raycast(transform.position, new Vector2(wallDistance, 0), wallDistance, groundLayer);
            Debug.DrawRay(transform.position, new Vector2(wallDistance, 0), Color.blue);
        }
        else if(transform.localScale.x == -1.5f)
        {
            wallCheckHit = Physics2D.Raycast(transform.position, new Vector2(-wallDistance, 0), wallDistance, groundLayer);
            Debug.DrawRay(transform.position, new Vector2(-wallDistance, 0), Color.blue);
        }

        if(wallCheckHit && !isGrounded && horizontalKey != 0)
        {
            isWallSliding = true;
            wallSlideTimeCounter = Time.time + wallSlideTime;
        }
        else if(wallSlideTime < Time.time)
        {
            isWallSliding = false;
        }

        if (isWallSliding)
        {
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, wallSlideSpeedMin, float.MaxValue));
        }
    }
    /// <summary>
    /// ローリングの処理
    /// </summary>
    private void Rolling()
    {
        if (Input.GetAxis("Rolling") > 0 || isPushRollingButton)
        {
            isRolling = true;
            col.offset = new Vector2(0.08f, 0.47f);
            col.size = new Vector2(0.8f, 0.55f);
        }
        else
        {
            isRolling = false;
            col.offset = new Vector2(-0.03f, 0.69f);
            col.size = new Vector2(0.32f, 1.1f);
        }
    }

    #region //攻撃
    /// <summary>
    /// 攻撃の処理(*このメソッドはAnimationのEventで呼び出す)
    /// </summary>
    public void Attack()
    {
        audioSource.PlayOneShot(sword);

        // OverlapCircleAllでプレイヤーが攻撃した時に触れた敵（のコライダー）を取得する
        Collider2D[] hitEnemys = Physics2D.OverlapCircleAll(attackPoint.position, attackRadius, enemyLayer);

        // 取得した敵にダメージを与える
        foreach (Collider2D hitEnemy in hitEnemys)
        {
            hitEnemy.GetComponent<EnemyManager>().OnDamage();
        }
    }

    /// <summary>
    /// 攻撃の判定
    /// </summary>
    private void IsAttack()
    {
        if(Input.GetAxis("Fire1") > 0 || isPushAttackButton)
        {
            isAttack = true;
        }
        else
        {
            isAttack = false;
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
    #endregion

    #region //防御

    /// <summary>
    /// 防御の処理
    /// </summary>
    public void Block()
    {
        // ガードポイント（ゲーム画面上の青色のゲージ）がある場合は、防御できる
        if(GameManager.instance.playerGuardPoint > 0)
        {
            anim.SetTrigger("blockSuccess");
            GameManager.instance.playerGuardPoint -= 10;
        }
        // ガードポイントがない場合は、防御出来ずにダメージを受ける
        else
        {
            GameManager.instance.playerGuardPoint = 0;
            isBlocking = false;
        }
    }

    /// <summary>
    /// 防御しているかどうかを判定するためのメソッド
    /// </summary>
    private void IsBlock()
    {
        if (Input.GetKey(KeyCode.Q) || isPushGuardButton)
        {
            isBlocking = true;
        }
        else
        {
            isBlocking = false;
        }
    }

    #endregion

    /// <summary>
    /// プレイヤーがダメージを受けた時の処理を行う
    /// </summary>
    public void PlayerHurt()
    {
        if(isBlocking)
        {
            Block();
        }
        else
        {
            isHurt = true;
            anim.SetTrigger("Hurt");
            audioSource.PlayOneShot(hit);
            GameManager.instance.playerHp -= 10;
        }
    }

    /// <summary>
    /// プレイヤーが死亡したときの処理を行う
    /// </summary>
    private void PlayerDie()
    {
        GameManager.instance.playerHp = 0;
        isDie = true;
        anim.Play("die");
        GameManager.instance.isGameOver = true;
    }

    #region //コンティニューした時の処理
    public void PlayerContinue()
    {
        isDie = false;
        anim.Play("idle");
        isJump = false;
        isContinue = true;
    }

    public bool IsContinueWaiting()
    {
        return IsDieAnimEnd();
    }

    private bool IsDieAnimEnd()
    {
        if(isDie && anim != null)
        {
            AnimatorStateInfo currentState = anim.GetCurrentAnimatorStateInfo(0);

            if (currentState.IsName("die"))
            {
                if(currentState.normalizedTime >= 1)
                {
                    return true;
                }
            }
        }

        return false;
    }
    #endregion

    /// <summary>
    /// Bool型のアニメーションを設定する
    /// </summary>
    private void SetBoolAnimation()
    {
        anim.SetBool("isRun", isRun);
        anim.SetBool("jump", isJump);
        anim.SetBool("isRolling", isRolling);
        anim.SetBool("ground", isGrounded);
        anim.SetBool("isAttack", isAttack);
        anim.SetBool("isBlocking", isBlocking);
        anim.SetBool("isWallSliding", isWallSliding);
    }

    /// <summary>
    /// 敵との接触判定
    /// </summary>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.tag == "Enemy")
        {
            PlayerHurt();
        }
    }
    /// <summary>
    /// 穴に落ちた時の処理
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "DeadArea")
        {
            PlayerDie();
        }
    }

    #region // ボタンの処理

    /// <summary>
    /// 左矢印ボタンの処理
    /// </summary>
    public void LeftButtonDown()
    {
        isPushLeftButton = true;
    }

    public void LeftButtonUp()
    {
        isPushLeftButton = false;
    }

    /// <summary>
    /// 右矢印ボタンの処理
    /// </summary>
    public void RightButtonDown()
    {
        isPushRightButton = true;
    }

    public void RightButtonUp()
    {
        isPushRightButton = false;
    }

    /// <summary>
    /// ジャンプボタンの処理
    /// </summary>
    public void JumpButtonDown()
    {
        isPushJumpButton = true;
    }

    public void JumpButtonUp()
    {
        isPushJumpButton = false;
        isJump = false;
    }

    /// <summary>
    /// ローリングボタンの処理
    /// </summary>
    public void RollingButtonDown()
    {
        isPushRollingButton = true;
    }

    public void RollingButtonUp()
    {
        isPushRollingButton = false;
    }

    /// <summary>
    /// 攻撃ボタンの処理
    /// </summary>
    public void AttackButtonDown()
    {
        isPushAttackButton = true;
    }

    public void AttackButtonUp()
    {
        isPushAttackButton = false;
    }

    /// <summary>
    /// ガードボタンの処理
    /// </summary>
    public void GuardButtonDown()
    {
        isPushGuardButton = true;
    }

    public void GuardButtonUp()
    {
        isPushGuardButton = false;
    }
    #endregion

    /// <summary>
    /// 走っている時に効果音を再生する(*このメソッドはAnimationのEventで呼び出す)
    /// </summary>
    public void RunSE()
    {
        audioSource.PlayOneShot(run);
    }

    /// <summary>
    /// ジャンプ時に効果音を再生する(*このメソッドはAnimationのEventで呼び出す)
    /// </summary>
    public void JumpSE()
    {
        audioSource.PlayOneShot(jump);
    }
}
