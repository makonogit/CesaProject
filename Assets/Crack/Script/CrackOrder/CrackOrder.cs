//------------------------------------------------------------------------------
// �S���ҁF���쒼�o
// ���e  �F�Ђт��Ăяo��
//------------------------------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrackOrder : MonoBehaviour
{
    //------------------------------------------------------------------------------
    //�\�ϐ��錾�\
    [Header("�Ăяo���I�u�W�F")]
    public GameObject PrefabObject;
    [Header("�Ăяo����")]
    public float numSummon;

    [Header("�G��Ȃ��ł��������B")]
    public List<Vector2> Points;

    [System.NonSerialized]
    public EdgeCollider2D EC2D;

    Transform Trans;
    bool SetPointFlg = false;

    public bool CrackFlg = false;
    public float CrackAngle;
    public Vector3 CrackPos;

    public Vector3 RayDirection;    //���C�̌���
    public float RayAngle;

    public float Raylength = 0.5f;         //���C�̒���

    public int CreateNum = 0; // ���̂Ђт̐������Ɋ��蓖�Ă���

     //�Ђт̐������
    public enum CrackState
    {
        NoneCreate, //�������Ă��Ȃ�
        NowCreate,  //������
        OldCreate,  //�����I��
    }

    [SerializeField]
    public CrackState crackState = CrackState.NoneCreate;

    public Vector2 Hitpoint;

    //------------------------------------------------------------------------------
    //�\�����������\
    void Start()
    {
        EC2D = GetComponent<EdgeCollider2D>();
        Trans = GetComponent<Transform>();
        RayDirection = new Vector3(0,0,0);
        RayAngle = 0;
    }
    //------------------------------------------------------------------------------
    //�\�X�V�����\
    void Update()
    {
        //---------------------------------------------------------
        //Ray���΂��ĉ����ƂԂ������琶�����~�߂�

        //Ray�̌�����ݒ�
        RayDirection = new Vector3(Mathf.Cos((RayAngle + 90) * Mathf.PI / 180) , Mathf.Sin((RayAngle + 90) * Mathf.PI / 180) , 0);
        Vector3 origin = new Vector3(EC2D.points[EC2D.pointCount - 1].x, EC2D.points[EC2D.pointCount - 1].y + 0.0001f, 0.0f);
        Vector3 Distance = RayDirection * Raylength;

        RaycastHit2D hit = Physics2D.Raycast(origin, RayDirection, Raylength, -1);

        Debug.DrawRay(origin, Distance, Color.red);

        //---------------------------------------------------
        //�^�O�Ɠ���̏Փ˂�����Ō��point���W���Փ˂������W�ɍ��킹��
        if (hit)
        {
            if (hit.collider.gameObject.tag == "Crack")
            {
                Hitpoint = hit.point;
                EC2D.points[EC2D.pointCount - 1] = hit.point;
                numSummon = 0;
            }
        }


        if (CrackFlg)
        {
            // ��Ԃ𐶐����ɂ���
            crackState = CrackState.NowCreate;

            //---------------------------------------------------------
            // �Ăяo���I�u�W�F�̈ʒu��ݒ�
            GameObject obj = Instantiate(PrefabObject, CrackPos, Quaternion.Euler(new Vector3(0.0f, 0.0f, CrackAngle)), Trans);
            // �p�x�ݒ�
            obj.transform.localEulerAngles = new Vector3(0, 0, CrackAngle - 90);
            float radian = (CrackAngle - 90) * Mathf.PI / 180;
            RayAngle += CrackAngle - 90; 
            // �Ă񂾂̂ŌĂѐ������炷�B
            numSummon--;
            CrackFlg = false;
        }

        //---------------------------------------------------------
        // �Ăт�������A�|�C���g���Z�b�g���ĂȂ����
        if (numSummon <= 0 && SetPointFlg == false)
        {
            // ��Ԃ𐶐��ς݂ɂ���
            crackState = CrackState.OldCreate;
            SetPointFlg = true;
            // �|�C���g�Z�b�g
            //EC2D.SetPoints(Points);
            EC2D.offset = new Vector2(this.transform.position.x * -1, this.transform.position.y * -1);
        }
    }
}
