using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadCheck : MonoBehaviour
{
    public bool isHead;
    public bool isHeadEnter, isHeadStay, isHeadExit;

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
}
