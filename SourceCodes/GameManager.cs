using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    [Header("スコア")]public int score;
    [Header("ステージの番号")]public int stageNum;
    [Header("現在のコンティニュー位置")] public int continueNum;
    [Header("プレイヤーのHP")]public int playerHp;
    public int playerGuardPoint;

    [HideInInspector] public bool isGameOver = false;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void RetryGame()
    {
        isGameOver = false;
        playerHp = 100;
        playerGuardPoint = 50;
        score = 0;
        stageNum = 1;
        continueNum = 0;
    }
}
