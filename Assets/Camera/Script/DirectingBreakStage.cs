//---------------------------------------------------------
//担当者：二宮怜
//内容　：ステージクリア後、ステージの最初の部分から壊れていき、それを映すカメラの移動
//---------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectingBreakStage : MonoBehaviour
{
    // 変数宣言
    public bool startInit = false; // 始まって一度だけ呼び出し
    public bool startBreak = false; // 演出開始

    [Header("カメラ移動時のスピード")]public float MoveSpeed = 5.0f; // カメラ移動時のスピード
    private float timer = 0; // 演出が始まってからの経過時間
    private Transform thisTransform; // 自身の座標
    private Vector3 InitPos; // 初期位置
    private float FinalySpeed; // 計算後の最終的なスピード
    [Header("割れる時のスピード")]public float BreakSpeed = 1.1f;

    [Header("カメラ移動時に加速していくか")]public bool isAxel = true;

    private float FaderRate = 1f; // 最大値から最小値の間を変化させる

    [Header("破壊するクリスタルが横に何枚並んでいるか"),SerializeField]
    private int CrystalNum_X; // 破壊するクリスタルが横に何枚並んでいるか

    private bool BreakStage; // ステージ破壊演出が終わったらtrue

    // 外部取得
    private GameObject StageData; // 必要となるオブジェクトの親オブジェクト

    private GameObject CameraTarget_last; // カメラが移動する最終的な座標を持つオブジェクト
    private Transform Last_Transform; // 目標オブジェクトの座標

    private GameObject StagePrefab; // ステージ情報プレハブ
    private CameraZoom sc_cameraZoom; // スクリプト

    private GameObject MainCamera;
    private CameraControl2 control;
    private GameObject player;

    [Header("各ステージごとに用意されたBorderLineマテリアルをセット")]public Material Mat;

    public GameObject particle; // 壊れた破片が落ちてくるパーティクル
    private GameObject ParticleObj; // 作成したパーティクルを持つ変数

    [SerializeField, Header("割れるエフェクト")]
    private ScreenBreak _ScreenBreak;

    [SerializeField, Header("背景クリスタル")]
    private List<GameObject> Crystal;

    // Start is called before the first frame update
    void Start()
    {
        // メインカメラ取得
        MainCamera = GameObject.Find("Main Camera");
        // カメラ追従スクリプト取得
        control = MainCamera.GetComponent<CameraControl2>();
        thisTransform = GetComponent<Transform>();
        InitPos = thisTransform.position;

        player = GameObject.Find("player");

        startInit = false;
        startBreak = false;
        BreakStage = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(startInit == false)
        {
            // 目標オブジェクト取得
            CameraTarget_last = GameObject.Find("CameraTarget_Last");
            Last_Transform = CameraTarget_last.GetComponent<Transform>();
            //Debug.Log(CameraTarget_last);
            //Debug.Log(Last_Transform);

            // ステージの情報を持つオブジェクト取得
            StageData = GameObject.Find("StageData");
            //Debug.Log(StageData);

            // ステージプレハブ　例)Stage1-1,Stage2-1
            StagePrefab = StageData.transform.GetChild(0).gameObject;
            //Debug.Log(StagePrefab);

            // カメラズームスクリプト取得
            sc_cameraZoom = StagePrefab.GetComponent<CameraZoom>();
            //Debug.Log(sc_cameraZoom);

            // 初期化
            Mat.SetFloat("_Fader", 1f);  // 初期値
            Mat.SetFloat("_Width", -9f); // 固定

            startInit = true;
        }

        if(startBreak == true && BreakStage == false)
        {
            // カメラ追従ターゲットを変更
            if(control.GetTarget() != this.gameObject)
            {
                // カメラ追従ターゲットをステージの最初の方にある自身に設定
                control.SetTarget(this.gameObject);
                //Debug.Log(control.GetTarget());

                // このスクリプトを持つオブジェクトの子オブジェクトに
               // ParticleObj = Instantiate(particle, this.gameObject.transform.GetChild(0).gameObject.transform);
            }

            // カメラ追従ターゲットの位置を目標地点まで加速しながら移動させる
            if (thisTransform.position.x < Last_Transform.position.x)
            {
                // 座標を目標オブジェクトの方まで移動させる
                // 後になればなるほど速くなる
                // 加速するなら
                if (isAxel == true)
                {
                    FinalySpeed = MoveSpeed + MoveSpeed * timer / 2f;
                }
                // 等速で移動させる
                else
                {
                    FinalySpeed = MoveSpeed;
                }

               thisTransform.Translate(FinalySpeed * Time.deltaTime, 0f, 0f);
            }
            else
            {
                thisTransform.position = new Vector3(Last_Transform.position.x,
                    thisTransform.position.y, 
                    thisTransform.position.z);
            }

            // 背景クリスタルを消す割合
            FaderRate -= Time.deltaTime * BreakSpeed;
            // 背景クリスタルマテリアルの閾値を最大値から最小値に下げる
            //Mat.SetFloat("_Fader", FaderRate);

            if (FaderRate <= 1f - CrystalNum_X * 2f)
            {
                // ステージ破壊完了フラグ
                BreakStage = true;
                Debug.Log("ステージ破壊完了");

                for(int i = 0; i < Crystal.Count; i++)
                {
                    Destroy(Crystal[i]);
                }

                // Destroy(ParticleObj);
                _ScreenBreak.enabled = true;
            }

            // カウント
            timer += Time.deltaTime;
        }
        else
        {

            thisTransform.position = InitPos;
            control.SetTarget(player);
            timer = 0f;

            FaderRate = 1f;
            Mat.SetFloat("_Fader", FaderRate);
        }
    }

    public bool GetBreakStage()
    {
        return BreakStage;
    }

    public void StartBreak()
    {
        startBreak = true;
    }
}
