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

    public void PressStart()
    {
        if (!firstPush)
        {
            fade.StartFadeOut();
            firstPush = true;
        }
    }

    public void Explanation()
    {
        exText.transform.parent.gameObject.SetActive(true);
        backText.transform.parent.gameObject.SetActive(true);
        startText.transform.parent.gameObject.SetActive(false);
        goText.transform.parent.gameObject.SetActive(false);
        quitText.transform.parent.gameObject.SetActive(false);
        titleText.gameObject.SetActive(false);
    }

    public void Back()
    {
        exText.transform.parent.gameObject.SetActive(false);
        backText.transform.parent.gameObject.SetActive(false);
        startText.transform.parent.gameObject.SetActive(true);
        goText.transform.parent.gameObject.SetActive(true);
        quitText.transform.parent.gameObject.SetActive(true);
        titleText.gameObject.SetActive(true);
    }

    public void Quit()
    {
        Application.Quit();
    }

    private void Update()
    {
        if(!goNextScene && fade.IsFadeOutComplete())
        {
            GameManager.instance.stageNum = 1;
            SceneManager.LoadScene("stage" + GameManager.instance.stageNum);
            goNextScene = true;
        }
    }
}
