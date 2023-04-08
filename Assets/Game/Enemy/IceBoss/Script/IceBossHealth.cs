//----------------------------------------------------------
// 担当者：中川直登
// 内容  ：氷エリアのボスの処理
//----------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class IceBossHealth : MonoBehaviour
{
    [SerializeField,Header("最大体力")]
    private int _maxHp;
    [SerializeField]
    private int _hp;

    private CrackCreater _crack;

    private string _tag = "Crack";



    // Use this for initialization
    void Start()
    {
        _hp = _maxHp;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // ひびなら
        if(collision.tag == _tag)
        {
            _crack = collision.GetComponent<CrackCreater>();
            // 生成中なら
            if (_crack.State == CrackCreater.CrackCreaterState.CREATING) 
            {
                _hp--;
            }
        }
    }

    public int HP 
    {
        get 
        {
            return _hp;
        }
    }
}