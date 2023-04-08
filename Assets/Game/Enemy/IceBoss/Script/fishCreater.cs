//----------------------------------------------------------
// 担当者：中川直登
// 内容  ：魚生成
//----------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class fishCreater : MonoBehaviour
{
    [SerializeField,Header("魚のプレハブ")]
    private GameObject fishObj;
    private IceBoss boss;
    private CrackCreater _crack;
    private string _tag = "Crack";

    // Use this for initialization
    void Start()
    {
        boss = GameObject.Find("IceBoss").GetComponent<IceBoss>();
        if (boss == null) Debug.LogError("IceBossのコンポーネントを取得できませんでした。");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // ひびなら
        if (collision.tag == _tag)
        {
            _crack = collision.GetComponent<CrackCreater>();
            // 生成中なら
            if (_crack.State == CrackCreater.CrackCreaterState.CREATING)
            {
                if (boss.fish == null) 
                {
                    Vector2 pos = collision.ClosestPoint(transform.position);
                    boss.fish = Instantiate(fishObj, new Vector3(pos.x, pos.y, 0), Quaternion.identity);
                }
                
            }
        }
    }
}