//-------------------------------
//�@�S���F�����S
//�@���e�F���̃M�~�b�N
//-------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SandControl : MonoBehaviour
{

    //---------------------------------
    //�@�ϐ��錾
    private SandManager sandmanager;    //�@���̃I�u�W�F�N�g�̊Ǘ��X�N���v�g
    private GameObject sandmanagerobj;  //�@�e�I�u�W�F�N�g
    private bool HitTrigger = false;    //�@1�񓖂��蔻��p
    private Vector3 ReleasePos;         //�@���o������W
    [SerializeField,Header("���o����鋗��")]
    private float ReleaseLength;

    [SerializeField,Header("���o�p���I�u�W�F�N�g")]
    private GameObject SandObj;
    private GameObject ReleasedSand;        //�@�����������I�u�W�F�N�g�ۑ��p
    private GameObject HitCrack;            //�@���������Ђт̃I�u�W�F�N�g
    private CrackCreater HitCrackCreater;   //�@���������Ђт�Creater
    private EdgeCollider2D Edge;            //�@�Փ˂���Edge��ۑ�

    private void Start()
    {
        //�@���̊Ǘ��X�N���v�g
        sandmanagerobj = GameObject.Find("SandManager");
        sandmanager = sandmanagerobj.GetComponent<SandManager>();
    }

    // Update is called once per frame
    void Update()
    {
        // �����������ꂽ����o�����悤�ɃA�j���[�V��������
        if(ReleasedSand != null)
        {
            //�@�ۗ�   
        }

        // �Ώۂ̂Ђт����݂��Ă����珈��
        if (HitCrack != null)
        {
            //�@�Ђт��̂΂��ꂽ��j�����čēx����
            if (HitCrackCreater.GetState() == CrackCreater.CrackCreaterState.ADD_CREATEBACK)
            {
                Debug.Log("�̂т�");
                Destroy(ReleasedSand);
                HitTrigger = false;
            }
        }
        else
        {
            // �ړ��ȂǂŖ����Ȃ�΍ēx�ݒ��Ԃ�
            Destroy(ReleasedSand);
            HitTrigger = false;
        }
    }

    private void OnParticleCollision(GameObject other)
    {
        // �Ђтɓ���������1�񏈗�
        if (other.tag == "Crack" && !HitTrigger)
        {
            Debug.Log("hibi");
            //�@�I�u�W�F�N�g�̏���ۑ�
            HitCrack = other;
            HitCrackCreater = other.transform.parent.gameObject.GetComponent<CrackCreater>();

            // EdgeCollider�̏����擾
            Edge = other.GetComponent<EdgeCollider2D>();

            // EdgeCollider�̏I�_����p�[�e�B�N������o
            var EdgePos = Edge.points[Edge.pointCount - 1];

            // �Ђт̌����ɂ���č��W���w��
            ReleasePos = Edge.points[Edge.pointCount - 1].x > transform.position.x ?
                new Vector3(EdgePos.x + ReleaseLength / 4, EdgePos.y - 0.1f, 1.0f) :
                new Vector3(EdgePos.x - ReleaseLength / 4, EdgePos.y - 0.1f, 1.0f);
                
            //�@�������W�ɍ����Ȃ���ΐ���
            if (!sandmanager.GetSand().Contains(ReleasePos))
            {
                ReleasedSand = Instantiate(SandObj, ReleasePos, Quaternion.identity);
                ReleasedSand.transform.localScale = new Vector3(ReleaseLength, 1.0f, 1.0f);
                ReleasedSand.transform.parent = sandmanagerobj.transform;
                sandmanager.SetSand(ReleasePos);
            }

            HitTrigger = true;
        }


        //------------------------------------------------
        //�@�v���C���[�ƏՓ˂����牟���Ԃ�
        if(other.tag == "Player")
        {
            PlayerMove move = other.GetComponent<PlayerMove>();
            other.transform.Translate(-6.0f * (move.GetMovement().x * move.BaseSpeed * Time.deltaTime), 0.0f, 0.0f);
        }
     

    }

}
