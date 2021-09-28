using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeImageManager : MonoBehaviour
{
    [Header("最初からフェードインが完了しているかどうか")] public bool firstFadeInComp;

    private Image image;

    private int frameCount;

    private float timer;

    private bool fadeIn;
    private bool compFadeIn;
    private bool fadeOut;
    private bool compFadeOut;

    /// <summary>
    /// フェードインを開始する時の処理
    /// </summary>
    public void StartFadeIn()
    {
        if (fadeIn || fadeOut)
        {
            return;
        }
        fadeIn = true;
        compFadeIn = false;
        timer = 0.0f;
        image.color = new Color(1, 1, 1, 1);
        image.fillAmount = 1;
        image.raycastTarget = true;
    }

    /// <summary>
    /// フェードインが完了したかどうか
    /// </summary>
    /// <returns></returns>
    public bool IsFadeInComplete()
    {
        return compFadeIn;
    }

    /// <summary>
    /// フェードアウトを開始する時の処理
    /// </summary>
    public void StartFadeOut()
    {
        if (fadeIn || fadeOut)
        {
            return;
        }
        fadeOut = true;
        compFadeOut = false;
        timer = 0.0f;
        image.color = new Color(1, 1, 1, 0);
        image.fillAmount = 0;
        image.raycastTarget = true;
    }

    /// <summary>
    /// フェードアウトが完了したかどうか
    /// </summary>
    /// <returns></returns>
    public bool IsFadeOutComplete()
    {
        return compFadeOut;
    }

    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
        if (firstFadeInComp)
        {
            FadeInComplete();
        }
        else
        {
            StartFadeIn();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(frameCount > 2f)
        {
            if (fadeIn)
            {
                FadeInUpdate();
            }
            else if (fadeOut)
            {
                FadeOutUpdate();
            }
        }
        ++frameCount;
    }

    /// <summary>
    /// フェードインしている時の処理
    /// </summary>
    private void FadeInUpdate()
    {
        if(timer < 1f)
        {
            image.color = new Color(1, 1, 1, 1 - timer);
            image.fillAmount = 1 - timer;
        }
        else
        {
            FadeInComplete();
        }
        timer += Time.deltaTime;
    }

    /// <summary>
    /// フェードアウトしている時の処理
    /// </summary>
    private void FadeOutUpdate()
    {
        if (timer < 1f)
        {
            image.color = new Color(1, 1, 1, timer);
            image.fillAmount = timer;
        }
        else
        {
            FadeOutComplete();
        }
        timer += Time.deltaTime;
    }

    /// <summary>
    /// フェードインが完了した時の処理
    /// </summary>
    private void FadeInComplete()
    {
        image.color = new Color(1, 1, 1, 0);
        image.fillAmount = 0;
        image.raycastTarget = false;
        timer = 0.0f;
        fadeIn = false;
        compFadeIn = true;
    }

    /// <summary>
    /// フェードアウトが完了した時の処理
    /// </summary>
    private void FadeOutComplete()
    {
        image.color = new Color(1, 1, 1, 1);
        image.fillAmount = 1;
        image.raycastTarget = false;
        timer = 0.0f;
        fadeOut = false;
        compFadeOut = true;
    }
}
