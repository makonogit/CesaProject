//---------------------------------
//担当：二宮怜
//内容：被ダメージ時にノックバック
//---------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockBack : MonoBehaviour
{
    // 変数宣言

    // デバッグ用
    public bool knockback = false; // trueにしたらノックバックする
    [Header("ノックバック距離")]
    public Vector2 KnockBackPower = new Vector2(1f, 0.5f); // ノックバック距離
    [Header("コルーチンの繰り返し回数")]
    public int CoroutineNum = 10; // コルーチンを繰り返す回数
    private float direction = 1f; // プレイヤーの向き保存用変数

    private Rigidbody2D rigid2D; // rigidbody用変数
    private Transform thisTransform; // Transform用変数
    private GameObject thisObj; // このスクリプトがコンポーネントされたオブジェクト用変数

    // Start is called before the first frame update
    void Start()
    {
        rigid2D = GetComponent<Rigidbody2D>();

        thisTransform = GetComponent<Transform>();

        thisObj = gameObject;
    }

    private void Update()
    {
        if(knockback == true)
        {
            KnockBack_Func();

            knockback = false;
        }
    }

    public void KnockBack_Func()
    {
         // オブジェクトの向きを割り出す
         direction = thisTransform.localScale.x / Mathf.Abs(thisTransform.localScale.x); // 現在の自分のスケールをその値の絶対値で割って符号を入手

        StartCoroutine(KnockBack_Coroutine());
    }

    private IEnumerator KnockBack_Coroutine()
    {
        int i = 0;
        while (i < 10)
        {
            // 停止時間決定
            float WaitTime = 0.02f;

            // 指定時間処理停止
            yield return new WaitForSeconds(WaitTime);

            // 指定時間経過したので処理再開
            // ノックバック処理
            // プレイヤーの向いている方向と逆方向にノックバック
            thisTransform.Translate(-direction * KnockBackPower.x * WaitTime, KnockBackPower.y * WaitTime,0f);

            //カウント
            i++;

        }
    }
}
