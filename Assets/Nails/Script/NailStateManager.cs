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
        NOMAL,      // �ʏ�
        THROW,      // �����Ă���r��
        AREAVISUAL, // �͈͂���������
    }

    [Header("�B�̏��")]
    public NailState nailState;

    //private GameObject NailAreaObj;       //�B�͈̔͗p�I�u�W�F�N�g
    private SpriteRenderer NailAreaSprite;  //�B�͈̔͗p�X�v���C�g

    private SetNailList nailList;
    private GameObject Player;
    private HammerNail hammerNail;

    private NailThrow nailThrow;        // �B�𓊂��Ă����Ԃ��擾
  
    // Start is called before the first frame update
    void Start()
    {
        //�ŏ����瓊���Ă�����
        nailState = NailState.THROW;
        //�q�I�u�W�F�N�g(�G���A)�̃X�v���C�g���擾
        NailAreaSprite = transform.GetChild(0).gameObject.GetComponentInChildren<SpriteRenderer>();

        //�q�I�u�W�F�N�g��Ԃ��擾
        nailList = transform.GetChild(0).gameObject.GetComponentInChildren<SetNailList>();

        //�B�̓����Ă����Ԃ��擾
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

            //�B�𓊂��I�������R���C�_�[��t����
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
