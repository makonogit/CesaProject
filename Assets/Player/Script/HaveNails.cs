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

    public int GetNailNum = 10; // そのオブジェクトから獲得できる釘の数

    // 工具箱関連
    private GameObject toolbox;
    private ToolBoxManager toolboxManager;

    [Header("釘所持数")]
    public int NailsNum = 0; // 持っている釘の数

    private void Start()
    {
        toolbox = GameObject.Find("ToolBox");
        toolboxManager = toolbox.GetComponent<ToolBoxManager>();
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
            NailsNum++;
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
                NailsNum += GetNailNum;

                // 使用済みにする
                toolboxManager.isPassedNails = true;
            }
        }
    }
}
