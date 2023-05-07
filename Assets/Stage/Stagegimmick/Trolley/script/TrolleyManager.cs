//------------------------------------
//�@�S��:�����S
//�@���e�F�g���b�R�̊Ǘ�(SE)
//------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrolleyManager : MonoBehaviour
{
    private Trolleys_Move move = null; //�����Ă���g���b�R

    private GameObject SEobj;           //SE�Đ��p�I�u�W�F�N�g
    private GimmickPlaySound PlaySE;    //SE�Đ��p�X�N���v�g
    private bool stop = false;

    // Start is called before the first frame update
    void Start()
    {
        //�@SE�Đ��p
        SEobj = GameObject.Find("GimmickSE");
        PlaySE = SEobj.GetComponent<GimmickPlaySound>();
    }

    // Update is called once per frame
    void Update()
    {
        //�@�����Ă���g���b�R���~�܂�����SE���~�߂�
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
    //�@�����Ă���g���b�R�̏����擾
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
