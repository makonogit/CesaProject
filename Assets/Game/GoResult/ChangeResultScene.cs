//---------------------------------
//担当：二宮怜
//内容：条件を満たしたらリザルトシーンに移行
//---------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeResultScene : MonoBehaviour
{
    // - 変数宣言 -
    public float time = 0.0f; // クリスタルを全部壊してからの経過時間
    private float WaitTime = 2.0f; // シーン遷移するまでの待ち時間

    // 外部取得
    private GameObject player;
    private PlayerStatas playerStatus;
    private GameObject Resultobj;   // リザルト演出用のオブジェクト
    private Result result;          // リザルト演出用のスクリプト

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("player");
        playerStatus = player.GetComponent<PlayerStatas>();

        //------------------------------------------------
        // リザルト演出用のシステム取得
        //Resultobj = GameObject.Find("Result");
        //result = Resultobj.GetComponent<Result>();
    }

    // Update is called once per frame
    void Update()
    {
        // 3つのクリスタルを壊したらリザルト画面に移動
        if (playerStatus.GetBreakCrystalNum() == 3)
        {
            //playerStatus.AddBreakCrystal();
            //演出開始
            //result.SetFadeFlg(true);

            // リザルト画面へ
            SceneManager.LoadScene("newSelectScene");

            //// 待ち時間が経過したら
            //if (time > WaitTime)
            //{
            //    // リザルト画面へ
            //    SceneManager.LoadScene("newSelectScene");
            //}
        }
    }
}
