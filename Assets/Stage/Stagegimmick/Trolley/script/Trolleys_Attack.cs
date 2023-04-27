//---------------------------------------------------------
//
// 担当者：中川直登
//
// 内容　：トロッコの攻撃
//
//---------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Trolleys_Attack : MonoBehaviour
{


    //======================================================
    //
    // 関数：Start()
    //
    //void Start(){}

    //======================================================
    //
    // 関数： Update()
    //
    //void Update(){}

    //======================================================
    //
    // 関数： OnTriggerEnter2D(Collider2D collision)
    // 
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //当たった物が Enemyタグを付けていたら
        if (collision.tag == "Enemy") Destroy(collision.gameObject);// それを消す
    }
}