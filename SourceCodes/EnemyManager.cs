using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyManager : MonoBehaviour
{
    #region //パブリック関数（インスペクターで設定する）
    [Header("移動速度")] public float speed;

    [Header("敵のHP")] public int enemyHp;
    [Header("敵を倒したときに得られるスコア")] public int score;

    [Header("画面外でも行動する")] public bool nonVisibleAct = false;

    [Header("接触判定")] public EnemyCollisionCheck collisionCheck;

    [Header("音源")] public AudioSource audioSource;
    [Header("死んだときのSE")] public AudioClip die;
    #endregion

    #region //プロテクティド変数
    protected Animator anim;
    protected Rigidbody2D rb;
    protected SpriteRenderer spriteRenderer;
    protected Slider hpSlider;
    #endregion

    public virtual void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        hpSlider = GetComponentInChildren<Slider>();
        hpSlider.maxValue = enemyHp; //HPスライダーの最大値を敵のHPに合わせる
        hpSlider.value = enemyHp; //HPスライダーの値を敵のHPに合わせる
    }

    /// <summary>
    /// プレイヤーから攻撃を受けたときの処理
    /// </summary>
    public virtual void OnDamage()
    {
        enemyHp -= 10;
        hpSlider.value -= 10;

        //HPが0以下になったときに死んだときの処理を行うメソッドを呼び出す
        if(enemyHp <= 0)
            Die();
    }

    public virtual void Die()
    {
        //HPがマイナスにならないようにするため
        enemyHp = 0;

        anim.SetTrigger("Die");

        //移動しないようにするため
        rb.velocity = Vector2.zero;

        audioSource.PlayOneShot(die);
        Destroy(this.gameObject,0.3f);
        GameManager.instance.score += this.score;
    }

    public virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.tag == "Player")
            collisionCheck.isOn = true;
    }

    public virtual void OnCollisionExit2D(Collision2D collision)
    {
        if(collision.collider.tag == "Player")
            collisionCheck.isOn = false;
    }
}
