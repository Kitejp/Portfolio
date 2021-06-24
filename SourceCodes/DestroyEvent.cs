using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyEvent : MonoBehaviour
{
    //*このメソッドは、AnimationのEventで呼び出される
    public void Destroy()
    {
        Destroy(this.gameObject);
    }
}
