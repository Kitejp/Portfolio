using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    private Text scoreText;
    private int oldScore;

    void Start()
    {
        scoreText = GetComponent<Text>();
        if(GameManager.instance != null)
        {
            scoreText.text = "Score: " + GameManager.instance.score;
        }
        else
        {
            Debug.Log("ゲームマネージャーがありません。");
            Destroy(this);
        }
    }

    void Update()
    {
        if(oldScore != GameManager.instance.score)
        {
            scoreText.text = "Score: " + GameManager.instance.score;
            oldScore = GameManager.instance.score;
        }
    }
}
