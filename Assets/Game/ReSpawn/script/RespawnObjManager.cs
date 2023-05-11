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
    public InitScoreCrystal _initScoreCrystal; // スコアクリスタル初期化用スクリプト

    // Start is called before the first frame update
    void Start()
    {
        _cameraZoom = GetComponent<CameraZoom>();
    }

    // Update is called once per frame
    void Update()
    {
        // リスポーンしたときに一度だけ実行
        if(Respawn == true)
        {
            Debug.Log("リスポーンマネージャー");

            // リスポーン時にリセットしたいオブジェクトの初期化関数呼び出し
            _initScoreCrystal.ScoreCrystalInitStart(); // スコアクリスタル
            _cameraZoom.RespawnInit();

            Respawn = false;
        }
    }

    public void RespawnInit()
    {
        Respawn = true;
    }
}
