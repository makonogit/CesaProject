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

    // 以下定点カメラ用変数
    private int ScreenNum = 0; // 左から何個目の定点カメラか　配列の添え字に利用する　添え字例→(2 * ScreenNum + 1)

    // カメラの移動範囲の制限の素の名前が入る
    private string[] CameraInfoName = 
        { "LeftDown1", "RightUp1",
          "LeftDown2", "RightUp2"}; 

    // 外部取得
    private GameObject target; // カメラが追従するゲームオブジェクトを保持
    //private Vector3 offset; // プレイヤーとカメラの位置関係を保持
    private Transform targetTransform; // ターゲットの座標
    private Transform thisTransform; // カメラの現在座標を持つ

    Camera Cam; // カメラを取得する変数

    // 移動範囲の制限の素
    GameObject LeftDown; 
    GameObject RightUp;

    private GameObject CameraArea;
    private CameraZoom zoom;

    // 以下定点カメラ用外部取得

    //------------------------------------------------------------------------------------------------------
    //* 初期化処理 *
    //------------------------------------------------------------------------------------------------------
    void Start()
    {
        // 最初の追従ターゲットはプレイヤー
        target = GameObject.Find("player");

        // ターゲットが設定される度に取得しなおす
        targetTransform = target.GetComponent<Transform>();

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
        //offset = thisTransform.position - target.transform.position;

        // 移動範囲の素となるゲームオブジェクトを探す
        LeftDown = GameObject.Find(CameraInfoName[2*ScreenNum]);    // この場合LeftDown1
        RightUp = GameObject.Find(CameraInfoName[2*ScreenNum + 1]); // この場合RightUp1
    }
    //------------------------------------------------------------------------------------------------------

    //------------------------------------------------------------------------------------------------------
    // *更新処理の後にされる処理*
    //------------------------------------------------------------------------------------------------------
    void LateUpdate()   // すべてのゲームオブジェクトのUpdateメソッドが呼び出された後に実行される関数
    {
        //------------------------------------------------------------------------------------------------------
        // エリア内にいるかいないかで追従ターゲットを変える
        if (zoom.InArea == true)
        {
            // エリア内でターゲットがプレイヤーなら
            if(target.name == "player")
            {
                // ターゲットを変更
                target = GameObject.Find("GoalArea");
                targetTransform = target.transform;
            }
        }
        else
        {
            // エリア外でターゲットがゴールエリアなら
            if (target.name == "GoalArea")
            {
                // ターゲットを変更
                target = GameObject.Find("player");
                targetTransform = target.transform;
            }
        }

        //------------------------------------------------------------------------------------------------------
        // カメラの追従 ＆　カメラ切り替え

        // 移動範囲制限に必要な四頂点の座標
        LeftPos = LeftDown.transform.position.x;
        DownPos = LeftDown.transform.position.y;
        RightPos = RightUp.transform.position.x;
        UpPos = RightUp.transform.position.y;

        // ターゲットがプレイヤーの時
        if (target.name == "player")
        {
            // プレイヤーがカメラ端からでたら移しているカメラ切り替え
            // （正確にはカメラの移動制限の範囲を変えているだけ

            int temp = ScreenNum; // 条件判定用

            // プレイヤーが見えている範囲の左端から出たとき
            if (targetTransform.position.x < LeftPos)
            {
                // カメラ切り替え
                ScreenNum--;
            }
            else if (targetTransform.position.x > RightPos)
            {
                // カメラ切り替え
                ScreenNum++;
            }

            // カメラが切り替わっていたら
            if(temp != ScreenNum)
            {
                // 移動制限の範囲も切り替え
                // 移動範囲の素となるゲームオブジェクトを探す
                LeftDown = GameObject.Find(CameraInfoName[2 * ScreenNum]);
                RightUp = GameObject.Find(CameraInfoName[2 * ScreenNum + 1]);
            }
        }

        // プレイヤーの現在位置から新しいカメラの位置を作成
        Vector3 vector = targetTransform.position;

        //------------------------------------------------------------------------------------------------------
        // 左端より左に行こうとしたらカメラのx座標固定
        if (vector.x - Cam.orthographicSize * 2 <= LeftPos)
        {
            vector.x = LeftPos + Cam.orthographicSize * 2;
        }
        // 右端より右に行こうとしたらカメラのx座標固定
        else if (vector.x + Cam.orthographicSize * 2 >= RightPos)
        {
            vector.x = RightPos - Cam.orthographicSize * 2;
        }

        // 下端より下に行こうとしたらカメラのy座標固定
        if (vector.y - Cam.orthographicSize <= DownPos)
        {
            vector.y = DownPos + Cam.orthographicSize;
        }
        // 上端より上に行こうとしたらカメラのy座標固定
        else if (vector.y + Cam.orthographicSize >= UpPos)
        {
            vector.y = UpPos - Cam.orthographicSize;
        }

        // 縦方向は固定
        // vector.y = transform.position.y;
        // カメラの位置を移動
        thisTransform.position = new Vector3(vector.x,vector.y,thisTransform.position.z);
    }
}
