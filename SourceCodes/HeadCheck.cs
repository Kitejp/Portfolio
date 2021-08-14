using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadCheck : MonoBehaviour
{
    // 頭をぶつけたかどうかを判定するための変数
    private bool isHead;
    private bool isHeadEnter, isHeadStay, isHeadExit;

    public bool IsHead()
    {
        if(isHeadEnter || isHeadStay)
        {
            isHead = true;
        }
        else if(isHeadExit)
        {
            isHead = false;
        }

        isHeadEnter = false;
        isHeadStay = false;
        isHeadExit = false;
        return isHead;
    }


    #region 頭をぶつけた時の接地判定
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Ground")
        {
            isHeadEnter = true;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.tag == "Ground")
        {
            isHeadStay = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "Ground")
        {
            isHeadExit = true;
        }
    }

    #endregion
}
