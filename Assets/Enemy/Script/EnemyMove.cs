//---------------------------------------------------------
//担当者：二宮怜
//内容　：敵の移動
//---------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    //---------------------------------------------------------
    // - 変数宣言 -

    public float MoveDistance = 1; // 移動範囲
    public float MoveSpeed = 0.05f; // 移動速度
    private Vector3 StartPosition; // 敵の開始位置
    private float StartTime = 0.0f; // 敵が生成されてからの経過時間
    public bool Stop = false; // デバッグ用 敵がその場にとどまる

    // 外部取得
    private Transform thisTranform; // このオブジェクトの座標を持つ変数

    // Start is called before the first frame update
    void Start()
    {
        //---------------------------------------------------------
        // オブジェクトのTransformを取得
        thisTranform = GetComponent<Transform>();

        //---------------------------------------------------------
        // 敵の開始位置を取得
        StartPosition = thisTranform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Stop == false)
        {
            //-------------------------------------------------------------------------------------------
            // 敵の開始位置からdistance * MoveSpeedの範囲で左右移動
            thisTranform.position = new Vector3(StartPosition.x + Mathf.Sin(StartTime) * MoveSpeed * MoveDistance, StartPosition.y, StartPosition.z);

            //-------------------------------------------------------------------------------------------
            //時間経過
            StartTime += Time.deltaTime;
        }
    }
}
