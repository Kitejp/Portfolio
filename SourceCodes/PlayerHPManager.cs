using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHPManager : MonoBehaviour
{
    public GameObject hpSliderObj;
    public GameObject gpSliderObj;

    private Slider hpSlider;
    private Slider gpSlider;

    private int oldHp;
    private int oldGp;

    void Start()
    {
        hpSlider = hpSliderObj.GetComponent<Slider>();
        gpSlider = gpSliderObj.GetComponent<Slider>();

        if (GameManager.instance != null)
        {
            hpSlider.value = 100f;
            gpSlider.value = 50f;
        }
        else
        {
            Debug.Log("ゲームマネージャーがありません。");
            Destroy(this);
        }
    }

    void Update()
    {
        if (oldHp != GameManager.instance.playerHp)
        {
            hpSlider.value = GameManager.instance.playerHp;
            oldHp = GameManager.instance.playerHp;
        }

        if(oldGp != GameManager.instance.playerGuardPoint)
        {
            gpSlider.value = GameManager.instance.playerGuardPoint;
            oldGp = GameManager.instance.playerGuardPoint;
        }
    }
}
