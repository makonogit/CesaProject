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


    private CrackManager _manager;

    private bool _Damaged;


    private string _tag = "Crack";



    // Use this for initialization
    void Start()
    {
        GameObject Object = GameObject.Find("CrackManager");
        _manager = Object.GetComponent<CrackManager>();
        if (_manager == null) Debug.LogError("CrackManagerコンポーネントを取得できませんでした。");
        Init();
    }
    public void Init()
    {
        _hp = _maxHp;
        _Damaged = false;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        // ひびなら
        if(collision.tag == _tag)
        {
            HitCrack(collision);
        }
    }
    public void SetDamageFlag() 
    {
        _Damaged = false;
    }
    public int HP 
    {
        get 
        {
            return _hp;
        }
    }

    private void HitCrack(Collider2D collision) 
    {
        CrackCreater.CrackCreaterState _state= _manager.GetHitCrackState(collision);
        // 生成中なら
        if (_state == CrackCreater.CrackCreaterState.CREATING && !_Damaged)
        {
            _hp--;
            _Damaged = true;
        }
    }
}