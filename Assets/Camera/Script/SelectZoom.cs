//-------------------------------------
//　担当：菅眞心
//　内容：セレクト画面のカメラズーム
//-------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectZoom : MonoBehaviour
{
    //---------------------------------------------------------
    // - 変数宣言 -

    [SerializeField,Header("一秒あたりの変化量")]
    private float ChangeVolume = 0.1f; // カメラサイズの変化量

    [Header("ズーム後の描画サイズ")]
    public float ZoomCameraSize = 2.5f;

    [SerializeField, Header("通常時の描画サイズ")]
    private float DefaultCameraSize; //通常時のカメラサイズ

    [Header("現在のカメラサイズ")]
    public float NowCameraSize = 5.0f; // 現在のカメラサイズ

    // 外部取得
    private GameObject Camera; // ゲームオブジェクトMainCamera
    Camera Cam; // カメラスクリプトを取得

    //　カメラの移動制限用Collider
    private EdgeCollider2D HorizonLimit;    //水平方向
    private EdgeCollider2D VerticalLimit;   //垂直方向

    private Transform cameraTransform; // カメラの座標

    private GameObject player;      // プレイヤー
    private Transform playertans;   // プレイヤーのTransform

    public bool Select = false;     // ステージを選択しているか
    public bool ZoomEnd = false;

    // Start is called before the first frame update
    void Start()
    {
        // MainCamera探す
        Camera = GameObject.Find("MainCamera");
        // カメラスクリプトを取得
        Cam = Camera.GetComponent<Camera>();

        cameraTransform = Camera.transform;

        player = GameObject.Find("Player(SelectScene)");
        playertans = player.transform;

        //　初期カメラのサイズを取得
        DefaultCameraSize = Cam.orthographicSize;

        //　カメラの移動制限用EdgeColliderを取得
        HorizonLimit = transform.GetChild(0).gameObject.GetComponent<EdgeCollider2D>();
        VerticalLimit = transform.GetChild(1).gameObject.GetComponent<EdgeCollider2D>();

    }

    // Update is called once per frame
    void Update()
    {
        // ステージ選択したらズーム
        if (Select)
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
                ZoomEnd = true;
            }

            // カメラの位置はプレイヤーの中心に固定
            cameraTransform.position = new Vector3(playertans.position.x, playertans.position.y,-1.0f);

            //カメラサイズ変更
            Cam.orthographicSize = NowCameraSize;


            //----------------------------------------------------------------------
            // 画面端の座標を取得
            float Max_x = HorizonLimit.points[1].x - Cam.orthographicSize * 1.78f;
            float Min_x = HorizonLimit.points[0].x + Cam.orthographicSize * 1.78f;
            float Max_y = VerticalLimit.points[0].y - Cam.orthographicSize;
            float Min_y = VerticalLimit.points[1].y + Cam.orthographicSize;


            Vector2 CameraPos = cameraTransform.position;

            // カメラの移動制限
            CameraPos.x = Mathf.Clamp(CameraPos.x, Min_x, Max_x);
            CameraPos.y = Mathf.Clamp(CameraPos.y, Min_y, Max_y);

            //　カメラの位置更新
            cameraTransform.position = new Vector3(CameraPos.x, CameraPos.y, -1.0f);


        }

    }
}
