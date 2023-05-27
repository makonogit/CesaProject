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
    [SerializeField]
    private Animator _anim;

    private CrackManager _manager;

    private bool _Damaged;


    private string _tag = "Crack";

    private bool on;
    private float nowTime;

    // 二宮追加
    [SerializeField] RenderOnOff _renderOnOff;

    // Use this for initialization
    void Start()
    {
        GameObject Object = GameObject.Find("CrackManager");
        _manager = Object.GetComponent<CrackManager>();
        if (_manager == null) Debug.LogError("CrackManagerコンポーネントを取得できませんでした。");
        Init();
    }

    private void Update()
    {
        _anim.SetBool("IsDamage", on);
        if (on) 
        {
            nowTime += Time.deltaTime;
        }
        if(nowTime > 2) 
        {
            on = false;
            nowTime = 0.0f;
        }
    }

    public void Init()
    {
        _hp = _maxHp;
        _Damaged = false;
        nowTime = 0.0f;
        on = false;
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
            on = true;
            Destroy(collision.gameObject);

            // 点滅
            _renderOnOff.SetFlash(true, 1.5f);
        }
    }
}