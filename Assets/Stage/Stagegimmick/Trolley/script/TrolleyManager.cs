//------------------------------------
//　担当:菅眞心
//　内容：トロッコの管理(SE)
//------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrolleyManager : MonoBehaviour
{
    private Trolleys_Move move = null; //動いているトロッコ

    private GameObject SEobj;           //SE再生用オブジェクト
    private GimmickPlaySound PlaySE;    //SE再生用スクリプト
    private bool stop = false;

    // Start is called before the first frame update
    void Start()
    {
        //　SE再生用
        SEobj = GameObject.Find("GimmickSE");
        PlaySE = SEobj.GetComponent<GimmickPlaySound>();
    }

    // Update is called once per frame
    void Update()
    {
        //　動いているトロッコが止まったらSEを止める
        if(move != null)
        {
            if (stop && PlaySE.NowSE() == GimmickPlaySound.GimmickSEList.TOLOLLEY_LOOP)
            {
                //PlaySE.Stop();
                move = null;
            }
        }
    }

    //---------------------------------------
    //　動いているトロッコの情報を取得
    public void SetMoveTrolley(Trolleys_Move _move)
    {
        move = _move;
        PlaySE.PlayerGimmickSE(GimmickPlaySound.GimmickSEList.TOLOLLEY_LOOP);
    }

    public void SetStop(bool _stop)
    {
        stop = _stop;
    }

}
