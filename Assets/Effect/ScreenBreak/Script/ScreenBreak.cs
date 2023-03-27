//------------------------------------------------------------------------------
// 担当：藤原昂祐
// 内容：画面が割れた演出
//------------------------------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenBreak : MonoBehaviour
{
    //--------------------------------------------------------------------------
    // - 変数宣言 -

    // 破片関連
    [Header("破片のオブジェクト")]
    public GameObject[] debris = new GameObject[3];// 破片用オブジェクト
    [Header("破片の生成数")]
    public int amount = 50;                        // 破片の生成数

    // 音関連
    [Header("効果音")]
    public AudioClip sound1;// 音声ファイル
    AudioSource audioSource;// 取得したAudioSourceコンポーネント

    //============================================================
    // - 初期化処理 -

    void Start()
    {
        // AudioSourceコンポーネントを取得
        audioSource = GetComponent<AudioSource>();

        Break();
    }

    //============================================================
    // - 画面を割る演習をする処理 -
    
    void Break()
    {
        //--------------------------------------------------------
        // 音声ファイルを再生する
        audioSource.PlayOneShot(sound1);

        //--------------------------------------------------------
        // 破片を生成する

        for (int i = 0; i < amount; i++)
        {
            // ランダムに形、座標、大きさ、回転率を決定する
            int rndDebris = Random.Range(0, 3);
            int rndX = Random.Range(1, 20);
            int rndY = Random.Range(1, 20);
            int rndSizeX = Random.Range(1, 10);
            int rndSizeY = Random.Range(1, 10);
            int rndRot = Random.Range(1, 360);

            // 破片を生成
            GameObject obj = Instantiate(debris[rndDebris], transform.position, Quaternion.identity);

            Transform objTransform = obj.transform;

            // 座標を変更
            Vector3 pos = objTransform.position;
            pos.x = -8.0f + 0.8f * rndX;
            pos.y = -8.0f + 0.8f * rndY;
            pos.z = 0.0f;

            // 大きさを変更
            Vector3 scale;
            scale.x = 0.2f * rndSizeX;
            scale.y = 0.2f * rndSizeY;
            scale.z = 0.2f;

            // 回転を変更
            Vector3 rot;
            rot.x = 1.0f;
            rot.y = 1.0f;
            rot.z = 1.0f * rndRot;

            // 変更を敵用する
            objTransform.position = pos;    // 座標
            objTransform.localScale = scale;// 大きさ
            objTransform.eulerAngles = rot; // 回転
        }
    }
    //============================================================
}
