//---------------------------------------
//担当者：尾花真理子(追加二宮)
//内容：カメラの追従
//追加：カメラの移動範囲制限
//---------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    //---------------------------------------------------------
    // - 変数宣言 -

    public float LeftPos;  // 左端
    public float RightPos; // 右端
    public float DownPos;  // 下端
    public float UpPos;    // 上端

    // 外部取得
    private GameObject target; // プレイヤーのゲームオブジェクトを保持
    private Vector3 offset; // プレイヤーとカメラの位置関係を保持
    private Transform thisTransform; // カメラの現在座標を持つ

    Camera Cam; // カメラを取得する変数

    // 移動範囲の制限の素
    GameObject LeftDown; 
    GameObject RightUp;

    private GameObject CameraArea;
    private CameraZoom zoom;

    //------------------------------------------------------------------------------------------------------
    //* 初期化処理 *
    //------------------------------------------------------------------------------------------------------
    void Start()
    {
        // 最初の追従ターゲットはプレイヤー
        target = GameObject.Find("player");

        // ズームエリアのオブジェクト探す
        CameraArea = GameObject.Find("GoalArea");
        // ズームスクリプト取得
        zoom = CameraArea.GetComponent<CameraZoom>();

        //-----------------------------------------
        // カメラのサイズをとるために必要
        Cam = GetComponent<Camera>();

        //-----------------------------------------
        // カメラの現在座標
        thisTransform = GetComponent<Transform>();

        // ゲームスタート時でのプレイヤーとカメラの位置関係を記憶
        offset = thisTransform.position - target.transform.position;

        // 移動範囲の素となるゲームオブジェクトを探す
        LeftDown = GameObject.Find("LeftDown");
        RightUp = GameObject.Find("RightUp");
    }
    //------------------------------------------------------------------------------------------------------

    //------------------------------------------------------------------------------------------------------
    // *更新処理の後にされる処理*
    //------------------------------------------------------------------------------------------------------
    void LateUpdate()   // すべてのゲームオブジェクトのUpdateメソッドが呼び出された後に実行される関数
    {
        // エリア内にいるかいないかで追従ターゲットを変える
        if(zoom.InArea == true)
        {
            // エリア内でターゲットがプレイヤーなら
            if(target.name == "player")
            {
                // ターゲットを変更
                target = GameObject.Find("GoalArea");
            }
        }
        else
        {
            // エリア外でターゲットがゴールエリアなら
            if (target.name == "GoalArea")
            {
                // ターゲットを変更
                target = GameObject.Find("player");
            }
        }

        // プレイヤーの現在位置から新しいカメラの位置を作成
        Vector3 vector = target.transform.position + offset;

        // 移動範囲制限に必要な四頂点の座標
        LeftPos = LeftDown.transform.position.x + Cam.orthographicSize * 2;
        DownPos = LeftDown.transform.position.y + Cam.orthographicSize;
        RightPos = RightUp.transform.position.x - Cam.orthographicSize * 2;
        UpPos = RightUp.transform.position.y - Cam.orthographicSize;

        //------------------------------------------------------------------------------------------------------
        // 左端より左に行こうとしたらカメラのx座標固定
        if (vector.x <= LeftPos)
        {
            vector.x = LeftPos;
        }
        // 右端より右に行こうとしたらカメラのx座標固定
        else if (vector.x >= RightPos)
        {
            vector.x = RightPos;
        }

        // 下端より下に行こうとしたらカメラのy座標固定
        if (vector.y <= DownPos)
        {
            vector.y = DownPos;
        }
        // 上端より上に行こうとしたらカメラのy座標固定
        else if (vector.y >= UpPos)
        {
            vector.y = UpPos;
        }

        // 縦方向は固定
        // vector.y = transform.position.y;
        // カメラの位置を移動
        thisTransform.position = vector;

    }


}
