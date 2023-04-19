//---------------------------------
//担当：二宮怜
//内容：ひびを生成したらたたいたところから粒が出てくる
//---------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateParticle : MonoBehaviour
{
    // 変数宣言

    // 生成するパーティクルのプレハブ
    public GameObject particle;

    // 一回たたいたら一度だけ生成
    private bool Create = false;

    // 外部取得
    private Hammer hammer;

    private void Start()
    {
        // プレイヤーの持つHammerスクリプト取得
        hammer = GetComponent<Hammer>();
    }

    // Update is called once per frame
    void Update()
    {
        // ひびを生成したタイミング
        if(hammer.hammerstate == Hammer.HammerState.HAMMER && Create == false)
        {
            // コルーチン呼び出し
            StartCoroutine(CreateObject());

            Create = true;
        } 


        if(hammer.hammerstate == Hammer.HammerState.NONE && Create == true)
        {
            Create = false;
        }
    }

    IEnumerator CreateObject()
    {
        // アニメーション分処理停止
        yield return new WaitForSeconds(0.25f);

        Vector2 worldPosition = hammer.CrackPointList[0];

        Instantiate(particle, new Vector3(worldPosition.x, worldPosition.y, 0f), Quaternion.identity);
    }
}
