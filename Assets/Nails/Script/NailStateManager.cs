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

    // Start is called before the first frame update
    void Start()
    {
        //�ŏ��͒ʏ���
        nailState = NailState.NOMAL;
        //�q�I�u�W�F�N�g(�G���A)�̃X�v���C�g���擾
        NailAreaSprite = transform.GetChild(0).gameObject.GetComponentInChildren<SpriteRenderer>();

    }

    // Update is called once per frame
    void Update()
    {
        //-----------------------------------------
        //��Ԃɂ���ăX�v���C�g�̓����x��ύX
        if (nailState == NailState.NOMAL)
        {
            NailAreaSprite.color = Color.clear; //�ʏ��Ԃ͓���
        }
        if (nailState == NailState.AREAVISUAL)
        {
            NailAreaSprite.color = new Color(1.0f, 0.0f, 0.0f, 0.5f);   //������ԂȂ��
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
