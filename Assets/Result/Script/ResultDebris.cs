//------------------------------------------------------------------------------
// 担当：藤原昂祐
// 内容：リザルトの破片
//------------------------------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultDebris : MonoBehaviour
{
    //============================================================
    // *** 変数宣言 ***
    //============================================================
   
    //-------------------------------------
    // 移動関連
    //-------------------------------------

    // 回転速度
    Vector3 rot_speed = new Vector3(0.0f, 0.0f,0.05f);

    // 移動速度
    public Vector2 move_speed;

    public float acceleration;

    // 移動方向（単位ベクトル）
    Vector2 direction;

    //目標地点
    Vector2 destination; 

    GameObject obj;
    ResultManager resultManager;

    //============================================================
    // *** 初期化処理 ***
    //============================================================

    void Start()
    {
        //-------------------------------------
        // テキストの周辺を目的地に設定
        //-------------------------------------

        // 目標地点を初期化
        obj = GameObject.Find("Result_StageClear");
        resultManager = obj.GetComponent<ResultManager>();
        destination = obj.transform.position;

        //-------------------------------------
        // ランダムに移動方向を決定する
        //-------------------------------------

        // 乱数を生成
        int rndX = Random.Range(-1, 1 + 1);
        int rndY = Random.Range(-1, 1 + 1);
        // 方向ベクトルを初期化
        direction.x = rndX;
        direction.y = rndY;
    }

    //============================================================
    // *** 更新処理 ***
    //============================================================

    void Update()
    {

        //-------------------------------------
        // 破片を回転させる
        //-------------------------------------

        // 現在の角度を取得
        Vector3 rot = this.transform.eulerAngles;
        // 現在の角度に回転速度を加算
        rot += rot_speed;
        // 現在の角度を更新
        this.transform.eulerAngles = rot;

        //-------------------------------------
        // 破片を移動させる
        //-------------------------------------

        // 現在の座標を取得
        Vector2 position = this.transform.position;
        // ベクトルの成分を求める
        Vector2 components;
        components.x = destination.x - this.transform.position.x;
        components.y = destination.y - this.transform.position.y;
        // ベクトルの大きさを求める
        float magnitude = (float)Mathf.Sqrt(components.x * components.x + components.y * components.y);
        if (resultManager.GetMoveFlg() == true)
        {
            // ベクトルを正規化
            direction.x = components.x / magnitude;
            direction.y = components.y / magnitude;
            move_speed.x += acceleration;
            move_speed.y += acceleration;

            // ベクトルの大きさが1未満ならこのオブジェクトを消去
            if (magnitude < 1)
            {
                Destroy(gameObject);
            }
        }
        // 現在の座標に移動ベクトルを加算
        position += move_speed * direction;
        // 現在の座標を更新
        this.transform.position = position;   
    }
}


