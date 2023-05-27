//---------------------------------
//担当：二宮怜
//内容：リスポーンを検知して、各親オブジェクトに初期化命令を出す
//---------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnObjManager : MonoBehaviour
{
    // 変数宣言

    private bool Respawn = false;

    private CameraZoom _cameraZoom; // リザルト用
    [SerializeField] private InitScoreCrystal _initScoreCrystal; // スコアクリスタル初期化用スクリプト
    [SerializeField] private RespawnBossInit _initBoss; // ボスの初期化用スクリプト
    private CrackManager _crackManager; // ひび管理スクリプト

    // Start is called before the first frame update
    void Start()
    {
        _cameraZoom = GetComponent<CameraZoom>();
        _crackManager = GameObject.Find("CrackManager").GetComponent<CrackManager>();
    }

    // Update is called once per frame
    void Update()
    {
        // リスポーンしたときに一度だけ実行
        if(Respawn == true)
        {
            //Debug.Log("リスポーンマネージャー");

            // リスポーン時にリセットしたいオブジェクトの初期化関数呼び出し
            _initScoreCrystal.ScoreCrystalInitStart(); // スコアクリスタル

            // ボスがいるなら初期化
            if (_initBoss != null)
            {
                _initBoss.RespawnInit(); // ボス
            }

            // カメラ関係初期化
            _cameraZoom.RespawnInit();

            // ひび全消去
            _crackManager.Init();

            Respawn = false;
        }
    }

    public void RespawnInit()
    {
        Respawn = true;
    }
}
