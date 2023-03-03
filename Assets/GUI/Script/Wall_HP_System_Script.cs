//----------------------------------------------------------
// 担当者：中川直登
// 内容  ：壁のHP
//----------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Wall_HP_System_Script : MonoBehaviour
{
    //-----------------------------------------------------------------
    //―変数―(公)Accessible variables
    
    // ナシ
    
    //-----------------------------------------------------------------
    //―変数―(私)Inaccessible variables
    private RectTransform RTrans;// サイズ調整するため。
    private float _nowHP;// 0～1.0f
    private float _maxSize;

    [Header("ダメージフィルター")]
    [SerializeField]
    private float _filter = 0.01f;

    //-----------------------------------------------------------------
    //―スタート処理―
    void Start()
    {
        // HPの初期化
        _nowHP = 1.0f;
        //--------------------------------------
        // RectTransformのコンポーネント取得
        RTrans = GetComponent<RectTransform>();
        if(RTrans == null) 
        {
            Debug.LogError("RectTransformのコンポーネントを取得できませんでした。");
        }
        // 最大サイズ保存
        _maxSize = RTrans.rect.width;
    }

    //-----------------------------------------------------------------
    //―更新処理―
    //void Update()
    //{}

    //-----------------------------------------------------------------
    //★★公開関数★★(公)

    //―HP設定関数―(公)
    public void SetHp(float _num) 
    {
        _nowHP = _num;
        // 値の制限
        HpLimit();
        // ゲージの更新
        RTrans.sizeDelta = new Vector2(_maxSize * _nowHP, RTrans.rect.height);
        
    }

    //―HP減少関数―(公)
    public void SubHp(float _num) 
    {
        _nowHP -= _num * _filter;
        // 値の制限
        HpLimit();
        // ゲージの更新
        RTrans.sizeDelta = new Vector2(_maxSize * _nowHP, RTrans.rect.height);
    }

    //-----------------------------------------------------------------
    //☆☆秘匿関数☆☆(私)

    //―HP制限関数―(私)
    private void HpLimit() 
    {
        _nowHP = (_nowHP < 0.0f ? 0.0f : _nowHP);// 下限
        _nowHP = (_nowHP > 1.0f ? 1.0f : _nowHP);// 上限
    }
}