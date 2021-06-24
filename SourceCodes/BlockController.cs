using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockController : MonoBehaviour
{
    public GameObject block1;
    public GameObject block2;
    public GameObject boss;

    // Start is called before the first frame update
    void Start()
    {
        block1.SetActive(false);
        block2.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (boss == null)
        {
            block1.SetActive(false);
            block2.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            block1.SetActive(true);
            block2.SetActive(true);
        }
    }
}
