//------------------------------------------------------------------------------
// 担当：藤原昂祐
// 内容：リザルトの破片
//------------------------------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultDebris : MonoBehaviour
{
    //--------------------------------------------------------------------------
    // - 変数宣言 -

    // 座標関連
    Vector3 startRot;       // 生成時の回転率
    Vector3 startPos;       // 生成時の座標
    public Vector3 clearPos;// テキスト生成時の座標

    // テキスト関連
    bool isMoveFlg = false;// 移動開始フラグ
    int delayCnt = 0;      // 待機時間をカウント

    //============================================================
    // - 初期化処理 -

    void Start()
    {
        //-------------------------------------------------------
        // 初期位置を保存する

        Transform objTransform = this.transform;
        startRot = objTransform.eulerAngles;
        startPos = objTransform.position;
    }

    //============================================================
    // - 更新処理 -

    void Update()
    {
        //--------------------------------------------------------
        // 落下時の挙動を制御する

        // このオブジェクトのTransformを取得
        Transform objTransform = this.transform;

        // 座標の指定
        Vector3 pos = objTransform.position;

        if (isMoveFlg == false)
        {
            // 落下させる
            if (pos.y > -5.0f)
            {
                pos.y -= 0.02f;
            }

            // 回転させる
            if (pos.y > -4.0f)
            {
                startRot.z += 0.1f;
            }

            // 画面の下まで落ちたら待機
            if(pos.y < -5.0f)
            {
                delayCnt++;
               
            }

            // 待機時間が終わったら移動開始
           if(delayCnt > 60 * 8)
            {
                isMoveFlg = true;
                delayCnt = 0;
            }
        }
        else
        {

            //--------------------------------------------------------
            // テキスト生成時の挙動を制御する

            if (clearPos.z != 0.0f)
            {
               
                // 大きさをテキスト用に小さくする
                Vector3 scale = objTransform.localScale;
                scale.x = 0.8f;
                scale.y = 0.8f;
                scale.z = 1.0f;
                objTransform.localScale = scale;

                // テキストの位置まで移動する
                if (clearPos.x > pos.x)
                {
                    pos.x += 0.01f;
                    
                }
                if (clearPos.x < pos.x)
                {
                    pos.x -= 0.01f;
                    
                }
                if (clearPos.y > pos.y)
                {
                    pos.y += 0.01f;
                    startRot.z += 0.1f;
                }
            }

           
        }

        //--------------------------------------------------------
        // Transformを敵用する

        // 回転率を敵用する
        Vector3 rot;
        rot.x = 1.0f;
        rot.y = 1.0f;
        rot.z = startRot.z;
        objTransform.eulerAngles = rot;

        // 座標を敵用する
        objTransform.position = pos;

    }
    //============================================================
}


