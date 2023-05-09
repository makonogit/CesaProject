//-------------------------------
//担当：菅眞心
//内容：カメラの追従・移動制限
//------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl2 : MonoBehaviour
{
    [SerializeField, Header("追従ターゲット")]
    private GameObject Target;

    [SerializeField,Header("追従ターゲットの座標")]
    private Transform TargetTrans;

    private CrackAutoMove _AutoMove;        // ひびの移動スクリプト

    private GameObject CameraArea;          // カメラの追従エリア
    private PolygonCollider2D AreaCollider; // 追従エリアのコライダー

    private Vector2[] NextAreaPos;          // 次のエリアの座標

    private AreaManager _AreaManager;       // エリア管理オブジェクト
    private int NowAreaNum;                 // 現在のエリア番号
    private bool AreaMove = false;          // エリアの移動イベント

    [Header("カメラの移動スピード")]
    public float CameraMoveSpeed;
    float NowMax_x;                         // 現在のカメラの右端

    private GameObject ZoomArea;            // カメラのズームエリア
    private CameraZoom zoom;                // カメラズーム用スクリプト

    private Camera MainCam;                 // メインカメラ


    // Start is called before the first frame update
    void Start()
    {
        // プレイヤーの情報取得
        Target = GameObject.Find("player");
        TargetTrans = Target.GetComponent<Transform>();

        // ひびの移動スクリプトを取得
        _AutoMove = Target.GetComponent<CrackAutoMove>();

        // カメラの追従エリアの情報を取得
        CameraArea = GameObject.Find("CameraArea");
        AreaCollider = CameraArea.GetComponent<PolygonCollider2D>();
        _AreaManager = CameraArea.GetComponent<AreaManager>();

        // カメラズームエリアの情報を取得
        //ZoomArea = GameObject.Find("GoalArea");
        //zoom = ZoomArea.GetComponent<CameraZoom>();

        // カメラの情報を取得
        MainCam = GetComponent<Camera>();

        // サイズを確保しておく
        NextAreaPos = new Vector2[4];

        // エリアマネージャーからエリアのサイズを計算
        Vector2[] points = AreaCollider.points;
        points[0].x = points[1].x + _AreaManager.AreaSize;
        points[3].x = points[1].x + _AreaManager.AreaSize;
        AreaCollider.SetPath(0, points);

        NowMax_x = points[0].x; // 現在のカメラ右端を設定

        NowAreaNum = 1;         // 最初のエリアを指定

    }

    private void LateUpdate()
    {
        // ズームエリアにいたら追従ターゲットを変更する
        //if (zoom.InArea)
        //{
        //    if (Target.name == "player")
        //    {
        //        // ターゲットを変更
        //        Target = GameObject.Find("GoalArea");
        //        TargetTrans = Target.transform;
        //    }
        //}
        //else
        //{
        //    // エリア外でターゲットがゴールエリアなら
        //    if (Target.name == "GoalArea")
        //    {
        //        // ターゲットを変更
        //        Target = GameObject.Find("player");
        //        TargetTrans = Target.transform;
        //    }

        //}

       
        // 現在の座標を取得
        Vector3 NowPos = new Vector3(TargetTrans.position.x, TargetTrans.position.y,transform.position.z);


        // ひびの移動中はカメラの追従を緩やかにする
        if (_AutoMove.movestate == CrackAutoMove.MoveState.CrackMove)
        {
            transform.position = Vector3.Lerp(transform.position, NowPos, 1.0f * Time.deltaTime);
        }
        else
        {
            // カメラの座標をターゲットを基に更新
            transform.position = new Vector3(NowPos.x, NowPos.y, transform.position.z);
        }

        //----------------------------------------------------------------------
        // エリアの情報からコライダーをリサイズ
        //Vector2[] points = AreaCollider.points;
        //points[0].x = points[1].x + _AreaManager.AreaSize;
        //points[3].x = points[1].x + _AreaManager.AreaSize;
        //AreaCollider.SetPath(0, points);

        //----------------------------------------------------------------------
        // コライダーの情報から画面端の座標を取得(Xだけなんかずれあるから1.77f)
        float Max_x = (AreaCollider.points[0].x + AreaCollider.offset.x) - MainCam.orthographicSize * 1.65f;
        float Min_x = (AreaCollider.points[1].x + AreaCollider.offset.x) + MainCam.orthographicSize * 1.65f;
        float Max_y = (AreaCollider.points[1].y + AreaCollider.offset.y) - MainCam.orthographicSize;
        float Min_y = (AreaCollider.points[2].y + AreaCollider.offset.y) + MainCam.orthographicSize;

        // ステージのPorigonColliderを基に移動制限
        NowPos.x = Mathf.Clamp(NowPos.x, Min_x, Max_x);
        NowPos.y = Mathf.Clamp(NowPos.y, Min_y, Max_y);

        //----------------------------------------------
        //プレイヤーがエリア外に出たら次のエリアを指定
        if (TargetTrans.position.x > AreaCollider.points[0].x + AreaCollider.offset.x)
        {
             if (!AreaMove)
            {
                NextAreaPos[0].x = AreaCollider.points[0].x + _AreaManager.AreaSize / 5;
                NextAreaPos[3].x = AreaCollider.points[0].x + _AreaManager.AreaSize / 5;
                NextAreaPos[1].x = AreaCollider.points[1].x + _AreaManager.AreaSize + 2.0f;
                NextAreaPos[2].x = AreaCollider.points[2].x + _AreaManager.AreaSize + 2.0f;
                NowAreaNum++;
                AreaMove = true;
            }
        }
        else
        {
            //AreaMove = false;
        }

        if (AreaMove)
        {
            Vector2[] points = AreaCollider.points;

            points[1].x = NextAreaPos[1].x;
            points[2].x = NextAreaPos[2].x;

            //次のエリアに到達するまで右端座標を更新
            if (NowMax_x <= NextAreaPos[0].x)
            {
                NowMax_x += CameraMoveSpeed * Time.deltaTime;
                points[0].x = NowMax_x;
                points[3].x = NowMax_x;
                AreaCollider.SetPath(0, points);
            }
            else
            {
                //到達したら移動を終了
                AreaMove = false;
            }
        }


        if (_AutoMove.movestate == CrackAutoMove.MoveState.CrackMove)
        {
            transform.position = Vector3.Lerp(transform.position, NowPos, 1.0f * Time.deltaTime);
        }
        else
        {
            //　カメラの座標を更新
            transform.position = new Vector3(NowPos.x, NowPos.y, transform.position.z);
        }
           
    }

    // 追従するターゲットを設定する
    public void SetTarget(GameObject _obj)
    {
        Target = _obj;
        TargetTrans = _obj.GetComponent<Transform>();
    }

    public GameObject GetTarget()
    {
        return Target;
    }
}
