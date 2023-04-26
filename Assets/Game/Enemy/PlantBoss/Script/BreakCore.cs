//---------------------------------
//　担当：菅眞心
//　プラント場ボスのダメージ処理
//---------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakCore : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //　ひびが当たったら消去
        if(collision.tag == "Crack")
        {
            Destroy(gameObject);
        }
    }
}
