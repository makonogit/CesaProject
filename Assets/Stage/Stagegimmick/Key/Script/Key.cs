//-------------------------------------------
// 担当：藤原昂祐
// 内容：鍵
//------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    //=====================================
    // *** 変数宣言 ***
    //=====================================

    //-------------------------------------
    // 外部参照
    //-------------------------------------

    GameObject player;// プレイヤー

    //-------------------------------------
    // 移動関連
    //-------------------------------------

    // 回転
    Vector2 center;      // 回転の中心座標
    float angle;         // 回転角度
    float radius = 0.25f;// 円の半径

    //-------------------------------------
    // 状態制御関連
    //-------------------------------------

    enum StateID// メイン状態ID
    {
        NULL,   // 状態なし
        NO_GET, // 未取得状態
        GET,    // 取得状態
        
    }
    StateID oldState = StateID.NULL;// 前の状態
    StateID nowState = StateID.NULL; // 現在の状態
    StateID nextState = StateID.NULL;// 次の状態

    //=====================================
    // *** 初期化処理 ***
    //=====================================

    void Start()
    {
        //---------------------------------
        // 変数の初期化
        //---------------------------------

        // 状態を未取得状態に変更
        nowState = StateID.NO_GET;
        // 回転の中心座標に初期位置を保存
        center = transform.position;
        // プレイヤーのオブジェクトを取得
        player = GameObject.Find("player");
    }

    //=====================================
    // *** 更新処理 ***
    //=====================================

    void Update()
    {
        //---------------------------------
        //  現在の状態を次の状態に遷移
        //---------------------------------

        if (nextState != StateID.NULL)
        {
            oldState = nowState;
            nowState = nextState;
            nextState = StateID.NULL;
        }

        //---------------------------------
        //  現在の状態によって処理を分岐
        //---------------------------------

        switch (nowState)
        {
            // 未取得状態
            case StateID.NO_GET:NoGet();break;
            // 取得状態
            case StateID.GET:Get();     break;
        }
    }

    //=====================================
    // *** 衝突判定 ***
    //=====================================

    void OnTriggerEnter2D(Collider2D other)
    {
        //----------------------------------------------------
        //  未取得状態でプレイヤーと衝突したら取得状態にする
        //----------------------------------------------------

        if(nowState == StateID.NO_GET)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                nextState = StateID.GET;
            }
        }

        //---------------------------------------------------
        //  取得状態でドアに衝突したら、ドアと鍵を消去する
        //---------------------------------------------------

        if (nowState == StateID.GET)
        {
            if (other.gameObject.name == "door")
            {
                Destroy(other.gameObject);
                Destroy(this.gameObject);
            }
        }
    }

    //=====================================
    // *** 未取得状態の処理 ***
    //=====================================

    void NoGet()
    {
        //---------------------------------------------------
        //  鍵を上下にふわふわさせる
        //---------------------------------------------------

        // 現在のトランスフォームを取得
        Vector3 pos = this.transform.position;
        // 角度をラジアンに変換
        float rd = -angle * Mathf.PI / 180.0f;
        // 回転後の座標を計算
        //pos.x = center.x + (Mathf.Sin(rd) * radius) + radius + 0.1f;
        pos.y = center.y + (Mathf.Cos(rd) * radius) + radius + 0.1f;
        // 変更を反映
        this.transform.position = pos;
        // 角度を加算
        angle += 0.2f;
    }

    //=====================================
    // *** 取得状態の処理 ***
    //=====================================

    void Get()
    {
        //-------------------------------------
        // プレイヤーの少し上に鍵の座標を移動
        //-------------------------------------

        Vector3 pos = this.transform.position;
        pos.x = player.transform.position.x;
        pos.y = player.transform.position.y + 1.0f;
        this.transform.position = pos;
    }
}
