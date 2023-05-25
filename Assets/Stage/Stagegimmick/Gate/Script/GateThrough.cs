//--------------------------------------------
//　担当：菅眞心
//　内容：プレイヤーがエリアに入った時の処理
//--------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateThrough : MonoBehaviour
{
    [SerializeField, Header("生成するブロック")]
    private GameObject GateBlock;

    public bool Throgh = false;    // 通り抜けたか

    private Transform PlayerTrans;  //　プレイヤーのTransform

    private AudioSource stage_bgm;  //ステージBGM
    private AudioSource bossbgm_Intro;   // ボスBGMイントロ
    private AudioSource bossbgm_Loop;    // ボスBGMループ
    
    private GameObject Gateobj;     //生成したGate

    private CameraControl2 camera;

    private GameObject MainCamera;
    private BGMFadeManager _BGMfadeMana;
    private PlayBgm _playBGM;

    private void Start()
    {
        // BGM再生システム取得
        stage_bgm = GameObject.Find("BGM(Loop)").GetComponent<AudioSource>();
        bossbgm_Intro = GameObject.Find("BossBGM(Intro)").GetComponent<AudioSource>();
        bossbgm_Loop = GameObject.Find("BossBGM").GetComponent<AudioSource>();

        PlayerTrans = GameObject.Find("player").transform;

        MainCamera = GameObject.Find("Main Camera");

        // カメラ制御取得
        camera = MainCamera.GetComponent<CameraControl2>();
        // BGMフェードマネージャー取得
        _BGMfadeMana = MainCamera.GetComponent<BGMFadeManager>();
        _playBGM = MainCamera.GetComponent<PlayBgm>();

    }

    //[SerializeField, Header("ブロックのTransform")]
    //private Transform Blocktransform;

    private void Update()
    {
        // リスポーン処理
        if (Throgh && PlayerTrans.position.x < transform.position.x)
        {
            Destroy(Gateobj);   //生成したゲートを削除


            bossbgm_Intro.Stop();
            bossbgm_Intro.volume = 0f;
            bossbgm_Loop.Stop();

            // ステージBGM再生開始
            stage_bgm.Play();

            stage_bgm.volume = 0f; // 二宮追加
            _playBGM.StartBossBattle = false;

            camera.InitCamera();    //カメラ位置初期化
            Throgh = false;     //通り抜けフラグの初期化

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        
        // プレイヤーがすり抜けたら生成、BGMの再生
        if(collision.tag == "Player" && collision.gameObject.transform.position.x > transform.position.x && !Throgh)
        {
            Throgh = true;

            // ステージBGMフェードアウト(もともと音量は0)
            _BGMfadeMana.SmallStageBGM();
            //stage_bgm.GetComponent<AudioSource>().Stop();

            // ボスBGM再生開始&フェードイン
            bossbgm_Intro.Play();
            _BGMfadeMana.BigBossBGM();
            _playBGM.StartBossBattle = true;

            Gateobj = Instantiate(GateBlock, transform);
            //Gateobj.transform.parent = null;
        }
    }
}
