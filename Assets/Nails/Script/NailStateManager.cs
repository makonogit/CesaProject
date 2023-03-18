//-------------------------------
//担当：菅眞心
//役割：釘の状態を管理する
//-------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NailStateManager : MonoBehaviour
{
    public enum NailState
    {
        NOMAL,      // 通常
        THROW,      // 投げている途中
        AREAVISUAL, // 範囲が見える状態
    }

    [Header("釘の状態")]
    public NailState nailState;

    //private GameObject NailAreaObj;       //釘の範囲用オブジェクト
    private SpriteRenderer NailAreaSprite;  //釘の範囲用スプライト

    private SetNailList nailList;
    private GameObject Player;
    private HammerNail hammerNail;

    private NailThrow nailThrow;        // 釘を投げている状態を取得
  
    // Start is called before the first frame update
    void Start()
    {
        //最初から投げている状態
        nailState = NailState.THROW;
        //子オブジェクト(エリア)のスプライトを取得
        NailAreaSprite = transform.GetChild(0).gameObject.GetComponentInChildren<SpriteRenderer>();

        //子オブジェクト状態を取得
        nailList = transform.GetChild(0).gameObject.GetComponentInChildren<SetNailList>();

        //釘の投げている状態を取得
        nailThrow = GetComponent<NailThrow>();

        Player = GameObject.Find("player");
        hammerNail = Player.GetComponent<HammerNail>();
      
    }

    // Update is called once per frame
    void Update()
    {

        if(nailThrow.NailDistance > 0)
        {
            nailState = NailState.THROW;
        }
        else if(nailList.ThisPointNum != -1 && nailList.Crackend == false)
        {
            nailState = NailState.AREAVISUAL;
        }
        else
        {
            nailState = NailState.NOMAL;
        }


        if(nailThrow.NailDistance <= 0)
        {

            //釘を投げ終わったらコライダーを付ける
            if (!gameObject.GetComponentInChildren<CircleCollider2D>())
            {
                gameObject.transform.GetChild(0).gameObject.AddComponent<CircleCollider2D>();
                CircleCollider2D circleCollider2D = gameObject.GetComponentInChildren<CircleCollider2D>();
                circleCollider2D.isTrigger = true;
            }

            if (!gameObject.GetComponent<PolygonCollider2D>())
            {
                gameObject.AddComponent<PolygonCollider2D>();
                PolygonCollider2D polygon = gameObject.GetComponent<PolygonCollider2D>();
                polygon.isTrigger = true;
            }

        }

        //-----------------------------------------
        //状態によってスプライトの透明度を変更
        if (hammerNail._HammerState == HammerNail.HammerState.NAILSET)
        {
            if (nailState == NailState.AREAVISUAL)
            {
                NailAreaSprite.color = new Color(1.0f, 0.0f, 0.0f, 0.2f);   //可視化状態なら赤
            }
            if (nailState == NailState.NOMAL)
            {
                NailAreaSprite.color = Color.clear; //通常状態は透明
            }
        }
        else
        {
            NailAreaSprite.color = Color.clear; //通常状態は透明
        }
    }

    //--------------------------------
    //状態をセットする関数
    //--------------------------------
    public void SetState(NailState _nailstate)
    {
        nailState = _nailstate;
    }


    //--------------------------------
    //状態を獲得する関数
    //--------------------------------
    public NailState GetState()
    {
        return nailState;
    }

}
