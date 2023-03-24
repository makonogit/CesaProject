//------------------------------------------------------------------------------
// �S���ҁF���쒼�o
// ���e  �F�Ђт����
//------------------------------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateCrack : MonoBehaviour
{
    //------------------------------------------------------------------------------
    //�\�ϐ��錾�\
    [Header("���I�u�W�F�̍ŏ����ő備�p�x")]
    public Vector2 nextAngleRange;
    [Header("���I�u�W�F�̃T�C�Y������")]
    public Vector3 decreaseRate = new Vector3(0.9f, 1.0f, 1.0f);
    [Header("���I�u�W�F�̌Ăяo������")]
    public float waitTime = 0.01f;
    [Header("�Ăяo���I�u�W�F")]
    public GameObject PrefabObject;
    Transform Trans;
    bool nextSummon = false;
    CrackOrder Order;
    

    //------------------------------------------------------------------------------
    //�\�����������\

    void Start()
    {
        Trans = GetComponent<Transform>();
        Order = GetComponentInParent<CrackOrder>();
        // EdgeCollider2D�ɂ��̃I�u�W�F�̈ʒu��ݒ肷��
        Order.Points.Add(this.transform.position);
        //����Point���X�V����
        Order.EC2D.SetPoints(Order.Points);

    }

    //------------------------------------------------------------------------------
    //�\�X�V�����\

    void Update()
    {
        // �҂����Ԃ̏���
        waitTime -= Time.deltaTime;
        //---------------------------------------------------------
        // �҂����Ԃ�0�ȉ��̎��A�I�[�_�[�̐����܂����鎞�A���̃I�u�W�F�N�g���Ăяo���ĂȂ���
        if (waitTime <= 0 && Order.numSummon > 0 && nextSummon == false)
        {
            //---------------------------------------------------------
            // �q�̃I�u�W�F�N�g�̊p�x�����߂�ix�`y�̊Ԃ������_���Łj
            float angle =Random.Range(nextAngleRange.x, nextAngleRange.y);
            // �q�I�u�W�F�̂��T�C�Y�̔���
            float radius = decreaseRate.y / 2;
            // ���W�A�������߂�
            float radian = ((angle + 90.0f) / 180.0f) * Mathf.PI;

            //---------------------------------------------------------
            // �q�I�u�W�F�����
            GameObject obj = Instantiate(PrefabObject, Trans);

            // �q�I�u�W�F�̈ʒu�i�e���猩���j
            obj.transform.localPosition = new Vector3(radius * Mathf.Cos(radian), 1.0f + radius * Mathf.Sin(radian), 0);
            // �q�I�u�W�F�̊p�x�i�e���猩���j
            obj.transform.localEulerAngles = new Vector3(0, 0, angle);
            // �q�I�u�W�F�̃T�C�Y�i�e���猩���j
            obj.transform.localScale = decreaseRate;
            

            Order.RayAngle += angle;
            //Order.RayDirection = Vector3.up;
            //---------------------------------------------------------
            nextSummon = true;
            Order.numSummon--;
        }
     
    }

}
