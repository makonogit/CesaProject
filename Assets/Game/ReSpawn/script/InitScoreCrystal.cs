//---------------------------------------------------------
//担当者：二宮怜
//内容　：リスポーン時にリスポーン地点によってクリスタルの表示非表示を切り替え
//---------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitScoreCrystal : MonoBehaviour
{
    // 変数宣言

    // 子オブジェクトのスコアクリスタルを格納するリスト
    private List<CrystalObj> CrystalList = new List<CrystalObj>();

    private int ChildNum; // 子オブジェクトの数

    private bool Init = false;

    private GameObject player;
    private PlayerStatas playerStatus;

    // Start is called before the first frame update
    void Start()
    {
        // 子オブジェクトの数取得
        ChildNum = this.transform.childCount;

        // 子の数だけリストにゲームオブジェクト格納
        for(int i = 0; i < ChildNum; i++)
        {
            CrystalObj cryObj = new CrystalObj();

            // 子オブジェクト取得
            cryObj._CrystalObj = this.transform.GetChild(i).gameObject;
            // 子の座標取得
            cryObj._CrystalPos = cryObj._CrystalObj.transform.position;
            // スプライトレンダラー
            cryObj._renderer = cryObj._CrystalObj.GetComponent<SpriteRenderer>();
            // 取得済みか変数を持つスクリプト取得
            cryObj._CrystalNum = cryObj._CrystalObj.GetComponent<CrystalNum>();

            // 順に子を取得、追加
            CrystalList.Add(cryObj);

        }

        // プレイヤー関係
        player = GameObject.Find("player");
        playerStatus = player.GetComponent<PlayerStatas>();
    }

    // Update is called once per frame
    void Update()
    {
        // 初期化命令があった時だけ実行
        if(Init == true)
        {
            for (int i = 0;i < ChildNum; i++)
            {
                // リスポーン座標より左ならfalse
                if (CrystalList[i]._CrystalPos.x < playerStatus.GetRespawn().x)
                {
                    // リスポーンより手前で取得済みなら
                    if (CrystalList[i]._CrystalNum.Get == true)
                    {
                        // 非表示
                        CrystalList[i]._renderer.enabled = false;
                        //Debug.Log("falseにした");
                    }
                }
                else
                {
                    // リスポーンより奥で取得済みなら
                    if (CrystalList[i]._CrystalNum.Get == true)
                    {
                        // 表示
                        CrystalList[i]._renderer.enabled = true;
                        // 再取得可能にする
                        CrystalList[i]._CrystalNum.Get = false;
                        //Debug.Log("trueにした");
                    }
                }
            }

            Init = false;
        }
    }

    // リスポーン時にスコアクリスタルの初期化スタート
    public void ScoreCrystalInitStart()
    {
        Init = true;
    }
}

// クリスタルオブジェクトを初期化するための変数があるクラス
public class CrystalObj
{
    public GameObject _CrystalObj;
    public Vector3 _CrystalPos;
    public SpriteRenderer _renderer;
    public CrystalNum _CrystalNum;
}