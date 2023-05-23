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
    [SerializeField]
    private IceBoss boss;
    private string _tag = "Crack";

    // Use this for initialization
    void Start()
    {
        if (boss == null) Debug.LogError("IceBossのコンポーネントを取得できませんでした。");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // ひびなら
        if (collision.tag == _tag)
        {
            if (boss.fish == null)
            {
                Vector2 pos = collision.ClosestPoint(transform.position);
                boss.fish = Instantiate(fishObj, new Vector3(pos.x, pos.y, 0), Quaternion.identity);
            }
        }
    }
}