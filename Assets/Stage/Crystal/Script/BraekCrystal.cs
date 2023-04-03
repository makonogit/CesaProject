//--------------------------------
//担当：菅眞心
//内容：クリスタルの破壊
//--------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BraekCrystal : MonoBehaviour
{
    GameObject StageObj;
    StageStatas stageStatas;    //ステージのステータス管理

    //-----------------------------
    // 二宮追加
    private float SubX;    // 求めたX座標の差を保持する変数
    private float SubY;    // 求めたY座標の差を保持する変数
    private float Distace; // 求めた距離を保持する変数
    private float judgeDistance = 2.0f; // 判定をとる範囲

    // 外部取得
    private GameObject player;
    private Transform playerTransform;
    private Transform thisTransform;

    private PlayerStatas playerStatus;

    private GameObject PlayerInputMana;
    private PlayerInputManager ScriptPIManager;

    // Start is called before the first frame update
    void Start()
    {
        //-------------------------------------
        //ステージのステータスを取得
        StageObj = transform.root.gameObject;
        stageStatas = StageObj.transform.GetChild(0).GetComponent<StageStatas>();

        // プレイヤー探す
        player = GameObject.Find("player");
        // コンポーネント取得
        playerTransform = player.GetComponent<Transform>();
        thisTransform = GetComponent<Transform>();
        playerStatus = player.GetComponent<PlayerStatas>();

        // PlayerInputManager探す
        PlayerInputMana = GameObject.Find("PlayerInputManager");
        ScriptPIManager = PlayerInputMana.GetComponent<PlayerInputManager>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        //-----------------------------------------------------------
        // プレイヤーが一定範囲内でスマッシュしたらクリスタルが壊れる

        // プレイヤーとの距離をもとめる
        SubX = thisTransform.position.x - playerTransform.position.x; // x差
        SubY = thisTransform.position.y - playerTransform.position.y; // y差

        // 三平方の定理
        Distace = SubX * SubX + SubY * SubY; // プレイヤーとクリスタルの距離が求まった

        // 一定距離内にプレイヤーがいる
        if(Distace < judgeDistance)
        {
            // 同時押しされた
            if(/* ScriptPIManager.GetNail_Left() && ScriptPIManager.GetNail_Right() */
                collision.tag == "Crack")
            {
                stageStatas.SetStageCrystal(stageStatas.GetStageCrystal() - 1);
                Destroy(this.gameObject);
                // クリスタル破壊数増加
                playerStatus.AddBreakCrystal();
            }
        }
    }


}
