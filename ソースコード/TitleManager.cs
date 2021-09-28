using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class TitleManager : MonoBehaviour
{
    [Header("フェード")]public FadeImageManager fade;

    public TextMeshProUGUI exText, backText, titleText, startText, goText, quitText;

    private bool firstPush = false;
    private bool goNextScene = false;

    private void Start()
    {
        exText.transform.parent.gameObject.SetActive(false);
        backText.transform.parent.gameObject.SetActive(false);
    }

    /// <summary>
    /// 「ゲームを始める」のボタンを押した時の処理
    /// </summary>
    public void PressStart()
    {
        if (!firstPush)
        {
            fade.StartFadeOut();
            firstPush = true;
        }
    }

    /// <summary>
    /// 「操作説明」のボタンを押した時の処理
    /// </summary>
    public void Explanation()
    {
        exText.transform.parent.gameObject.SetActive(true);
        backText.transform.parent.gameObject.SetActive(true);
        startText.transform.parent.gameObject.SetActive(false);
        goText.transform.parent.gameObject.SetActive(false);
        quitText.transform.parent.gameObject.SetActive(false);
        titleText.gameObject.SetActive(false);
    }

    /// <summary>
    /// 「戻る」ボタンを押した時の処理
    /// </summary>
    public void Back()
    {
        exText.transform.parent.gameObject.SetActive(false);
        backText.transform.parent.gameObject.SetActive(false);
        startText.transform.parent.gameObject.SetActive(true);
        goText.transform.parent.gameObject.SetActive(true);
        quitText.transform.parent.gameObject.SetActive(true);
        titleText.gameObject.SetActive(true);
    }

    /// <summary>
    /// 「ゲームを終了する」ボタンを押したときの処理
    /// </summary>
    public void Quit()
    {
        Application.Quit();
    }

    private void Update()
    {
        // フェードアウトが完了した時にシーン遷移
        if(!goNextScene && fade.IsFadeOutComplete())
        {
            GameManager.instance.stageNum = 1;
            SceneManager.LoadScene("stage" + GameManager.instance.stageNum);
            goNextScene = true;
        }
    }
}
