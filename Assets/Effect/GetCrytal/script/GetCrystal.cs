//---------------------------------------
//担当者：中川直登
//内容　：クリスタルを取得した時のエフェクト管理
//---------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class GetCrystal : MonoBehaviour
{
    [SerializeField,Header("クリスタル（演出用）")]
    private GameObject crystal;
    private FlyChase flyChase;
    [SerializeField, Header("プレイヤー")]
    private GameObject player;
    private bool _start;
    private HaveCrystal haveCrystal;

    // Use this for initialization
    void Start()
    {
        
        flyChase = crystal.GetComponent<FlyChase>();
        if (flyChase == null) Debug.LogError("FlyChaseコンポーネントを取得できませんでした。");
        if(player == null) Debug.LogError("設定されていません");
        haveCrystal = player.GetComponent<HaveCrystal>();
        if (haveCrystal == null) Debug.LogError("HaveCrystalコンポーネントを取得できませんでした。");
    }

    // Update is called once per frame
    void Update()
    {
        if (_start) Creating();
    }

    private void Creating() 
    {
        flyChase.SetStart(this.transform,haveCrystal);
        GameObject obj = Instantiate(crystal, player.transform.position, Quaternion.identity);
        _start = false;
    }


    public void Creat()
    { _start = true; }
}