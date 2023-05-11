//=========================================
// 担当：藤原昂祐
// 内容：洞窟のボスの敵を降らせる攻撃
//=========================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDrop_CaveBoss : MonoBehaviour
{
    //=====================================
    // *** 変数宣言 ***
    //=====================================

    //-------------------------------------
    // *** インスタンス ***

    public static EnemyDrop_CaveBoss instance;// このクラスのインスタンス

    //-------------------------------------
    // *** 攻撃関連 ***

    [Header("降らせる敵")]
    public GameObject needleEnemy;// 降らせる敵
    [Header("降らせる敵の範囲")]
    public float dropRange = 15.0f;

    //-------------------------------------
    // *** 座標関連 ***

    Vector2 startPos;// 初期位置

    //=====================================
    // *** 初期化処理 ***
    //=====================================

    void Start()
    {
        //--------------------------------
        // *** 変数の初期化 ***

        // 初期位置を保存
        startPos = transform.position;

        // このクラスのインスタンスを生成
        if (instance == null)
        {
            instance = this;
        }
    }

    //=====================================
    // *** 敵を降らせる処理 ***
    //
    // 引数　：無し
    // 戻り値：攻撃が終了したか（true：終了、false：攻撃中）
    //=====================================

    public bool EnemyDrop()
    {
        //---------------------------------
        // *** 敵を生成 ***

        for (int i = 0; i < 3; i++)
        {
            // 敵を生成
            GameObject obj = Instantiate(needleEnemy, transform.position, Quaternion.identity);

            int rndX = Random.Range(1, 20);

            // 座標を変更
            Transform objTransform = obj.transform;
            Vector3 pos = objTransform.position;
            pos.x = startPos.x + -dropRange + (dropRange * 0.1f) * rndX;
            pos.y = transform.position.y;

            // 座標の変更を敵用する
            objTransform.position = pos;
        }

        return true;
    }
}
