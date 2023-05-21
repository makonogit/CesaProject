//--------------------------------
//担当：二宮怜
//内容：ボスの通路にいるかどうかを伝える
//--------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GettingSmallerBGM : MonoBehaviour
{
    // 変数宣言
    private string playerTag = "Player";

    private bool InArea = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // プレイヤーのみ
        if(collision.gameObject.tag == playerTag)
        {
            InArea = true;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        // プレイヤーのみ
        if (collision.gameObject.tag == playerTag)
        {
            InArea = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // プレイヤーのみ
        if (collision.gameObject.tag == playerTag)
        {
            InArea = false;
        }
    }

    public bool GetInPassageArea()
    {
        return InArea;
    }
}
