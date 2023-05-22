//---------------------------------------------------------
//担当者：二宮怜
//内容　：背景オブジェクトの星が降る
//---------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingStar : MonoBehaviour
{
    //-変数宣言-

    [Header("星が降る速度")]
    [SerializeField] private float FallSpeed = 10.0f;
    [Header("リスポーンまでの時間")]
    [SerializeField] private float RespawnTime = 3.0f;
    // 経過時間
    private float time = 0.0f;

    // 外部取得
    private Transform thisTransform;
    private GameObject parent;
    private CreateRandomPosition _createRandomPos;

    private void Start()
    {
        // Transform取得
        thisTransform = GetComponent<Transform>();

        parent = transform.parent.gameObject;
        _createRandomPos = parent.GetComponent<CreateRandomPosition>();

        // 星の降る速度をランダムに
        // 10～20
        FallSpeed = Random.Range(10.0f, 20.0f);
    }

    // Update is called once per frame
    void Update()
    {
        // 時間経過
        time += Time.deltaTime;

        // 生まれてから一定時間経過したら座標を決めなおしてリスポーン
        if(time > RespawnTime || thisTransform.position.y < -6f)
        {
            // ある範囲からランダムな座標を取得してその場に移動させる
            var pos = _createRandomPos.GetSpawnPos();

            thisTransform.position = new Vector3(pos.x, pos.y, 0f);

            // 速度再設定
            FallSpeed = Random.Range(10.0f, 20.0f);

            // 初期化
            time = 0f;
        }

        // 落ちる方向
        Vector3 fall = new Vector3(-0.5f, -1.0f, 0.0f);

        // 落ちる方向に現在地から移動させる
        thisTransform.Translate(fall * FallSpeed * Time.deltaTime);
    }
}
