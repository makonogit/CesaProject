//--------------------------------
//担当：菅眞心
//内容：クリスタルの破壊
//--------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BraekCrystal : MonoBehaviour
{
    //-----------------------------------------
    // 変数宣言

    private LayerMask BlockLayer = 14;

    GameObject StageObj;
    StageStatas stageStatas;    //ステージのステータス管理

    // 外部取得
    private GameObject player;
    private PlayerStatas playerStatus;
    private SpriteRenderer render;

    [SerializeField, Header("ひびのスプライト")]
    private Sprite Crack;

    private float ParentBreakTime; // 親オブジェクトが破壊された時間
    private bool Break = false;       //　破壊されたかどうか 
    private bool Add = false;

    private BreakBlock breakblock; // スクリプト取得用変数

   
    // Start is called before the first frame update
    void Start()
    {
        //-------------------------------------
        //ステージのステータスを取得
        StageObj = GameObject.Find("StageData");
        stageStatas = StageObj.transform.GetChild(0).GetComponent<StageStatas>();

        // プレイヤー探す
        player = GameObject.Find("player");
        playerStatus = player.GetComponent<PlayerStatas>();

        //　このオブジェクトのspriterenderer
        render = GetComponent<SpriteRenderer>();

        //Debug.Log(transform.parent.gameObject);
        // 親オブジェクトのレイヤーがBlock
        if (transform.parent.gameObject.layer == BlockLayer)
        {
            breakblock = transform.parent.gameObject.GetComponent<BreakBlock>();
        }
    }

    private void Update()
    {
        //if (breakblock != null)
        //{
        //    if (breakblock.Break == true && Add == false)
        //    {
        //        ParentBreakTime = Time.time;

        //        // コライダー追加
        //        var col = this.gameObject.AddComponent<CircleCollider2D>();
        //        col.radius = 12.5f;
        //        col.isTrigger = true;

        //        Add = true;
        //    }
        //}
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // ひびに当たったら
        if (collision.tag == "Crack" && !Break)
        {

            render.sprite = Crack;  //　スプライトの変更

            Destroy(collision.gameObject);
            stageStatas.SetStageCrystal(stageStatas.GetStageCrystal() - 1);

            // クリスタル破壊数増加
            //playerStatus.AddBreakCrystal();
            Break = true;
            //if (Time.time > ParentBreakTime)
            //{
               
            //}
        }
    }


}
