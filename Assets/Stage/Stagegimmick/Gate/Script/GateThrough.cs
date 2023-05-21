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
    private AudioSource boss_bgm;   //ボスBGM
    
    private GameObject Gateobj;     //生成したGate

    private CameraControl2 camera;

    private void Start()
    {
        // BGM再生システム取得
        stage_bgm = GameObject.Find("BGM(Loop)").GetComponent<AudioSource>();
        boss_bgm = GameObject.Find("BossBGM").GetComponent<AudioSource>();

        PlayerTrans = GameObject.Find("player").transform;

        // カメラ制御取得
        camera = GameObject.Find("Main Camera").GetComponent<CameraControl2>();

    }

    //[SerializeField, Header("ブロックのTransform")]
    //private Transform Blocktransform;

    private void Update()
    {
        // リスポーン処理
        if (Throgh && PlayerTrans.position.x < transform.position.x)
        {
            Destroy(Gateobj);   //生成したゲートを削除
            boss_bgm.Stop();    
            stage_bgm.Play();
            stage_bgm.volume = 0f; // 二宮追加
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

            stage_bgm.GetComponent<AudioSource>().Stop();
            boss_bgm.Play();
            Gateobj = Instantiate(GateBlock, transform);
            //Gateobj.transform.parent = null;
        }
    }
}
