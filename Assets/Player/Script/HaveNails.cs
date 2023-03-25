//---------------------------------------
//担当者：二宮
//内容　：プレイヤーが持つ釘の数を管理
//---------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HaveNails : MonoBehaviour
{
    //---------------------------------------------------------
    // - 変数宣言 -
    private string NailTag = "Nail"; //タグ名
    private string ToolBoxTag = "ToolBox"; // タグ名

    private int GetNailNum = 10; // そのオブジェクトから獲得できる釘の数

    // 工具箱関連
    private GameObject toolbox;
    private ToolBoxManager toolboxManager;

    [Header("釘所持数")]
    private int NailsNum = 0; // 持っている釘の数

    // 外部取得
    private GameObject player;
    private PlayerStatas status;

    private void Start()
    {
        toolbox = GameObject.Find("ToolBox");
        toolboxManager = toolbox.GetComponent<ToolBoxManager>();

        player = GameObject.Find("player");
        status = player.GetComponent<PlayerStatas>();
    }

    private void Update()
    {
        //エンターで釘所持数が増える(デバック用)
        if (Input.GetKeyDown(KeyCode.Z))
        {
            NailsNum++;
        }
    }

    //落ちている釘に触れると釘所持数が増える
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //---------------------------------------------------------
        // 触れたのがアイテムとしての釘なら釘所持数増える

        // タグが釘なら
        if (collision.tag == NailTag)
        {
            status.SetNail(status.GetNail() + 1);
            // アイテムとしての釘は消す
            Destroy(collision.gameObject);
        }

        // タグが工具箱なら
        if(collision.tag == ToolBoxTag)
        {
            // まだ釘を渡していない工具箱なら
            if (!toolboxManager.isPassedNails)
            {
                // オブジェクトごとに獲得数は指定する（初期値は10）
                status.SetNail(status.GetNail() + GetNailNum);

                // 使用済みにする
                toolboxManager.isPassedNails = true;
            }
        }
    }
}
