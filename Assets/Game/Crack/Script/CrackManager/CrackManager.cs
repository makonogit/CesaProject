//---------------------------------------------------------
//
//担当者：中川直登
//
//内容　：ひびの管理
//
//---------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CrackManager : MonoBehaviour
{
    //-----------------------------------------------------
    // private variable
    //-----------------------------------------------------
    [SerializeField,Header("存在するひびのGameObjectリスト")]
    private List<GameObject> gameObjects;
    [SerializeField, Header("存在するひびのCrackCreaterリスト")]
    private List<CrackCreater> creaters;

    float nowTime;

    //-----------------------------------------------------
    // private method
    //-----------------------------------------------------
    // Use this for initialization
    //void Start()
    //{   
    //}

    // Update is called once per frame
    //void Update()
    //{
    //}

    //-----------------------------------------------------
    // public method
    //-----------------------------------------------------

    //
    // 関数：Init()
    //
    // 内容：初期化
    //
    public void Init()
    {
        // ひびを消す
        for (int i=0; i < gameObjects.Count; i++) 
        {
            Destroy(gameObjects[i]);
        }
        // 全要素を消す
        gameObjects.Clear();
        // 全要素を消す
        creaters.Clear();
    }

    //
    // 関数：AddCracInfokList(CrackInfo _info) 
    //
    // 内容：CrackInfoのリストに追加する。
    //
    public void AddCracInfokList(CrackInfo _info) 
    {
        gameObjects.Add(_info.gameObject);
        creaters.Add(_info.creater);
    }

    //
    // 関数： GetHitCrackState(Collider2D collider) 
    //
    // 内容：当たったひびの状態を返す
    //
    public CrackCreater.CrackCreaterState GetHitCrackState(Collider2D collider) 
    {
        // 検索して要素番号を返す
        int num = gameObjects.IndexOf(collider.gameObject);

        // 要素が見つからなかった時
        if (num < 0) return CrackCreater.CrackCreaterState.NONE;
        
        // 要素の状態を返す
        return creaters[num].GetState();
    }
    //
    // 関数： GetHitCrackState(GameObject _gameObject)
    //
    // 内容：当たったひびの状態を返す
    //
    public CrackCreater.CrackCreaterState GetHitCrackState(GameObject _gameObject)
    {
        // 検索して要素番号を返す
        int num = gameObjects.IndexOf(_gameObject.gameObject);

        // 要素が見つからなかった時
        if (num < 0) return CrackCreater.CrackCreaterState.NONE;

        // 要素の状態を返す
        return creaters[num].GetState();
    }

}

public struct CrackInfo 
{
    public GameObject gameObject;
    public CrackCreater creater;
    public void NullCheck() 
    {
        if (gameObject == null) Debug.LogError("gameObjectがNULLです。");
        if (creater == null) Debug.LogError("createrがNULLです。");
    }
}