//---------------------------------
//�S���F�����S
//�����F�͈͓��ɂ���B��T��
//---------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NailSearch : MonoBehaviour
{
    NailStateManager manager;   //�B�̏�Ԃ��擾����X�N���v�g
    HammerNail hammer;          //�ł����ݏ�Ԃ��擾����X�N���v�g

    // Start is called before the first frame update
    void Start()
    {
        //�e�I�u�W�F�N�g��HammerNail���擾
        hammer = GetComponentInParent<HammerNail>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        //-------------------------------------------------
        //�͈͓��ɓB�������č\�����Ȃ�B�͈̔͂�����
        if(collision.gameObject.tag == "UsedNail")
        {
            //���������B�̏�ԃX�N���v�g���擾
            manager = collision.gameObject.GetComponent<NailStateManager>();

            //�ł����ݏ�Ԃɂ���ēB�̏�Ԃ�ύX
            if (hammer._HammerState == HammerNail.HammerState.NAILSET)
            {
                manager.SetState(NailStateManager.NailState.AREAVISUAL);
            }
            if(hammer._HammerState == HammerNail.HammerState.NONE ||
                hammer._HammerState == HammerNail.HammerState.HAMMER)
            {
                manager.SetState(NailStateManager.NailState.NOMAL);
            }

        }


    }

}
