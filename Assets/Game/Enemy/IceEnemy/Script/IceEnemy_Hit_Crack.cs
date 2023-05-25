//----------------------------------------------------------
// 担当者：中川直登
// 内容  ：SelectSceneのプレイヤーの移動処理
//----------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class IceEnemy_Hit_Crack : MonoBehaviour
{
    [SerializeField]
    private GameObject Parent;
    private IceEnemy iceEnemy;
    private Collider2D _collision;
    private CrackCreater crack;
    
    // Use this for initialization
    void Start()
    {
        iceEnemy = Parent.GetComponent<IceEnemy>();
        if (iceEnemy == null) Debug.LogError("IceEnemyのコンポーネントを取得できませんでした。");
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        _collision = collision;
        if (_isHitCrack)
        {
            crack = _collision.gameObject.GetComponent<CrackCreater>();
            if (_isDamage)
            {
                Debug.Log("ｐ");
                iceEnemy.State = IceEnemy.StateID.DEATH;
                //this.gameObject.SetActive(false);
            }
        }
    }
    private bool _isHitCrack
    {
        get
        {
            if (_collision.transform.tag != "Crack") return false;
            if (iceEnemy.State == IceEnemy.StateID.DEATH) return false;
            return true;
        }
    }
    private bool _isDamage
    {
        get
        {
            if (crack == null) return false;
            if (!_isCreating)return false;
            return true;
        }
    }
    private bool _isCreating
    { 
        get 
        {
            if (crack.State == CrackCreater.CrackCreaterState.CREATING) return true;
            if (crack.State == CrackCreater.CrackCreaterState.ADD_CREATING) return true;
            return false;
        }
    }
}