//----------------------------------------------------------
// 担当者：中川直登
// 内容  ：魚生成
//----------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class IceFishCreater : MonoBehaviour
{
    [SerializeField, Header("魚入りの氷プレハブ")]
    private GameObject fishObj;
    //[SerializeField]private IceBoss boss;

    private GameObject nowIce;
    private bool _created;
    private float nowTime;
    [SerializeField, Header("氷が出てくるまでのクールタイム")]
    private float coolTime=10.0f; 
    // Use this for initialization
    void Start()
    {
        //boss = GameObject.Find("IceBoss").GetComponent<IceBoss>();
        //if (boss == null) Debug.LogError("IceBossのコンポーネントを取得できませんでした。");
        _created = false;
        nowTime = 0.0f;
    }
    private void Update()
    {
        if (nowIce == null && _created) 
        {
            nowTime += Time.deltaTime;
        }
        if(coolTime < nowTime) 
        {
            _created = false;
        }
            
        if (nowIce == null && !_created)
        {
            nowIce = Instantiate(fishObj, this.transform.position, Quaternion.identity);
            _created = true;
            nowTime = 0.0f;
        }
    }
    
}