//---------------------------------------------------------
//担当者：二宮怜
//内容　：プレイヤーの無敵時間を管理
//---------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitEnemy : MonoBehaviour
{
    //---------------------------------------------------------
    // - 変数宣言 -

    public float NoDamageTime = 2f; //無敵時間
    [SerializeField]public float HitTime = 0.0f; // 前回ダメージを受けた時からの経過時間

    private void Start()
    {
        // 始まった時無敵時間なのを防ぐため初期化
        HitTime = NoDamageTime;
    }

    void Update()
    {
        HitTime += Time.deltaTime;
    }
}
