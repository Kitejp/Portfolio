using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 敵が壁や他の敵にぶつかったときの判定を行うスクリプト
public class EnemyCollisionCheck : MonoBehaviour
{
    /// <summary>
    /// 判定内に敵か壁がある
    /// </summary>
    [HideInInspector]
    public bool isOn;

    #region //接触判定
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Ground" || collision.tag == "Enemy")
        {
            isOn = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Ground" || collision.tag == "Enemy")
        {
            isOn = false;
        }
    }
    #endregion
}
