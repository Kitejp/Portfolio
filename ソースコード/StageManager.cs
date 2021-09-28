using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageManager : MonoBehaviour
{
    [Header("プレイヤーゲームオブジェクト")]public GameObject playerObj;
    [Header("コンティニュー位置")] public GameObject[] continuePoint;
    [Header("ゲームオーバーの画像")] public GameObject gameOverObj;
    [Header("ステージクリアの画像")] public GameObject stageClearObj;
    [Header("ボスのオブジェクト")] public GameObject bossObj;
    [Header("メニューのオブジェクト")] public GameObject menuObj;
    [Header("説明のオブジェクト")] public GameObject explanationObj;

    [Header("フェード")] public FadeImageManager fade;

    public AudioSource audioSource;
    public AudioClip gameOverSound, clearSound;

    private int nextStageNum;

    private bool startFade = false;
    private bool doGameOver = false;
    private bool retryGame = false;
    private bool doSceneChange = false;
    private bool doStageClear = false;

    private PlayerController playerController;

    void Start()
    {
        if(playerObj != null && continuePoint != null && continuePoint.Length > 0)
        {
            gameOverObj.SetActive(false);
            stageClearObj.SetActive(false);
            menuObj.SetActive(false);
            explanationObj.SetActive(false);
            playerObj.transform.position = continuePoint[0].transform.position;
            playerController = playerObj.GetComponent<PlayerController>();

            if(playerController == null)
            {
                Debug.Log("プレイヤーではないものがアタッチされています。");
            }
        }
        else
        {
            Debug.Log("設定が足りません。");
        }
    }

    void Update()
    {
        if (GameManager.instance.isGameOver && !doGameOver)
        {
            Time.timeScale = 0;
            audioSource.Stop();
            gameOverObj.SetActive(true);
            audioSource.PlayOneShot(gameOverSound);
            doGameOver = true;
        }
        else if(playerController != null && playerController.IsContinueWaiting() && !doGameOver)
        {
            if(continuePoint.Length > GameManager.instance.continueNum)
            {
                playerObj.transform.position = continuePoint[GameManager.instance.continueNum].transform.position;
                playerController.PlayerContinue();
            }
            else
            {
                Debug.Log("コンティニューポイントの設定が足りません。");
            }
        }

        if (bossObj == null && !doStageClear)
        {
            Time.timeScale = 0;
            audioSource.Stop();
            stageClearObj.SetActive(true);
            audioSource.PlayOneShot(clearSound);
            doStageClear = true;
        }

        if (fade != null && startFade && !doSceneChange)
        {
            if (fade.IsFadeOutComplete())
            {
                if (retryGame)
                {
                    GameManager.instance.RetryGame();
                }
                else
                {
                    GameManager.instance.stageNum = nextStageNum;
                }
                SceneManager.LoadScene(nextStageNum);
                doSceneChange = true;
            }
        }

        if (Input.GetKey(KeyCode.Escape) && fade.IsFadeInComplete())
            Menu();
    }

    public void Retry()
    {
        Time.timeScale = 1;
        ChangeScene(GameManager.instance.stageNum);
        retryGame = true;
    }

    public void BackToTitle()
    {
        Time.timeScale = 1;
        ChangeScene(0);
        retryGame = true;
    }

    public void NextStage()
    {
        Time.timeScale = 1;
        ChangeScene(2);
        GameManager.instance.playerHp += 50;
        if (GameManager.instance.playerHp >= 100)
            GameManager.instance.playerHp = 100;

        GameManager.instance.playerGuardPoint = 50;
    }

    /// <summary>
    /// メニューの「タイトルに戻る」ボタンを押したときの処理
    /// </summary>
    public void Title()
    {
        Time.timeScale = 1;
        ChangeScene(0);
    }

    /// <summary>
    /// メニューを開いている時の処理
    /// </summary>
    private void Menu()
    {
        Time.timeScale = 0;
        menuObj.SetActive(true);
    }

    /// <summary>
    /// メニューの「操作説明」ボタンを押したときの処理
    /// </summary>
    public void Explanation()
    {
        explanationObj.SetActive(true);
        menuObj.SetActive(false);
    }

    /// <summary>
    /// メニューの「戻る」ボタンを押したときの処理
    /// </summary>
    public void Back()
    {
        Time.timeScale = 1;
        menuObj.SetActive(false);
        explanationObj.SetActive(false);
    }

    /// <summary>
    /// シーン遷移するときの処理
    /// </summary>
    /// <param name="num"></param>
    public void ChangeScene(int num)
    {
        if (fade != null)
        {
            nextStageNum = num;
            fade.StartFadeOut();
            startFade = true;
        }
    }
}
