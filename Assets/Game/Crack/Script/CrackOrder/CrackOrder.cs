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
    [Header("1���Ăяo������")]
    public float WaitTime = 0.05f;
    [Header("�Ђт̃T�C�Y�ƌ������Ǝ��͈̔�")]
    public Vector3 _Scale;// �T�C�Y
    public Vector3 _decreaseRate = new Vector3(0.9f, 1.0f, 1.0f);// �T�C�Y�̌�����
    public Vector2 _nextAngleRange;// ���̊p�x�����߂�͈�
    [Header("�G��Ȃ��ł��������B")]
    public List<Vector2> Points;
    public List<GameObject> _crackObjects;// �Ђт̃��X�g
    [System.NonSerialized]
    public EdgeCollider2D EC2D;

    Transform Trans;
    bool SetPointFlg = false;

    public bool CrackFlg = false;
    public float CrackAngle;
    public Vector3 CrackPos;

    public Vector3 RayDirection;    //���C�̌���
    public float RayAngle;

    public float Raylength = 1.0f;         //���C�̒���

    public int CreateNum = 0; // ���̂Ђт̐������Ɋ��蓖�Ă���

    
    float CreateTime = 0.0f;      //�Ђт������������Ă��玞�Ԃ��v������(�G����������܂ł̎���)
    [SerializeField,Header("�G�Ƃ̓����蔻����擾���鎞��")]
    float EnemyHitTime = 0.5f;    //�G�̓����蔻�����鎞��

     //�Ђт̐������
    public enum CrackState
    {
        NoneCreate, //�������Ă��Ȃ�
        NowCreate,  //������
        OldCreate,  //�����I��
    }

    [SerializeField]
    public CrackState crackState = CrackState.NoneCreate;

    float _creatTime;// �Ђт𐶐����鎞��

    float _nextAngle;// ���̂Ђт̊p�x

    //------------------------------------------------------------------------------
    //�\�����������\
    void Start()
    {
        EC2D = GetComponent<EdgeCollider2D>();
        Trans = GetComponent<Transform>();
        RayDirection = new Vector3(0,0,0);
        RayAngle = 0;

        crackState = CrackState.NowCreate;
        // �I�t�Z�b�g
        EC2D.offset = new Vector2(this.transform.position.x * -1, this.transform.position.y * -1);

        // ���W���X�g�ǉ�
        Points.Add(this.transform.position);
       
        // ���͂��ꂽ����������
        _nextAngle = CrackAngle - 90;
        RayAngle = (_nextAngle + 90) * Mathf.PI / 180;
        
        
        numSummon--;
    }
    //------------------------------------------------------------------------------
    //�\�X�V�����\
    void Update()
    {
        _creatTime -= Time.deltaTime;

        //---------------------------------------------------------
        // �Ăяo����Ȃ�
        if (numSummon > 0 )
        {
            //---------------------------------------------------------
            //Ray���΂��ĉ����ƂԂ������琶�����~�߂�

            //Ray�̌�����ݒ�
            RayDirection = new Vector3(Mathf.Cos(RayAngle) , Mathf.Sin(RayAngle) , 0);
            Vector3 origin = new Vector3(Points[Points.Count - 1].x,Points[Points.Count - 1].y + 0.0001f, 0.0f);
            Vector3 Distance = RayDirection * _Scale.y;
            
            RaycastHit2D hit = Physics2D.Raycast(origin, RayDirection, Raylength, 3);

            Debug.DrawRay(origin, Distance, Color.red,1000,false);

            bool noHit = true;// �������Ă��Ȃ���
            //---------------------------------------------------
            //�^�O�Ɠ���̏Փ˂�����Ō��point���W���Փ˂������W�ɍ��킹��
            if (hit)
            {
                
                Debug.Log(hit.collider.gameObject.tag);
                if (hit.collider.gameObject.tag == "Crack" || hit.collider.gameObject.tag == "Ground")
                {
                    //Debug.Log(Points.Count);//���ڂœ���������
                    
                    if(hit.distance >= 0.2f) 
                    {
                        // �Ăяo���I�u�W�F�̈ʒu��ݒ�
                        Vector3 pos = new Vector3(Points[Points.Count - 1].x, Points[Points.Count - 1].y, 0);
                        pos += new Vector3(Mathf.Cos(hit.distance) * 0.5f, Mathf.Sin(hit.distance) * 0.5f,0) ;
                        _crackObjects.Add(Instantiate(PrefabObject, hit.point, new Quaternion(0, 0, 0, 0), Trans));
                        // �p�x�ݒ�
                        _crackObjects[_crackObjects.Count - 1].transform.localEulerAngles = new Vector3(0, 0, _nextAngle);
                        // �T�C�Y�ݒ�
                        _Scale = new Vector3(_Scale.x, hit.distance / 2, _Scale.z);
                        _crackObjects[_crackObjects.Count - 1].transform.localScale = _Scale;
                        // �ʒu�̍X�V
                        Points.Add(hit.point);
                        // �|�C���g�Z�b�g
                        EC2D.SetPoints(Points);
                    }
                    numSummon = 0;
                    noHit = false;
                }
            }

            //---------------------------------------------------
            //�Փ˂��Ă��Ȃ��Ȃ�Ăяo��
            if (noHit && _creatTime <= 0) 
            {
                //---------------------------------------------------
                // �Ăяo���I�u�W�F�̈ʒu��ݒ�
                Vector3 pos = new Vector3(Points[Points.Count - 1].x, Points[Points.Count - 1].y, 0);
                pos += new Vector3(RayDirection.x * (_Scale.y / 2), RayDirection.y * (_Scale.y / 2), 0);
                _crackObjects.Add(Instantiate(PrefabObject, pos, new Quaternion(0, 0, 0, 0), Trans));
                // �p�x�ݒ�
                _crackObjects[_crackObjects.Count - 1].transform.localEulerAngles = new Vector3(0, 0, _nextAngle);
                // �T�C�Y�ݒ�
                _crackObjects[_crackObjects.Count - 1].transform.localScale = _Scale;
                // �T�C�Y������������
                _Scale = new Vector3(_Scale.x * _decreaseRate.x, _Scale.y * _decreaseRate.y, _Scale.z * _decreaseRate.z);
               
                //---------------------------------------------------
                // �ʒu�̍X�V
                Vector2 nextPos = Points[Points.Count - 1];
                nextPos += new Vector2(RayDirection.x * _Scale.y, RayDirection.y * _Scale.y);
                Points.Add(nextPos);
                // �|�C���g�Z�b�g
                EC2D.SetPoints(Points);

                // ���̊p�x�����߂�
                _nextAngle += Random.Range(_nextAngleRange.x, _nextAngleRange.y);
                RayAngle = (_nextAngle + 90) * Mathf.PI / 180;// �X�V

                numSummon--;
                _creatTime = WaitTime;// ���Z�b�g
            }
            
        }
        //---------------------------------------------------------
        // �Ăт�������A�|�C���g���Z�b�g���ĂȂ����
        else if (SetPointFlg == false) 
        {
            //--------------------------------
            //�����㎞�Ԍv���A�Ђт𐶐��ς݂ɂ���
            if(CreateTime < EnemyHitTime)
            {
                CreateTime += Time.deltaTime;
            }
            else
            {
                // ��Ԃ𐶐��ς݂ɂ���
                crackState = CrackState.OldCreate;
                SetPointFlg = true;
            }
        }
        
    }
}
