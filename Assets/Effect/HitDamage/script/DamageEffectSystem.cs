//---------------------------------------
//担当者：中川直登
//内容　：クリスタルを取得した時のエフェクト管理
//---------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class DamageEffectSystem : MonoBehaviour
{
    [SerializeField, Header("クリスタル（演出用）")]
    private GameObject crystal;
    private FlyHammer flyHammer;
    [SerializeField, Header("プレイヤー")]
    private GameObject player;
    private bool _start;
    private GameOver gameOver; // ゲームオーバー画面遷移用スクリプト取得用変数

    // Use this for initialization
    void Start()
    {

        flyHammer = crystal.GetComponent<FlyHammer>();
        if (flyHammer == null) Debug.LogError("FlyHammerコンポーネントを取得できませんでした。");
        if (player == null) Debug.LogError("設定されていません");
        gameOver = player.GetComponent<GameOver>();
        if (gameOver == null) Debug.LogError("gameoverコンポーネントを取得できませんでした。");
     
    }

    // Update is called once per frame
    void Update()
    {
        if (_start) Creating();
    }

    private void Creating()
    {
        flyHammer.SetStart(this.transform,gameOver);
        GameObject obj = Instantiate(crystal, player.transform.position, Quaternion.identity);
        _start = false;
    }


    public void Creat()
    { _start = true; }
}