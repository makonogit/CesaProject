//------------------------------------------------------------------------------
// 担当：藤原昂祐
// 内容：リザルト
//------------------------------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Result : MonoBehaviour
{
    //--------------------------------------------------------------------------
    // - 変数宣言 -

    bool isFadeFlg = false;// 画面を割る処理のON、OFF

    // フェード関連
    [Header("フェードアウトする速度")]
    public float fadeSpeed = 0.001f;// フェードアウトする速度
    float alpha = 0.0f;             // パネルの透明度

    // 画面遷移関連
    int fadeDelay;// 画面遷移するまでの待機時間

    // 破片関連
    [Header("破片のオブジェクト")]
    public GameObject[] debris = new GameObject[3];// 破片用オブジェクト
    [Header("破片の生成数")]
    public int amount = 95;  // 破片の生成数
    GameObject[] debrisRist = new GameObject[95];

    // 音関連
    [Header("効果音")]
    public AudioClip sound1;// 音声ファイル
    AudioSource audioSource;// 取得したAudioSourceコンポーネント

    // クリアテキスト関連
    int[] clear = new int[]{

          1,1,1,0,1,0,0,0,1,1,1,0,0,1,0,0,1,1,0 ,
          1,0,0,0,1,0,0,0,1,0,0,0,1,0,1,0,1,0,1 ,
          1,0,0,0,1,0,0,0,1,1,1,0,1,1,1,0,1,1,1 ,
          1,0,0,0,1,0,0,0,1,0,0,0,1,0,1,0,1,0,1 ,
          1,1,1,0,1,1,1,0,1,1,1,0,1,0,1,0,1,0,1 ,
    };

    //============================================================
    // - 初期化処理 -

    void Start()
    {
        // AudioSourceコンポーネントを取得
        audioSource = GetComponent<AudioSource>();

        // この関数で演出をON、OFFする
        //SetFadeFlg(true);
    }

    //============================================================
    // - 更新処理 -

    void Update()
    {
        //--------------------------------------------------------
        // 画面が見えなくなるまでフェードアウト後、破片を生成する

        if (isFadeFlg == true)
        {
            if (alpha >= 1.0f)
            {
                //--------------------------------------------------------
                // 効果音を再生

                audioSource.PlayOneShot(sound1);

                //--------------------------------------------------------
                // 破片を生成

                for (int i = 0; i < amount; i++)
                {

                    // ランダムに形、座標、大きさ、回転率を取得する
                    int rndDebris = Random.Range(0, 3);
                    int rndX = Random.Range(0, 21);
                    int rndY = Random.Range(0, 21);
                    int rndSizeX = Random.Range(1, 10);
                    int rndSizeY = Random.Range(1, 10);
                    int rndRot = Random.Range(1, 360);

                    // 破片を生成
                    debrisRist[i] = Instantiate(debris[rndDebris], transform.position, Quaternion.identity);

                    // 破片のクリア後の座標を決定
                    if (clear[i] == 1)
                    {
                        ResultDebris resultDebris = debrisRist[i].GetComponent<ResultDebris>();
                        resultDebris.clearPos.x = transform.position.x + -7.0f + 0.8f * (i % 19);
                        resultDebris.clearPos.y = 4.0f - 0.8f * (i / 19);
                        resultDebris.clearPos.z = 2.0f;
                    }

                    Transform objTransform = debrisRist[i].transform;

                    // 座標を変更
                    Vector3 pos = objTransform.position;
                    pos.x = transform.position.x + -8.0f + 0.8f * rndX;
                    pos.y = -8.0f + 0.8f * rndY;
                    pos.z = 0.0f;

                    // 大きさを変更
                    Vector3 scale;
                    scale.x = 0.5f * rndSizeX;
                    scale.y = 0.5f * rndSizeY;
                    scale.z = 1.0f;

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

                //--------------------------------------------------------
                // フェード処理をOFFにする

                isFadeFlg = false;
                alpha = 0.0f;
                
            }
            else
            {

                //--------------------------------------------------------
                // 画面が見えなくなるまでフェードアウトする

                // 透明度を加算
                alpha += fadeSpeed;
            }

            // alphaの値を敵用する
            SpriteRenderer renderer = GetComponent<SpriteRenderer>();
            renderer.color = new Color(renderer.color.r, renderer.color.g, renderer.color.b, alpha);
        }

        //--------------------------------------------------------
        // 演出後フェードアウトしてセレクト画面に遷移する

        fadeDelay++;

        if(fadeDelay > 3500)
        {
            if(alpha < 1.0f)
            {
                //--------------------------------------------------------
                // 画面が見えなくなるまでフェードアウトする

                // 透明度を加算
                alpha += fadeSpeed;
                // alphaの値を敵用する
                SpriteRenderer renderer = GetComponent<SpriteRenderer>();
                renderer.color = new Color(renderer.color.r, renderer.color.g, renderer.color.b, alpha);
            }
            else
            {
                SceneManager.LoadScene("SelectScene");
            }
        }
    }

    //============================================================
    // - isScreenBreakFlg用セッター -

    public void SetFadeFlg(bool _isSet)
    {
        isFadeFlg = _isSet;
    }
    //============================================================
}
