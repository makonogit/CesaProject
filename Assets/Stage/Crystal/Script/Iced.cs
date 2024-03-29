//---------------------------------------------------------
//担当者：二宮怜
//内容　：釘を打つと割れる氷の処理
//---------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Iced : MonoBehaviour
{
    //----------------------------------------------------------------------------------------------------------
    // - 変数宣言 -
    private string NailTag = "UsedNail";

    // 投げた釘と氷の部分が接触したら氷が割れる(現段階では消える)
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == NailTag)
        {
            Material mat = GetComponent<SpriteRenderer>().material;

            mat.color = new Color(0.0f, 0.0f, 0.0f, 0.0f);

            this.gameObject.GetComponent<BoxCollider2D>().enabled = false;
        }
    }
}
