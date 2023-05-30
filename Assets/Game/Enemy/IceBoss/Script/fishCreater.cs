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

    private SpriteRenderer renderer;

    // Use this for initialization
    void Start()
    {
        boss = GameObject.Find("IceBoss").GetComponent<IceBoss>();
        if (boss == null) Debug.LogError("IceBossのコンポーネントを取得できませんでした。");
        renderer = GetComponent<SpriteRenderer>();
        if (renderer == null) Debug.LogError("SpriteRendererのコンポーネントを取得できませんでした。");
    }
    private void Update()
    {
        if (!renderer.enabled) 
        {
            createFish();
        }
    }

    private void createFish() 
    {
        if (boss.fish == null)
        {
            Vector3 pos = this.transform.position;
            pos = new Vector3(pos.x, -1, 0);
            boss.fish = Instantiate(fishObj, pos, Quaternion.identity);
        }
        Destroy(this.gameObject);
    }
}