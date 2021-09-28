using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPRecovery : MonoBehaviour
{
    [Header("回復量")] public int recoveryPoint;
    [Header("アイテムを拾った時に鳴る音")] public AudioClip hpRecoverySE;

    [Header("音源")] private AudioSource audioSource;

    private void Start()
    {
        audioSource = FindObjectOfType<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            GameManager.instance.playerHp += recoveryPoint;
            if (GameManager.instance.playerHp >= 100)
                GameManager.instance.playerHp = 100;

            audioSource.PlayOneShot(hpRecoverySE);
            Destroy(this.gameObject);
        }
    }
}
