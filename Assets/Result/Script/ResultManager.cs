//-------------------------------------------
// 担当者：藤原昂祐
// 内容　：PlayResult関数でリザルト演出をする
//-------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResultManager : MonoBehaviour
{
    //=============================================
    // *** 変数宣言 ***
    //=============================================

    //---------------------------------------------
    // 汎用
    //---------------------------------------------

    [Header("[デバックのON、OFF]")]
    public bool debugging = false;

    int flameCnt;    // フレームカウント用

    //---------------------------------------------
    // 状態関連
    //---------------------------------------------

    public enum StateID// 状態ID
    {
        NULL,          // 状態なし
        RESULT_INIT,   // リザルト演出準備
        RESULT_UPDATE, // リザルト演出中
        RESULT_END,    // リザルト演出終了
    }
    StateID oldState = StateID.NULL; // 前の状態
    StateID nowState = StateID.NULL; // 現在の状態
    StateID nextState = StateID.NULL;// 次の状態

    //---------------------------------------------
    // 破片関連
    //---------------------------------------------

    [Header("[破片生成の設定]")]
    [Header("・生成開始位置（このオブジェクトが基準点）")]
    public Vector2 start = new Vector2(6.0f,2.0f);
    [Header("・生成する間隔（距離）")]
    public Vector2 distance = new Vector2(0.02f, 0.5f);
    [Header("・破片の大きさ")]
    public float size = 0.02f;
    [Header("・何フレーム毎に生成するか")]
    public float flame = 2;
    [Header("・破片の移動速度")]
    public Vector2 move_speed = new Vector2(0.002f, 0.002f);
    [Header("・集まる時の破片の加速度")]
    public float acceleration = 0.0002f;
    [Header("・破片のオブジェクト")]
    public GameObject[] debris = new GameObject[3];// 破片用オブジェクト
   
    bool isMoveFlg;                         // 破片の集まるフラグ

    //---------------------------------------------
    // クリアテキスト関連
    //---------------------------------------------

    [Header("[クリアテキストの設定]")]
    [Header("・テキストの出現方法（0通常、1勢い）")]
    int type = 1;// テキストの出現方法

    // 挙動制御用変数
    Vector3 textScale;// 初期サイズ
    float scaleX;     // 加算する大きさ
    bool flg;

    //---------------------------------------------
    // リザルト関連
    //---------------------------------------------

    [Header("[リザルトの設定]")]
    [Header("・演出終了後の待機フレーム数")]
    public int standbyTim = 1000;// 演出終了後の待機フレーム数

    //=============================================
    // *** 初期化処理 ***
    //=============================================

    void Start()
    {
        //-----------------------------------------
        // デバックがONなら演習を開始
        //-----------------------------------------

        if (debugging == true)
          PlayResult();

        //-----------------------------------------
        // クリアテキストの大きさを保存する
        //-----------------------------------------

        textScale = this.transform.localScale;

        //----------------------------------------------
        // テキストの横幅を0にする
        //----------------------------------------------

        Transform textTransform = this.transform;
        Vector3 scale = textTransform.localScale;
        scale.x = 0.0f;
        textTransform.localScale = scale;
    }

    //=============================================
    // *** 更新処理 ***
    //=============================================

    void Update()
    {
        //-----------------------------------------
        // 現在の状態によって処理を分岐
        //-----------------------------------------

        if (nextState != StateID.NULL)
        {
            oldState = nowState;
            nowState = nextState;
            nextState = StateID.NULL;
        }

        switch (nowState)
        {

            // リザルト演出準備
            case StateID.RESULT_INIT:
                ResultInit();
                break;
            // リザルト演出中
            case StateID.RESULT_UPDATE:
                ResultUpdate();
                break;
            // リザルト演出終了
            case StateID.RESULT_END:
                ResultEnd();
                break;
        }
    }

    //=============================================
    // *** リザルト演出準備　処理 ***
    //=============================================

    void ResultInit()
    {
        //-----------------------------------------
        // 変更の初期化
        //-----------------------------------------

        flameCnt = 0;
        isMoveFlg = false;

        flg = false;
        scaleX = 0.0f;

        //----------------------------------------------
        // テキストの横幅を0にする
        //----------------------------------------------

        Transform textTransform = this.transform;
        Vector3 scale = textTransform.localScale;
        scale.x = 0.0f;
        textTransform.localScale = scale;

        //----------------------------------------------
        // 状態をRESULT_UPDATEに変更
        //----------------------------------------------

        nextState = StateID.RESULT_UPDATE;
    }

    //=============================================
    // *** リザルト演出　初期化処理 ***
    //=============================================

    void ResultUpdate()
    {
        //----------------------------------------------
        // 現在のフレーム数を更新
        //----------------------------------------------

        //flameCnt++;

        //----------------------------------------------
        // 破片を生成
        //----------------------------------------------

        if (start.x * 2 > distance.x * flameCnt)
        {
            //if(flameCnt % flame == 0)
            //{
            //    // ランダムに形、座標、大きさ、回転率を取得する
            //    int rndDebris = Random.Range(0, 3);
            //    int rndX = Random.Range(0, 10);
            //    int rndY = Random.Range(0, 10);
            //    int rndSizeX = Random.Range(1, 10);
            //    int rndSizeY = Random.Range(1, 10);
            //    int rndRot = Random.Range(1, 360);

            //    // 破片を生成
            //    GameObject debrisRist = Instantiate(debris[rndDebris], transform.position, Quaternion.identity);

            //    // 生成した破片のtransformを取得
            //    Transform objTransform = debrisRist.transform;

            //    // 座標を変更
            //    Vector3 pos0;
            //    pos0.x = objTransform.position.x + start.x - distance.x * flameCnt;
            //    pos0.y = objTransform.position.y + start.y - distance.y * rndY;
            //    pos0.z = -0.9f;

            //    // 大きさを変更
            //    Vector3 scale;
            //    scale.x = size * rndSizeX;
            //    scale.y = size * rndSizeY;
            //    scale.z = 1.0f;

            //    // 回転を変更
            //    Vector3 rot;
            //    rot.x = 1.0f;
            //    rot.y = 1.0f;
            //    rot.z = 1.0f * rndRot;

            //    // 変更を敵用する
            //    objTransform.position = pos0;   // 座標
            //    objTransform.localScale = scale;// 大きさ
            //    objTransform.eulerAngles = rot; // 回転

            //    // 破片の変数を初期化
            //    debrisRist.GetComponent<ResultDebris>().move_speed = move_speed;
            //    debrisRist.GetComponent<ResultDebris>().acceleration = acceleration;
            //}


        }

        //----------------------------------------------
        // 生成が終了したらRESULT_ENDにする
        //----------------------------------------------

        else
        {
            nextState = StateID.RESULT_END;
            flameCnt = 0;
        }

        //----------------------------------------------
        // クリアテキストの挙動を制御する
        //----------------------------------------------

        if ( start.x > flameCnt)
        {
            // 横幅をtextScaleまで大きくする
            Transform textTransform = this.transform;  
            Vector3 scale = textTransform.localScale;

            // テキストの出現方法によって処理を分岐
            switch (type)
            {
                case 0:
                    if (textTransform.localScale.x < textScale.x)
                    {
                        scale.x += textScale.x * 0.01f;
                    }
                    break;

                case 1:

                    if (textTransform.localScale.x < textScale.x + 2.0f)
                    {
                        scale.x += 0.03f + 0.00002f * flameCnt;
                    }
                    else
                    {
                        flg = true;
                    }

                    if (flg == true)
                    {
                        if (textTransform.localScale.x > textScale.x)
                        {
                            scale.x -= 0.06f + 0.00002f * flameCnt;
                        }
                        else
                        {
                            nextState = StateID.RESULT_END;
                        }
                    }
                    break;

                default:
                    if (textTransform.localScale.x < textScale.x)
                    {
                        scale.x += textScale.x * 0.01f;
                    }
                    break;
            }

            textTransform.localScale = scale;
        }
    }

    //=============================================
    // *** リザルト演出　終了処理 ***
    //=============================================

    void ResultEnd()
    {

        //----------------------------------------------
        // 現在のフレーム数を更新
        //----------------------------------------------

        flameCnt++;

        //----------------------------------------------
        // 待機時間が経過したらセレクト画面に遷移
        //----------------------------------------------

        if (standbyTim / 2 < flameCnt)
        {
            isMoveFlg = true;
        }

        if (standbyTim < flameCnt)
        {
            SceneManager.LoadScene("newSelectScene");
            nowState = StateID.NULL;
        }
    }

    //=============================================
    // *** リザルト演出を開始する関数 ***
    //
    // この関数を呼び出すと演出が開始する
    //=============================================

    public void PlayResult()
    {
        //----------------------------------------------
        // 次の状態をRESULT_INITにセット
        //----------------------------------------------
        if (nowState == StateID.NULL)
        {
            nextState = StateID.RESULT_INIT;
        }
    
    }

    public bool GetMoveFlg()
    {
        return isMoveFlg;
    }
}
