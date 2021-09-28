using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ボスと戦闘する際に出現する壁の処理を行うスクリプト
public class BlockController : MonoBehaviour
{
    [Header("ブロックのオブジェクト")]
    public GameObject block1;
    public GameObject block2;
    [Header("ボスのオブジェクト")] public GameObject boss;

    // Start is called before the first frame update
    void Start()
    {
        // 初めは非表示にする
        block1.SetActive(false);
        block2.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // ボスを倒したときに非表示にする
        if (boss == null)
        {
            block1.SetActive(false);
            block2.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // プレイヤーの特定の位置（ボスの近く）に行った時に、表示する
        if (collision.tag == "Player")
        {
            block1.SetActive(true);
            block2.SetActive(true);
        }
    }
}
