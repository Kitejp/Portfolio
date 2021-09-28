using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GPRecovery : MonoBehaviour
{
    // パブリック変数（インスペクターで設定する）
    [Header("回復量")] public int recoveryPoint;
    [Header("アイテムを拾った時に鳴る音")] public AudioClip gpRecoverySE;

    // プライベート変数
    [Header("音源")] private AudioSource audioSource;

    void Start()
    {
        audioSource = FindObjectOfType<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            GameManager.instance.playerGuardPoint += recoveryPoint;
            if (GameManager.instance.playerGuardPoint >= 50)
                GameManager.instance.playerGuardPoint = 50;

            audioSource.PlayOneShot(gpRecoverySE);
            Destroy(this.gameObject);
        }
    }
}
