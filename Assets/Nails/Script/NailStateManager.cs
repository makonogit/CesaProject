//-------------------------------
//�S���F�����S
//�����F�B�̏�Ԃ��Ǘ�����
//-------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NailStateManager : MonoBehaviour
{
    public enum NailState
    {
        NOMAL,      //�ʏ�
        AREAVISUAL, //�͈͂���������
    }

    [SerializeField,Header("�B�̏��")]
    private NailState nailState;

    //private GameObject NailAreaObj;       //�B�͈̔͗p�I�u�W�F�N�g
    private SpriteRenderer NailAreaSprite;  //�B�͈̔͗p�X�v���C�g

    private SetNailList nailList;
    private GameObject Player;
    private HammerNail hammerNail;

    // Start is called before the first frame update
    void Start()
    {
        //�ŏ��͒ʏ���
        nailState = NailState.NOMAL;
        //�q�I�u�W�F�N�g(�G���A)�̃X�v���C�g���擾
        NailAreaSprite = transform.GetChild(0).gameObject.GetComponentInChildren<SpriteRenderer>();

        //�q�I�u�W�F�N�g��Ԃ��擾
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
        //��Ԃɂ���ăX�v���C�g�̓����x��ύX
        if (hammerNail._HammerState == HammerNail.HammerState.NAILSET)
        {
            if (nailState == NailState.AREAVISUAL)
            {
                NailAreaSprite.color = new Color(1.0f, 0.0f, 0.0f, 0.2f);   //������ԂȂ��
            }
            if (nailState == NailState.NOMAL)
            {
                NailAreaSprite.color = Color.clear; //�ʏ��Ԃ͓���
            }
        }
        else
        {
            NailAreaSprite.color = Color.clear; //�ʏ��Ԃ͓���
        }
    }

    //--------------------------------
    //��Ԃ��Z�b�g����֐�
    //--------------------------------
    public void SetState(NailState _nailstate)
    {
        nailState = _nailstate;
    }


    //--------------------------------
    //��Ԃ��l������֐�
    //--------------------------------
    public NailState GetState()
    {
        return nailState;
    }

}
