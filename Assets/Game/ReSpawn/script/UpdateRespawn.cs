//---------------------------------
//担当：二宮怜
//内容：プレイヤーのリスポーン座標更新
//---------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateRespawn : MonoBehaviour
{
    // 変数宣言

    private string PlayerTag = "Player";

    private bool Used = false; // 既にセットしているか

    [Header("ステージ左から数えたリス地の順番")]public int RespawnNumber;

    private GameObject Child;
    private Vector3 RespawnPos;

    SetStage setstage = new SetStage();

    private bool Init = false;

    // 外部取得

    private StageManager stageManager;

    private PlayerStatas playerStatus; // リスポーン座標を持たせる

    // Start is called before the first frame update
    void Start()
    {
        // プレイヤー情報取得
        playerStatus = GameObject.Find("player").GetComponent<PlayerStatas>();

        Child = transform.GetChild(0).gameObject;

        // リスポーンさせる座標
        RespawnPos = Child.transform.position;
    }

    private void Update()
    {
        if(Init == false)
        {
            // 初期リスポーン地点オブジェクトならステージデータのステージプレハブの初期位置を取得
            stageManager = GameObject.Find("StageData").GetComponent<StageManager>();
            if (this.gameObject.name == "RespawnArea")
            {
                RespawnPos = stageManager.GetInitPlayerPos(setstage.GetAreaNum(), setstage.GetStageNum());
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 当たったのがプレイヤーなら
        if(collision.gameObject.tag == PlayerTag)
        {
            // 一度もリス設定していない
            if(Used == false)
            {
                // 自身より先にあるリスポーン座標を設定していなければ
                if(RespawnNumber > playerStatus.GetNowRespawnNum())
                {
                    // リスポーン設定
                    playerStatus.SetRespawnNum(RespawnNumber);
                    playerStatus.SetRespawn(RespawnPos);

                    Debug.Log("リスポーン地点更新");
                    Debug.Log(playerStatus.GetRespawn());

                    Used = true;
                }
                else
                {
                    Debug.Log("先のリスポーン地点が設定されています");
                }
            }
            else
            {
                Debug.Log("使用済み");
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        // 当たったのがプレイヤーなら
        if (collision.gameObject.tag == PlayerTag)
        {
            // 一度もリス設定していない
            if (Used == false)
            {
                // 自身より先にあるリスポーン座標を設定していなければ
                if (RespawnNumber > playerStatus.GetNowRespawnNum())
                {
                    // リスポーン設定
                    playerStatus.SetRespawnNum(RespawnNumber);
                    playerStatus.SetRespawn(RespawnPos);

                    Used = true;
                }
            }
        }
    }
}
