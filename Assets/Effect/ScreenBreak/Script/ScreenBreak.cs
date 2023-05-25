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
    [SerializeField]
    private GameObject BreakManager;        //破片管理オブジェクト
    [Header("破片のオブジェクト")]
    public GameObject[] debris = new GameObject[3];// 破片用オブジェクト
    [Header("破片の生成数")]
    public int amount = 50;                        // 破片の生成数
    
    //[SerializeField,Header("画面端座標")]
    private float screenedge;

    // 二宮追加
    private Transform _playerTransform;

    // 音関連
    [Header("効果音")]
    public AudioClip sound1;// 音声ファイル
    AudioSource audioSource;// 取得したAudioSourceコンポーネント

    //============================================================
    // - 初期化処理 -

    void Start()
    {
        BreakManager = GameObject.Find("BreakCrystal");
        // AudioSourceコンポーネントを取得
        audioSource = GetComponent<AudioSource>();

        if (GameObject.Find("player"))
        {
            _playerTransform = GameObject.Find("player").GetComponent<Transform>();
        }
        Break();
    }

    //============================================================
    // - 画面を割る演習をする処理 -
    
    void Break()
    {
        //--------------------------------------------------------
        // 音声ファイルを再生する
        if (_playerTransform != null)
        {
            audioSource.PlayOneShot(sound1);
        }
        //--------------------------------------------------------
        // 破片を生成する

        for (int i = 0; i < amount; i++)
        {
            // ランダムに形、座標、大きさ、回転率を決定する
            int rndDebris = Random.Range(1, 101) % 5;
            int rndX = Random.Range(1, 20);
            int rndY = Random.Range(1, 20);
            float rndSizeX = Random.Range(0.1f, 2.0f);
            float rndSizeY = Random.Range(0.1f, 2.0f);
            float rndSize = Random.Range(0.1f, 2.0f);
            int rndRot = Random.Range(1, 360);

            // 破片を生成
            GameObject obj = Instantiate(debris[rndDebris], transform.position, Quaternion.identity);

            Transform objTransform = obj.transform;

            screenedge = _playerTransform.position.x;

            // 座標を変更
            Vector3 pos = Vector3.zero;
            pos.x = screenedge - 8f + 0.8f * rndX;
            pos.y = objTransform.position.y -8.0f + 0.8f * rndY;
            pos.z = 0.0f;

            // 大きさを変更
            Vector3 scale;
            scale.x = 0.6f * rndSize;
            scale.y = 0.6f * rndSize;
            scale.z = 1.0f;

            // 回転を変更
            Vector3 rot;
            rot.x = 1.0f;
            rot.y = 1.0f;
            rot.z = 1.0f * rndRot;

            // 変更を敵用する
            objTransform.localPosition = pos;    // 座標
            objTransform.localScale = scale;// 大きさ
            objTransform.eulerAngles = rot; // 回転

            obj.transform.parent = BreakManager.transform;
        }
    }
    //============================================================
}
