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


    private GameObject CameraArea;          //カメラの追従エリア
    private PolygonCollider2D AreaCollider; //追従エリアのコライダー

    private AreaManager _AreaManager;       //エリア管理オブジェクト

    private GameObject ZoomArea;            //カメラのズームエリア
    private CameraZoom zoom;                //カメラズーム用スクリプト

    private Camera MainCam;                 //メインカメラ


    // Start is called before the first frame update
    void Start()
    {
        // プレイヤーの情報取得
        Target = GameObject.Find("player");
        TargetTrans = Target.GetComponent<Transform>();

        // カメラの追従エリアの情報を取得
        CameraArea = GameObject.Find("CameraArea");
        AreaCollider = CameraArea.GetComponent<PolygonCollider2D>();
        _AreaManager = CameraArea.GetComponent<AreaManager>();

        // カメラズームエリアの情報を取得
        ZoomArea = GameObject.Find("GoalArea");
        zoom = ZoomArea.GetComponent<CameraZoom>();

        // カメラの情報を取得
        MainCam = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        // ズームエリアにいたら追従ターゲットを変更する
        if (zoom.InArea)
        {
            if(Target.name == "player")
            {
                // ターゲットを変更
                Target = GameObject.Find("GoalArea");
                TargetTrans = Target.transform;
            }
        }
        else
        {
            // エリア外でターゲットがゴールエリアなら
            if (Target.name == "GoalArea")
            {
                // ターゲットを変更
                Target = GameObject.Find("player");
                TargetTrans = Target.transform;
            }

        }

        // 現在の座標を取得
        Vector3 NowPos = TargetTrans.position;

        // カメラの座標をターゲットを基に更新
        transform.position = new Vector3(NowPos.x, NowPos.y, transform.position.z);

        //----------------------------------------------------------------------
        // エリアの情報からコライダーをリサイズ
        //AreaCollider.points[0].Set(AreaCollider.points[1].x + _AreaManager.AreaSize, AreaCollider.points[0].y);
        //AreaCollider.points[3].Set(AreaCollider.points[1].x + _AreaManager.AreaSize, AreaCollider.points[3].y);
        //Debug.Log(_AreaManager.AreaSize);

        //----------------------------------------------------------------------
        // コライダーの情報から画面端の座標を取得(Xだけなんかずれあるから1.77f)
        float Max_x = (AreaCollider.points[0].x + AreaCollider.offset.x) - MainCam.orthographicSize * 1.77f;
        float Min_x = (AreaCollider.points[1].x + AreaCollider.offset.x) + MainCam.orthographicSize * 1.77f;
        float Max_y = (AreaCollider.points[1].y + AreaCollider.offset.y) - MainCam.orthographicSize;
        float Min_y = (AreaCollider.points[2].y + AreaCollider.offset.y) + MainCam.orthographicSize;

        // ステージのPorigonColliderを基に移動制限
        NowPos.x = Mathf.Clamp(NowPos.x, Min_x, Max_x);
        NowPos.y = Mathf.Clamp(NowPos.y, Min_y, Max_y);

        //　カメラの座標を更新
        transform.position = new Vector3(NowPos.x, NowPos.y, transform.position.z);

    }
}
