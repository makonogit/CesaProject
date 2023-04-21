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
    //public Vector3 clearPos;// テキスト生成時の座標

    // テキスト関連
    bool isMoveFlg = false;// 移動開始フラグ
    int delayCnt = 0;      // 待機時間をカウント

    int rndX;
    int rndY;

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
        // このオブジェクトのTransformを取得
        Transform objTransform = this.transform;

        // 座標の指定
        Vector3 pos = objTransform.position;


        if (isMoveFlg == false)
        {

            rndX = Random.Range(-3, 3 + 1);
            rndY = Random.Range(-3, 3 + 1);

            pos.x = startPos.x + 0.01f * rndX;
            pos.y = startPos.y + 0.01f * rndY;

            isMoveFlg = true;
        }

        pos.x += 0.001f * rndX;
        pos.y += 0.001f * rndY;

        // 座標を敵用する
        objTransform.position = pos;

        Vector3 rot;
        rot.x = 1.0f;
        rot.y = 1.0f;
        rot.z = startRot.z += 0.05f;
        objTransform.eulerAngles = rot;

    }
}


