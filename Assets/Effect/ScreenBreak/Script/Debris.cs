//------------------------------------------------------------------------------
// 担当：藤原昂祐
// 内容：破片
//------------------------------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debris : MonoBehaviour
{
    //--------------------------------------------------------------------------
    // - 変数宣言 -

    // 座標関連
    Vector3 startRot;// 生成時の回転率
    Vector3 startPos;// 生成時の座標

    //============================================================
    // - 初期化処理 -

    void Start()
    {
        //-------------------------------------------------------
        // 初期位置を保存する

        Transform objTransform = this.transform;
        startRot = objTransform.eulerAngles;
        startPos = objTransform.localPosition;
    }

    //============================================================
    // - 更新処理 -

    void Update()
    {
        //--------------------------------------------------------
        // 落下時の挙動を制御する

        // 回転させる
        startRot.z += 2 * Time.deltaTime;

        // このオブジェクトのTransformを取得
        Transform objTransform = this.transform;

        // 座標の指定
        Vector3 pos = objTransform.localPosition;
        // 真ん中より右なら右に移動する
        if(pos.x > 2.0f)
        {
            pos.x += 1.0f * Time.deltaTime;
        }
        // 真ん中より左なら左に移動する
        if (pos.x < -2.0f)
        {
            pos.x -= 1.0f * Time.deltaTime;
        }
        // 最初は少し上に移動させる
        if (startPos.y > pos.y - 2.0f)
        {
         pos.y += 3.0f * Time.deltaTime;
        }

        // 回転率の指定
        Vector3 rot;
        rot.x = 1.0f;
        rot.y = 1.0f;
        rot.z = startRot.z;

        // 座標、回転率を敵用する
        objTransform.eulerAngles = rot;
        objTransform.localPosition = pos;

        // 画面外ならこのオブジェクトを消去する
        if ((pos.y < -8.0f)|| (pos.y > 8.0f))
        {
            Destroy(this.gameObject);
        }
    }
    //============================================================

}
