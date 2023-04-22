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

    int flameCnt;    // フレームカウント用
    float screenWide;// 画面の横幅

    //---------------------------------------------
    // 状態関連
    //---------------------------------------------

    enum StateID// 状態ID
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

    [Header("破片のオブジェクト")]
    public GameObject[] debris = new GameObject[3];// 破片用オブジェクト

    //---------------------------------------------
    // クリアテキスト関連
    //---------------------------------------------

    Vector3 textScale;// 初期サイズ

    //---------------------------------------------
    // リザルト関連
    //---------------------------------------------

    [Header("演出終了後の待機フレーム数")]
    public int standbyTim = 360;// 演出終了後の待機フレーム数

    //=============================================
    // *** 初期化処理 ***
    //=============================================

    void Start()
    {
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

        //-----------------------------------------
        // 画面幅をワールド座標用に変換する
        //-----------------------------------------

        screenWide = 0.03f * Screen.width;

        //PlayResult();
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

        //----------------------------------------------
        // 状態をRESULT_UPDATEに変更
        //----------------------------------------------

        nextState = StateID.RESULT_UPDATE;
    }

    //=============================================
    // *** リザルト演出中　処理 ***
    //=============================================

    void ResultUpdate()
    {
        //----------------------------------------------
        // 現在のフレーム数を更新
        //----------------------------------------------

        flameCnt++;

        //----------------------------------------------
        // 破片を画面の端から端まで生成
        //----------------------------------------------

        if (screenWide / 2 > 0.1f * flameCnt)
        {
            // ランダムに形、座標、大きさ、回転率を取得する
            int rndDebris = Random.Range(0, 3);
            int rndX = Random.Range(0, 21);
            int rndY = Random.Range(0, 21);
            int rndSizeX = Random.Range(1, 10);
            int rndSizeY = Random.Range(1, 10);
            int rndRot = Random.Range(1, 360);

            // 破片を生成
            GameObject debrisRist = Instantiate(debris[rndDebris], transform.position, Quaternion.identity);

            // 生成した破片をのtransformを取得
            Transform objTransform = debrisRist.transform;

            // 座標を変更
            Vector3 pos0 = objTransform.position;
            pos0.x = objTransform.position.x + screenWide / 4 - 0.1f * flameCnt;
            pos0.y = objTransform.position.y + 1.0f - 3.0f + 0.2f * rndY;
            pos0.z = 1.0f;//-0.9f;

            // 大きさを変更
            Vector3 scale;
            scale.x = 0.05f * rndSizeX;
            scale.y = 0.05f * rndSizeY;
            scale.z = 1.0f;

            // 回転を変更
            Vector3 rot;
            rot.x = 1.0f;
            rot.y = 1.0f;
            rot.z = 1.0f * rndRot;

            // 変更を敵用する
            objTransform.position = pos0;   // 座標
            objTransform.localScale = scale;// 大きさ
            objTransform.eulerAngles = rot; // 回転
        }

        //----------------------------------------------
        // 端まで行ったら状態をRESULT_ENDにする
        //----------------------------------------------

        else
        {
            // 横幅をtextScaleまで大きくする
            Transform textTransform = this.transform;
            Vector3 scale = textTransform.localScale;
            if (textTransform.localScale.x < textScale.x)
            {
                scale.x += 6 * Time.deltaTime;
            }
            else
            {
                nextState = StateID.RESULT_END;
                flameCnt = 0;
            }

            textTransform.localScale = scale;

            //nextState = StateID.RESULT_END;
            //flameCnt = 0;
        }

        //----------------------------------------------
        // クリアテキストの挙動を制御する
        //----------------------------------------------

        if (screenWide <= 0.1f * flameCnt)
        {
            //Debug.Log(0.1f * flameCnt);
        }
        

    }

    //=============================================
    // *** リザルト演出終了　処理 ***
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
}
