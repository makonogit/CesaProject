//---------------------------------------------------------
//担当者：二宮怜
//内容　：カメラのズーム
//---------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    //---------------------------------------------------------
    // - 変数宣言 -
    private string playerTag = "Player"; // プレイヤーのタグ

    //[System.NonSerialized]
    public bool InArea = false; // エリア内にいるか

    [Header("一秒あたりの変化量")]
    public float ChangeVolume = 0.1f; // カメラサイズの変化量

    [Header("ズーム後の描画サイズ")]
    public float ZoomCameraSize = 2.5f;

    [Header("通常時の描画サイズ")]
    public float DefaultCameraSize = 5.0f; //通常時のカメラサイズ

    [Header("現在のカメラサイズ")]
    public float NowCameraSize = 5.0f; // 現在のカメラサイズ

    // 外部取得
    private GameObject Camera; // ゲームオブジェクトMainCamera
    Camera Cam; // カメラスクリプトを取得

    private GameObject goal; // ゴールオブジェクト
    private Transform goalTransform;   // ゴールの座標
    private Transform cameraTransform; // カメラの座標

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // プレイヤーとの当たり判定のみ考慮
        if (collision.gameObject.tag == playerTag)
        {
            // 状態→エリア内にいる
            InArea = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // プレイヤーとの当たり判定のみ考慮
        if (collision.gameObject.tag == playerTag)
        {
            // 状態→エリア内にいない
            InArea = false;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // MainCamera探す
        Camera = GameObject.Find("Main Camera");
        // カメラスクリプトを取得
        Cam = Camera.GetComponent<Camera>();

        // Goal探す
        goal = GameObject.Find("Goal");

        // Goalの座標取得
        goalTransform = goal.GetComponent<Transform>();
        cameraTransform = Camera.GetComponent<Transform>();
        //Debug.Log(Camera);
    }

    // Update is called once per frame
    void Update()
    {
        // ゴールイベントの範囲内にいれば
        if (InArea == true)
        {
            // ズーム後のカメラ描画サイズになるまで徐々にズームインしていく
            if (NowCameraSize > ZoomCameraSize)
            {
                // 描画サイズ計算
                NowCameraSize -= ChangeVolume * Time.deltaTime;
            }
            else
            {
                NowCameraSize = ZoomCameraSize;
            }

            // カメラの位置はエリアの中心に固定
            cameraTransform = goalTransform;
        }
        else
        {
            // デフォルトのカメラ描画サイズになるまで徐々にズームアウトしていく
            if (NowCameraSize < DefaultCameraSize)
            {
                // 描画サイズ計算
                NowCameraSize += ChangeVolume * Time.deltaTime;
            }
            else
            {
                NowCameraSize = DefaultCameraSize;
            }
        }

        //カメラサイズ変更
        Cam.orthographicSize = NowCameraSize;
    }
}
