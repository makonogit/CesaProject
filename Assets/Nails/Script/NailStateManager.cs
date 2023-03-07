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
        NOMAL,      //通常
        AREAVISUAL, //範囲が見える状態
    }

    [SerializeField,Header("釘の状態")]
    private NailState nailState;

    //private GameObject NailAreaObj;       //釘の範囲用オブジェクト
    private SpriteRenderer NailAreaSprite;  //釘の範囲用スプライト

    private SetNailList nailList;
    private GameObject Player;
    private HammerNail hammerNail;

    // Start is called before the first frame update
    void Start()
    {
        //最初は通常状態
        nailState = NailState.NOMAL;
        //子オブジェクト(エリア)のスプライトを取得
        NailAreaSprite = transform.GetChild(0).gameObject.GetComponentInChildren<SpriteRenderer>();

        //子オブジェクト状態を取得
        nailList = transform.GetChild(0).gameObject.GetComponentInChildren<SetNailList>();

        Player = GameObject.Find("player");
        hammerNail = Player.GetComponent<HammerNail>();
    }

    // Update is called once per frame
    void Update()
    {

        if(nailList.ThisPointNum != -1 && nailList.Crackend == false)
        {
            nailState = NailState.AREAVISUAL;
        }
        else
        {
            nailState = NailState.NOMAL;
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
